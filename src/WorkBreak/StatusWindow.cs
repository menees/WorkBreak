namespace WorkBreak
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Linq;
	using System.Text;
	using System.Windows.Forms;
	using Menees;
	using Microsoft.Win32;

	#endregion

	internal partial class StatusWindow : Form
	{
		#region Private Data Members

		private static readonly int[] DefaultSnoozeTimes = new[] { 1, 3, 5, 10 };

		#endregion

		#region Constructors

		public StatusWindow()
		{
			this.InitializeComponent();
			this.Icon = Properties.Resources.Icon32_Stopwatch_00;

			SystemEvents.SessionEnded += (s, e) => Application.Exit();
		}

		#endregion

		#region Public Properties

		/// <summary>
		///  Gets whether the break time warning was explicitly dismissed.
		/// </summary>
		public bool Dismissed
		{
			get;
			private set;
		}

		#endregion

		#region Protected Properties

		protected override bool ShowWithoutActivation
		{
			get
			{
				// We don't want this window to steal focus from other windows when it is shown.
				// That's also why we're using ToolStrip controls (since they don't take focus).
				// http://stackoverflow.com/questions/156046/show-a-form-without-stealing-focus
				// Other ideas at:
				// http://www.dreamincode.net/forums/topic/133745-show-a-window-without-taking-focus/
				return true;
			}
		}

		#endregion

		#region Public Methods

		public void UpdateStatus(bool snoozeVisible, string message)
		{
			this.statusMessage.Text = message;
			this.snoozeButton.Visible = snoozeVisible;
		}

		public void UpdateProgressImage(Image image)
		{
			this.progressImage.Image = image;
		}

		#endregion

		#region Internal Methods

		internal static void MoveNearSystemTray(Form form)
		{
			Screen primaryScreen = Screen.PrimaryScreen;
			if (primaryScreen != null)
			{
				const int Padding = 5;
				Rectangle primaryArea = primaryScreen.WorkingArea;
				int newX = primaryArea.Right - form.Width - Padding;
				int newY = primaryArea.Bottom - form.Height - Padding;
				Point newLocation = new(newX, newY);
				form.Location = newLocation;
			}
		}

		#endregion

		#region Private Methods

		private void AddSnoozeItem(TimeSpan snoozeTime, bool isCurrent)
		{
			ToolStripMenuItem item = new();
			int minutes = (int)snoozeTime.TotalMinutes;
			if (minutes == 1)
			{
				item.Text = "1 minute";
			}
			else
			{
				item.Text = minutes + " minutes";
			}

			item.Tag = snoozeTime;
			item.Checked = isCurrent;
			item.Click += this.SnoozeTimeItem_Click;

			this.snoozeTimes.Items.Add(item);
		}

		private void Snooze(TimeSpan snoozeTime)
		{
			Options.SnoozeTime = snoozeTime;
			this.Hide();
		}

		#endregion

		#region Private Event Handlers

		private void StatusWindow_Load(object? sender, EventArgs e)
		{
			// If the user has never positioned the window before, then move it near the system tray.
			if (!this.formSaver.Load())
			{
				MoveNearSystemTray(this);
			}
		}

		private void StatusWindow_VisibleChanged(object? sender, EventArgs e)
		{
			if (this.Visible)
			{
				// Clear this on each display so the program can
				// determine why the window was "closed"/hidden.
				this.Dismissed = false;

				// Populate the snooze times context/dropdown menu.
				// Re-populate each time, so we're assured that the current
				// value is present and checked.
				this.snoozeTimes.Items.Clear();
				bool containsCurrent = false;
				foreach (int minutes in DefaultSnoozeTimes)
				{
					TimeSpan snoozeTime = TimeSpan.FromMinutes(minutes);
					bool isCurrent = snoozeTime == Options.SnoozeTime;
					if (isCurrent)
					{
						containsCurrent = true;
					}

					this.AddSnoozeItem(snoozeTime, isCurrent);
				}

				if (!containsCurrent)
				{
					this.AddSnoozeItem(Options.SnoozeTime, true);
				}
			}
		}

		private void StatusWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			// If the user clicks the 'X'/close button on the window caption,
			// we just want to hide the window.
			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				this.Hide();
			}
		}

		private void DismissButton_Click(object? sender, EventArgs e)
		{
			this.Dismissed = true;
			this.Hide();
		}

		private void SnoozeButton_ButtonClick(object? sender, EventArgs e)
		{
			this.Snooze(Options.SnoozeTime);
		}

		private void SnoozeTimeItem_Click(object? sender, EventArgs e)
		{
			if (sender is ToolStripMenuItem item && item.Tag is TimeSpan snoozeTime)
			{
				this.Snooze(snoozeTime);
			}
		}

#pragma warning disable CC0091 // Use static method. Designer likes instance event handlers.
		private void FormSaver_SaveSettings(object sender, SettingsEventArgs e)
#pragma warning restore CC0091 // Use static method
		{
			// Save the latest options while we have the chance
			// since we may have updated the SnoozeTime.
			Options.Save(e.SettingsNode);
		}

		#endregion
	}
}
