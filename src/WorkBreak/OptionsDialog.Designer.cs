namespace WorkBreak
{
	internal partial class OptionsDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.NumericUpDown excessDelayMinutes;
		private System.Windows.Forms.Label excessDelayLabel;
		private System.Windows.Forms.CheckBox startScreenSaver;
		private System.Windows.Forms.CheckBox playSound;
		private System.Windows.Forms.CheckBox showProgress;
		private System.Windows.Forms.NumericUpDown breakTimeSeconds;
		private System.Windows.Forms.NumericUpDown workTimeMinutes;
		private Menees.Windows.Forms.FormSaver formSaver;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}

			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.Button okay;
			System.Windows.Forms.Button cancel;
			System.Windows.Forms.GroupBox groupBox1;
			System.Windows.Forms.Label breakTimeLabel;
			System.Windows.Forms.Label workTimeLabel;
			System.Windows.Forms.GroupBox groupBox2;
			this.breakTimeSeconds = new System.Windows.Forms.NumericUpDown();
			this.workTimeMinutes = new System.Windows.Forms.NumericUpDown();
			this.excessDelayMinutes = new System.Windows.Forms.NumericUpDown();
			this.excessDelayLabel = new System.Windows.Forms.Label();
			this.startScreenSaver = new System.Windows.Forms.CheckBox();
			this.playSound = new System.Windows.Forms.CheckBox();
			this.showProgress = new System.Windows.Forms.CheckBox();
			this.formSaver = new Menees.Windows.Forms.FormSaver(this.components);
			okay = new System.Windows.Forms.Button();
			cancel = new System.Windows.Forms.Button();
			groupBox1 = new System.Windows.Forms.GroupBox();
			breakTimeLabel = new System.Windows.Forms.Label();
			workTimeLabel = new System.Windows.Forms.Label();
			groupBox2 = new System.Windows.Forms.GroupBox();
			groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.breakTimeSeconds)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.workTimeMinutes)).BeginInit();
			groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.excessDelayMinutes)).BeginInit();
			this.SuspendLayout();
			// 
			// okay
			// 
			okay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			okay.DialogResult = System.Windows.Forms.DialogResult.OK;
			okay.Location = new System.Drawing.Point(136, 255);
			okay.Name = "okay";
			okay.Size = new System.Drawing.Size(87, 27);
			okay.TabIndex = 2;
			okay.Text = "OK";
			okay.UseVisualStyleBackColor = true;
			okay.Click += new System.EventHandler(this.Okay_Click);
			// 
			// cancel
			// 
			cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			cancel.Location = new System.Drawing.Point(232, 255);
			cancel.Name = "cancel";
			cancel.Size = new System.Drawing.Size(87, 27);
			cancel.TabIndex = 3;
			cancel.Text = "Cancel";
			cancel.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			groupBox1.Controls.Add(this.breakTimeSeconds);
			groupBox1.Controls.Add(this.workTimeMinutes);
			groupBox1.Controls.Add(breakTimeLabel);
			groupBox1.Controls.Add(workTimeLabel);
			groupBox1.Location = new System.Drawing.Point(8, 8);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(312, 88);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = "Timing";
			// 
			// breakTimeSeconds
			// 
			this.breakTimeSeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.breakTimeSeconds.Location = new System.Drawing.Point(244, 51);
			this.breakTimeSeconds.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
			this.breakTimeSeconds.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.breakTimeSeconds.Name = "breakTimeSeconds";
			this.breakTimeSeconds.Size = new System.Drawing.Size(56, 23);
			this.breakTimeSeconds.TabIndex = 3;
			this.breakTimeSeconds.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
			// 
			// workTimeMinutes
			// 
			this.workTimeMinutes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.workTimeMinutes.Location = new System.Drawing.Point(244, 23);
			this.workTimeMinutes.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
			this.workTimeMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.workTimeMinutes.Name = "workTimeMinutes";
			this.workTimeMinutes.Size = new System.Drawing.Size(56, 23);
			this.workTimeMinutes.TabIndex = 1;
			this.workTimeMinutes.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
			// 
			// breakTimeLabel
			// 
			breakTimeLabel.AutoSize = true;
			breakTimeLabel.Location = new System.Drawing.Point(8, 52);
			breakTimeLabel.Name = "breakTimeLabel";
			breakTimeLabel.Size = new System.Drawing.Size(181, 15);
			breakTimeLabel.TabIndex = 2;
			breakTimeLabel.Text = "Mininum Break Time In Seconds:";
			// 
			// workTimeLabel
			// 
			workTimeLabel.AutoSize = true;
			workTimeLabel.Location = new System.Drawing.Point(8, 24);
			workTimeLabel.Name = "workTimeLabel";
			workTimeLabel.Size = new System.Drawing.Size(184, 15);
			workTimeLabel.TabIndex = 0;
			workTimeLabel.Text = "Maximum Work Time In Minutes:";
			// 
			// groupBox2
			// 
			groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			groupBox2.Controls.Add(this.excessDelayMinutes);
			groupBox2.Controls.Add(this.excessDelayLabel);
			groupBox2.Controls.Add(this.startScreenSaver);
			groupBox2.Controls.Add(this.playSound);
			groupBox2.Controls.Add(this.showProgress);
			groupBox2.Location = new System.Drawing.Point(8, 100);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(312, 144);
			groupBox2.TabIndex = 1;
			groupBox2.TabStop = false;
			groupBox2.Text = "Display";
			// 
			// excessDelayMinutes
			// 
			this.excessDelayMinutes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.excessDelayMinutes.Location = new System.Drawing.Point(164, 107);
			this.excessDelayMinutes.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.excessDelayMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.excessDelayMinutes.Name = "excessDelayMinutes";
			this.excessDelayMinutes.Size = new System.Drawing.Size(56, 23);
			this.excessDelayMinutes.TabIndex = 4;
			this.excessDelayMinutes.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// excessDelayLabel
			// 
			this.excessDelayLabel.AutoSize = true;
			this.excessDelayLabel.Location = new System.Drawing.Point(28, 108);
			this.excessDelayLabel.Name = "excessDelayLabel";
			this.excessDelayLabel.Size = new System.Drawing.Size(121, 15);
			this.excessDelayLabel.TabIndex = 3;
			this.excessDelayLabel.Text = "Excess Delay Minutes:";
			// 
			// startScreenSaver
			// 
			this.startScreenSaver.AutoSize = true;
			this.startScreenSaver.Location = new System.Drawing.Point(12, 80);
			this.startScreenSaver.Name = "startScreenSaver";
			this.startScreenSaver.Size = new System.Drawing.Size(239, 19);
			this.startScreenSaver.TabIndex = 2;
			this.startScreenSaver.Text = "Start The Screen Saver After Excess Delay";
			this.startScreenSaver.UseVisualStyleBackColor = true;
			this.startScreenSaver.CheckedChanged += new System.EventHandler(this.StartScreenSaver_CheckedChanged);
			// 
			// playSound
			// 
			this.playSound.AutoSize = true;
			this.playSound.Location = new System.Drawing.Point(12, 52);
			this.playSound.Name = "playSound";
			this.playSound.Size = new System.Drawing.Size(287, 19);
			this.playSound.TabIndex = 1;
			this.playSound.Text = "Play Sound When Showing The Status Is Required";
			this.playSound.UseVisualStyleBackColor = true;
			// 
			// showProgress
			// 
			this.showProgress.AutoSize = true;
			this.showProgress.Location = new System.Drawing.Point(12, 24);
			this.showProgress.Name = "showProgress";
			this.showProgress.Size = new System.Drawing.Size(144, 19);
			this.showProgress.TabIndex = 0;
			this.showProgress.Text = "Show Progress Images";
			this.showProgress.UseVisualStyleBackColor = true;
			// 
			// formSaver
			// 
			this.formSaver.AutoLoad = false;
			this.formSaver.ContainerControl = this;
			this.formSaver.SettingsNodeName = "Options Dialog";
			this.formSaver.SaveSettings += new System.EventHandler<Menees.SettingsEventArgs>(this.FormSaver_SaveSettings);
			// 
			// OptionsDialog
			// 
			this.AcceptButton = okay;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = cancel;
			this.ClientSize = new System.Drawing.Size(331, 293);
			this.Controls.Add(groupBox2);
			this.Controls.Add(groupBox1);
			this.Controls.Add(cancel);
			this.Controls.Add(okay);
			this.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsDialog";
			this.Text = "Options";
			this.Load += new System.EventHandler(this.OptionsDialog_Load);
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.breakTimeSeconds)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.workTimeMinutes)).EndInit();
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.excessDelayMinutes)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
	}
}