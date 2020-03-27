namespace WorkBreak
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	internal static class Options
	{
		#region Public Properties

		public static TimeSpan MaxWorkTime { get; set; }

		public static TimeSpan MinBreakTime { get; set; }

		public static bool ShowProgress { get; set; }

		public static bool PlaySound { get; set; }

		public static bool StartScreenSaver { get; set; }

		public static TimeSpan ScreenSaverDelay { get; set; }

		public static TimeSpan SnoozeTime { get; set; }

		#endregion

		#region Public Methods

		public static void Load(ISettingsNode node)
		{
#pragma warning disable MEN010 // Avoid magic numbers. Default minutes and seconds values are clear in context.
			MaxWorkTime = TimeSpan.FromMinutes(node.GetValue("MaxWorkMinutes", 30));
			MinBreakTime = TimeSpan.FromSeconds(node.GetValue("MinBreakSeconds", 30));
			ShowProgress = node.GetValue(nameof(ShowProgress), true);
			PlaySound = node.GetValue(nameof(PlaySound), false);
			StartScreenSaver = node.GetValue(nameof(StartScreenSaver), false);
			ScreenSaverDelay = TimeSpan.FromMinutes(node.GetValue("ScreenSaverDelayMinutes", 3));
			SnoozeTime = TimeSpan.FromMinutes(node.GetValue("SnoozeMinutes", 3));
#pragma warning restore MEN010 // Avoid magic numbers
		}

		public static void Save(ISettingsNode node)
		{
			node.SetValue("MaxWorkMinutes", (int)MaxWorkTime.TotalMinutes);
			node.SetValue("MinBreakSeconds", (int)MinBreakTime.TotalSeconds);
			node.SetValue(nameof(ShowProgress), ShowProgress);
			node.SetValue(nameof(PlaySound), PlaySound);
			node.SetValue(nameof(StartScreenSaver), StartScreenSaver);
			node.SetValue("ScreenSaverDelayMinutes", (int)ScreenSaverDelay.TotalMinutes);
			node.SetValue("SnoozeMinutes", (int)SnoozeTime.TotalMinutes);
		}

		#endregion
	}
}
