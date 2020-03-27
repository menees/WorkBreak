namespace WorkBreak
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Linq;
	using System.Media;
	using System.Reflection;
	using System.Text;
	using System.Threading;
	using System.Windows.Forms;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	internal class Program : ApplicationContext
	{
		#region Private Data Members

		private System.Windows.Forms.Timer timer;
		private NotifyIcon trayIcon;
		private StatusWindow statusWindow;
		private DateTime? workStartedUtc;
		private TimeSpan totalSnoozeTime;
		private bool startedScreenSaver;

		#endregion

		#region Constructors

		private Program()
		{
			// Load options first.
			using (ISettingsStore store = ApplicationInfo.CreateUserSettingsStore())
			{
				Options.Load(store.RootNode);
			}

			// Create a tray icon with no parent form.
			// http://bluehouse.wordpress.com/2006/01/24/how-to-create-a-notify-icon-in-c-without-a-form/
			this.trayIcon = new NotifyIcon();
			this.trayIcon.Visible = true;
			this.trayIcon.Icon = new Icon(Properties.Resources.Icon16_Stopwatch_00, SystemInformation.SmallIconSize);
			this.trayIcon.ContextMenuStrip = new ContextMenuStrip();
			this.trayIcon.DoubleClick += this.ShowStatus_Click;

			// Windows uses the tooltip text to identify an icon for its tray notification settings.
			// So the tooltip needs to be predictable and consistent for Windows to match it up
			// correctly.  If this includes variable data (e.g., working time), then Windows will
			// treat each instance as a unique instance, which sucks.
			this.trayIcon.Text = ApplicationInfo.ApplicationName;

			// Set up the context menu.
			ToolStripItemCollection menuItems = this.trayIcon.ContextMenuStrip.Items;
			menuItems.Add("&Show Status", null, this.ShowStatus_Click);
			menuItems.Add("&Reset Timer", null, this.ResetTimer_Click);
			menuItems.Add("-");
			menuItems.Add("Options...", null, this.Options_Click);
			menuItems.Add("About...", null, this.About_Click);
			menuItems.Add("-");
			menuItems.Add("E&xit", null, this.Exit_Click);

			// Create the status window, but don't show it yet.
			this.statusWindow = new StatusWindow();
			this.statusWindow.VisibleChanged += this.StatusWindow_VisibleChanged;

			// Set up the timer.
			this.timer = new System.Windows.Forms.Timer();
			this.timer.Interval = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;
			this.timer.Tick += this.Timer_Tick;
			this.timer.Start();
		}

		#endregion

		#region Private Enums

		private enum WorkStatus
		{
			Idle,
			Working,
			BreakRequired,
		}

		#endregion

		#region Private Properties

		private TimeSpan CurrentActualWorkTime
		{
			get
			{
				TimeSpan result = TimeSpan.Zero;

				if (this.workStartedUtc != null)
				{
					result = DateTime.UtcNow - this.workStartedUtc.Value;
				}

				return result;
			}
		}

		private TimeSpan CurrentAllowedWorkTime
		{
			get
			{
				TimeSpan result = Options.MaxWorkTime + this.totalSnoozeTime;
				return result;
			}
		}

		private WorkStatus Status
		{
			get
			{
				WorkStatus result = WorkStatus.Idle;

				if (this.workStartedUtc != null)
				{
					if (this.CurrentActualWorkTime > this.CurrentAllowedWorkTime)
					{
						result = WorkStatus.BreakRequired;
					}
					else
					{
						result = WorkStatus.Working;
					}
				}

				return result;
			}
		}

		#endregion

		#region Protected Methods

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.trayIcon != null)
				{
					this.trayIcon.Dispose();
					this.trayIcon = null;
				}

				if (this.timer != null)
				{
					this.timer.Dispose();
					this.timer = null;
				}

				if (this.statusWindow != null)
				{
					this.statusWindow.Dispose();
					this.statusWindow = null;
				}
			}

			base.Dispose(disposing);
		}

		#endregion

		#region Main Entry Point

		[STAThread]
		private static void Main()
		{
			using (Mutex mutex = new Mutex(true, "Menees.WorkBreak", out bool createdNew))
			{
				if (createdNew)
				{
					WindowsUtility.InitializeApplication("Work Break", null);
					using Program program = new Program();
					Application.Run(program);

					// Force the mutex to live the whole life of the program
					// (just in case the JITer decides we're not really using it).
					GC.KeepAlive(mutex);
				}
				else
				{
					WindowsUtility.ShowError(null, "Another instance of Work Break is running.");
				}
			}
		}

		#endregion

		#region Private Methods

		private static int GetProgressImageIndex(double percentage, bool reverse)
		{
			// This depends on the images used by SetProgressImage.
			const int MaxImageIndex = 12;

			// Use Round instead of Floor so we'll always get to the start and end images.
			double percentImage = percentage * MaxImageIndex;
			int imageIndex = (int)Math.Round(percentImage);
			int result = Math.Max(0, Math.Min(imageIndex, MaxImageIndex));

			// Reverse the image order so it will appear to "count down" backward.
			if (reverse)
			{
				result = MaxImageIndex - result;
			}

			return result;
		}

		private void ClearWorkTime(bool updateUI = true)
		{
			this.workStartedUtc = null;
			this.totalSnoozeTime = TimeSpan.Zero;
			this.startedScreenSaver = false;

			if (updateUI)
			{
				this.UpdateUI();
			}
		}

		private void UpdateUI()
		{
			// Set or clear the start of the work time.
			TimeSpan idleTime = NativeMethods.GetTimeSinceLastInput();
			if (idleTime >= Options.MinBreakTime)
			{
				this.ClearWorkTime(false);
			}
			else if (this.workStartedUtc == null)
			{
				this.workStartedUtc = DateTime.UtcNow;
			}

			WorkStatus status = this.Status;
			this.SetProgressImages(status, idleTime);

			switch (status)
			{
				case WorkStatus.Idle:
					this.statusWindow.Hide();
					this.statusWindow.UpdateStatus(false, string.Empty);
					break;

				case WorkStatus.Working:
					this.statusWindow.UpdateStatus(false, this.BuildMessage(status));
					break;

				case WorkStatus.BreakRequired:
					// Only start the screen saver once per required break period.
					// If the user immediately exits the screen saver and keeps
					// working, we'll still display the status window.
					TimeSpan? timeUntilScreenSaver = null;
					if (Options.StartScreenSaver && !this.startedScreenSaver)
					{
						timeUntilScreenSaver = (this.CurrentAllowedWorkTime + Options.ScreenSaverDelay) - this.CurrentActualWorkTime;
						if (timeUntilScreenSaver <= TimeSpan.Zero)
						{
							this.startedScreenSaver = true;
							NativeMethods.StartScreenSaver();
						}
					}

					this.statusWindow.UpdateStatus(true, this.BuildMessage(status, idleTime, timeUntilScreenSaver));
					this.ShowStatus(false);
					break;
			}
		}

		private void SetProgressImages(WorkStatus status, TimeSpan idleTime)
		{
			if (Options.ShowProgress && status != WorkStatus.Idle)
			{
				if (status == WorkStatus.Working)
				{
					double percentage = this.CurrentActualWorkTime.TotalMinutes / this.CurrentAllowedWorkTime.TotalMinutes;
					int imageIndex = GetProgressImageIndex(percentage, false);
					this.SetProgressImage(imageIndex);
				}
				else
				{
					double percentage = idleTime.TotalSeconds / Options.MinBreakTime.TotalSeconds;
					int imageIndex = GetProgressImageIndex(percentage, true);
					this.SetProgressImage(imageIndex);
				}
			}
			else
			{
				this.SetProgressImage(0);
			}
		}

		private void SetProgressImage(int imageIndex)
		{
			// If these images change, then GetProgressImageIndex also needs to be changed.
			switch (imageIndex)
			{
#pragma warning disable MEN010 // Avoid magic numbers. These are image indexes for positions on the clock face.
				case 1:
					this.trayIcon.Icon = Properties.Resources.Icon16_Stopwatch_01;
					this.statusWindow.UpdateProgressImage(Properties.Resources.Png48_Stopwatch_01);
					this.statusWindow.Icon = Properties.Resources.Icon32_Stopwatch_01;
					break;

				case 2:
					this.trayIcon.Icon = Properties.Resources.Icon16_Stopwatch_02;
					this.statusWindow.UpdateProgressImage(Properties.Resources.Png48_Stopwatch_02);
					this.statusWindow.Icon = Properties.Resources.Icon32_Stopwatch_02;
					break;

				case 3:
					this.trayIcon.Icon = Properties.Resources.Icon16_Stopwatch_03;
					this.statusWindow.UpdateProgressImage(Properties.Resources.Png48_Stopwatch_03);
					this.statusWindow.Icon = Properties.Resources.Icon32_Stopwatch_03;
					break;

				case 4:
					this.trayIcon.Icon = Properties.Resources.Icon16_Stopwatch_04;
					this.statusWindow.UpdateProgressImage(Properties.Resources.Png48_Stopwatch_04);
					this.statusWindow.Icon = Properties.Resources.Icon32_Stopwatch_04;
					break;

				case 5:
					this.trayIcon.Icon = Properties.Resources.Icon16_Stopwatch_05;
					this.statusWindow.UpdateProgressImage(Properties.Resources.Png48_Stopwatch_05);
					this.statusWindow.Icon = Properties.Resources.Icon32_Stopwatch_05;
					break;

				case 6:
					this.trayIcon.Icon = Properties.Resources.Icon16_Stopwatch_06;
					this.statusWindow.UpdateProgressImage(Properties.Resources.Png48_Stopwatch_06);
					this.statusWindow.Icon = Properties.Resources.Icon32_Stopwatch_06;
					break;

				case 7:
					this.trayIcon.Icon = Properties.Resources.Icon16_Stopwatch_07;
					this.statusWindow.UpdateProgressImage(Properties.Resources.Png48_Stopwatch_07);
					this.statusWindow.Icon = Properties.Resources.Icon32_Stopwatch_07;
					break;

				case 8:
					this.trayIcon.Icon = Properties.Resources.Icon16_Stopwatch_08;
					this.statusWindow.UpdateProgressImage(Properties.Resources.Png48_Stopwatch_08);
					this.statusWindow.Icon = Properties.Resources.Icon32_Stopwatch_08;
					break;

				case 9:
					this.trayIcon.Icon = Properties.Resources.Icon16_Stopwatch_09;
					this.statusWindow.UpdateProgressImage(Properties.Resources.Png48_Stopwatch_09);
					this.statusWindow.Icon = Properties.Resources.Icon32_Stopwatch_09;
					break;

				case 10:
					this.trayIcon.Icon = Properties.Resources.Icon16_Stopwatch_10;
					this.statusWindow.UpdateProgressImage(Properties.Resources.Png48_Stopwatch_10);
					this.statusWindow.Icon = Properties.Resources.Icon32_Stopwatch_10;
					break;

				case 11:
					this.trayIcon.Icon = Properties.Resources.Icon16_Stopwatch_11;
					this.statusWindow.UpdateProgressImage(Properties.Resources.Png48_Stopwatch_11);
					this.statusWindow.Icon = Properties.Resources.Icon32_Stopwatch_11;
					break;

				case 12:
#pragma warning restore MEN010 // Avoid magic numbers
					this.trayIcon.Icon = Properties.Resources.Icon16_Stopwatch_12;
					this.statusWindow.UpdateProgressImage(Properties.Resources.Png48_Stopwatch_12);
					this.statusWindow.Icon = Properties.Resources.Icon32_Stopwatch_12;
					break;

				default:
					this.trayIcon.Icon = Properties.Resources.Icon16_Stopwatch_00;
					this.statusWindow.UpdateProgressImage(Properties.Resources.Png48_Stopwatch_00);
					this.statusWindow.Icon = Properties.Resources.Icon32_Stopwatch_00;
					break;
			}
		}

		private string BuildMessage(WorkStatus status, TimeSpan? idleTime = null, TimeSpan? timeUntilScreenSaver = null)
		{
			StringBuilder sb = new StringBuilder();

			TimeSpan workTime = this.CurrentActualWorkTime;
			int minutes = (int)workTime.TotalMinutes;
			sb.AppendFormat(
				"You have been working for {0} minute{1}.",
				minutes,
				minutes == 1 ? string.Empty : "s");

			if (status == WorkStatus.BreakRequired)
			{
				double totalBreakSeconds = Options.MinBreakTime.TotalSeconds;
				double idleSeconds = idleTime.GetValueOrDefault().TotalSeconds;
				int remainingSeconds = (int)Math.Ceiling(totalBreakSeconds - idleSeconds);
				sb.AppendLine();
				sb.AppendLine();
				sb.AppendFormat("Please take a {0} second break and relax.", remainingSeconds);

				// Warn if within one minute of screen saver starting.
				if (timeUntilScreenSaver != null && timeUntilScreenSaver.Value > TimeSpan.Zero
					&& timeUntilScreenSaver.Value.Ticks < TimeSpan.TicksPerMinute)
				{
					int secondsUntil = (int)Math.Ceiling(timeUntilScreenSaver.Value.TotalSeconds);
					sb.AppendLine();
					sb.AppendFormat(
						"The screen saver will start in {0} second{1}.",
						secondsUntil,
						secondsUntil == 1 ? string.Empty : "s");
				}
			}

			return sb.ToString();
		}

		private void ShowStatus(bool userInitiated)
		{
			if (!this.statusWindow.Visible)
			{
				this.statusWindow.Show();

				if (!userInitiated)
				{
					if (Options.PlaySound)
					{
						SystemSounds.Asterisk.Play();
					}

					// See Microsoft's guidelines about notifications: http://msdn.microsoft.com/en-us/library/aa511497.aspx
					// Flashing 3 times is warranted since this needs to get the user's immediate attention.
					const int FlashCount = 3;
					NativeMethods.FlashWindow(this.statusWindow, FlashCount);
				}
			}
		}

		#endregion

		#region Private Event Handlers

		private void ShowStatus_Click(object sender, EventArgs e)
		{
			this.UpdateUI();
			this.ShowStatus(true);
		}

		private void ResetTimer_Click(object sender, EventArgs e)
		{
			this.ClearWorkTime();
		}

		private void Options_Click(object sender, EventArgs e)
		{
			using (OptionsDialog dialog = new OptionsDialog())
			{
				if (dialog.Execute())
				{
					this.UpdateUI();
				}
			}
		}

#pragma warning disable CC0091 // Use static method. Designer likes instance event handlers.
		private void About_Click(object sender, EventArgs e)
#pragma warning restore CC0091 // Use static method
		{
			WindowsUtility.ShowAboutBox(null, Assembly.GetExecutingAssembly());
		}

		private void Exit_Click(object sender, EventArgs e)
		{
			this.trayIcon.Visible = false;
			Application.Exit();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			this.UpdateUI();
		}

		private void StatusWindow_VisibleChanged(object sender, EventArgs e)
		{
			if (!this.statusWindow.Visible)
			{
				WorkStatus status = this.Status;

				// If we're in the idle or working states, then closing/hiding
				// the status window shouldn't have any side effects.
				if (status == WorkStatus.BreakRequired)
				{
					if (this.statusWindow.Dismissed)
					{
						// The user explicitly hit the Dismiss button when told that
						// a break was required, so clear the working time.
						this.ClearWorkTime();
					}
					else
					{
						// The user hit the snooze button or the 'X'/Close button,
						// so snooze a little bit before warning them again.  We can't
						// just add SnoozeTime to totalSnoozeTime; we have to also
						// include any extra time overage.  Suppose the user works
						// 1:30 over and then hits Snooze for 1 minute.  We need to
						// set 2:30 as the total snooze time.  If they work another
						// 0:30 over then hit 1 min Snooze, we'd set total to 4:00.
						this.totalSnoozeTime = (this.CurrentActualWorkTime - Options.MaxWorkTime) + Options.SnoozeTime;
					}
				}
			}
		}

		#endregion
	}
}
