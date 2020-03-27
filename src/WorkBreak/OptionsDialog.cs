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
	using Menees.Windows.Forms;

	#endregion

	internal partial class OptionsDialog : Form
	{
		#region Constructors

		public OptionsDialog()
		{
			this.InitializeComponent();
			this.Icon = Properties.Resources.Icon32_Stopwatch_02;
		}

		#endregion

		#region Internal Methods

		internal bool Execute()
		{
			this.workTimeMinutes.Value = (int)Options.MaxWorkTime.TotalMinutes;
			this.breakTimeSeconds.Value = (int)Options.MinBreakTime.TotalSeconds;
			this.showProgress.Checked = Options.ShowProgress;
			this.playSound.Checked = Options.PlaySound;
			this.startScreenSaver.Checked = Options.StartScreenSaver;
			this.excessDelayMinutes.Value = (int)Options.ScreenSaverDelay.TotalMinutes;

			bool result = this.ShowDialog() == DialogResult.OK;
			return result;
		}

		#endregion

		#region Private Methods

		private void UpdateControls()
		{
			bool enable = this.startScreenSaver.Checked;
			this.excessDelayLabel.Enabled = enable;
			this.excessDelayMinutes.Enabled = enable;
		}

		#endregion

		#region Private Event Handlers

		private void OptionsDialog_Load(object sender, EventArgs e)
		{
			this.UpdateControls();

			if (!this.formSaver.Load())
			{
				StatusWindow.MoveNearSystemTray(this);
			}
		}

		private void StartScreenSaver_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateControls();
		}

		private void Okay_Click(object sender, EventArgs e)
		{
			// Update the options in the OK handler so the latest values
			// will be saved when the SaveSettings handler is called.
			Options.MaxWorkTime = TimeSpan.FromMinutes((double)this.workTimeMinutes.Value);
			Options.MinBreakTime = TimeSpan.FromSeconds((double)this.breakTimeSeconds.Value);
			Options.ShowProgress = this.showProgress.Checked;
			Options.PlaySound = this.playSound.Checked;
			Options.StartScreenSaver = this.startScreenSaver.Checked;
			Options.ScreenSaverDelay = TimeSpan.FromMinutes((double)this.excessDelayMinutes.Value);
		}

#pragma warning disable CC0091 // Use static method. Designer likes instance event handlers.
		private void FormSaver_SaveSettings(object sender, SettingsEventArgs e)
#pragma warning restore CC0091 // Use static method
		{
			// Save the latest options while we have the chance.
			Options.Save(e.SettingsNode);
		}

		#endregion
	}
}
