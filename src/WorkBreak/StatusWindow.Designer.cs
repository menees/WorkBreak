namespace WorkBreak
{
	internal partial class StatusWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.PictureBox progressImage;
		private System.Windows.Forms.Label statusMessage;
		private Menees.Windows.Forms.ExtendedToolStrip buttonBar;
		private System.Windows.Forms.ToolStripButton dismissButton;
		private System.Windows.Forms.ContextMenuStrip snoozeTimes;
		private System.Windows.Forms.ToolStripSplitButton snoozeButton;
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
			this.formSaver = new Menees.Windows.Forms.FormSaver(this.components);
			this.progressImage = new System.Windows.Forms.PictureBox();
			this.statusMessage = new System.Windows.Forms.Label();
			this.buttonBar = new Menees.Windows.Forms.ExtendedToolStrip();
			this.dismissButton = new System.Windows.Forms.ToolStripButton();
			this.snoozeButton = new System.Windows.Forms.ToolStripSplitButton();
			this.snoozeTimes = new System.Windows.Forms.ContextMenuStrip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.progressImage)).BeginInit();
			this.buttonBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// formSaver
			// 
			this.formSaver.AutoLoad = false;
			this.formSaver.ContainerControl = this;
			this.formSaver.SettingsNodeName = "Status Window";
			this.formSaver.SaveSettings += new System.EventHandler<Menees.SettingsEventArgs>(this.FormSaver_SaveSettings);
			// 
			// progressImage
			// 
			this.progressImage.Dock = System.Windows.Forms.DockStyle.Left;
			this.progressImage.Image = global::WorkBreak.Properties.Resources.Png48_Stopwatch_00;
			this.progressImage.Location = new System.Drawing.Point(0, 0);
			this.progressImage.Name = "progressImage";
			this.progressImage.Size = new System.Drawing.Size(48, 63);
			this.progressImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.progressImage.TabIndex = 4;
			this.progressImage.TabStop = false;
			// 
			// statusMessage
			// 
			this.statusMessage.AutoEllipsis = true;
			this.statusMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.statusMessage.Location = new System.Drawing.Point(48, 0);
			this.statusMessage.Name = "statusMessage";
			this.statusMessage.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.statusMessage.Size = new System.Drawing.Size(233, 63);
			this.statusMessage.TabIndex = 0;
			this.statusMessage.Text = "Ready";
			this.statusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// buttonBar
			// 
			this.buttonBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.buttonBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dismissButton,
            this.snoozeButton});
			this.buttonBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.buttonBar.Location = new System.Drawing.Point(0, 63);
			this.buttonBar.Name = "buttonBar";
			this.buttonBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.buttonBar.ShowItemToolTips = false;
			this.buttonBar.Size = new System.Drawing.Size(281, 25);
			this.buttonBar.TabIndex = 1;
			// 
			// dismissButton
			// 
			this.dismissButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.dismissButton.Image = global::WorkBreak.Properties.Resources.Dismiss;
			this.dismissButton.Name = "dismissButton";
			this.dismissButton.Size = new System.Drawing.Size(67, 22);
			this.dismissButton.Text = "&Dismiss";
			this.dismissButton.Click += new System.EventHandler(this.DismissButton_Click);
			// 
			// snoozeButton
			// 
			this.snoozeButton.DropDown = this.snoozeTimes;
			this.snoozeButton.Image = global::WorkBreak.Properties.Resources.Snooze;
			this.snoozeButton.Name = "snoozeButton";
			this.snoozeButton.Size = new System.Drawing.Size(77, 22);
			this.snoozeButton.Text = "&Snooze";
			this.snoozeButton.ButtonClick += new System.EventHandler(this.SnoozeButton_ButtonClick);
			// 
			// snoozeTimes
			// 
			this.snoozeTimes.Name = "snoozeTimes";
			this.snoozeTimes.OwnerItem = this.snoozeButton;
			this.snoozeTimes.Size = new System.Drawing.Size(61, 4);
			// 
			// StatusWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(281, 88);
			this.Controls.Add(this.statusMessage);
			this.Controls.Add(this.progressImage);
			this.Controls.Add(this.buttonBar);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StatusWindow";
			this.Text = "Work Break";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StatusWindow_FormClosing);
			this.Load += new System.EventHandler(this.StatusWindow_Load);
			this.VisibleChanged += new System.EventHandler(this.StatusWindow_VisibleChanged);
			((System.ComponentModel.ISupportInitialize)(this.progressImage)).EndInit();
			this.buttonBar.ResumeLayout(false);
			this.buttonBar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
	}
}