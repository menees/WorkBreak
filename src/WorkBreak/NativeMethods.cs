namespace WorkBreak
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Windows.Forms;

	#endregion

	internal static class NativeMethods
	{
		#region Public Methods

		public static TimeSpan GetTimeSinceLastInput()
		{
			TimeSpan result = TimeSpan.Zero;

			LASTINPUTINFO info = new()
			{
				cbSize = LASTINPUTINFO.SizeOf,
			};

			if (GetLastInputInfo(ref info))
			{
				// Assign into longs so we can subtract below without fear of bounds checking exceptions.
				// Also, we can handle the 49.7 day rollover case easier.
				long lastInputTickCount = info.dwTime;
				long currentTickCount = unchecked((uint)Environment.TickCount);

				// If current is less than last input time, then we've run into the annoying 49.7 day
				// rollover problem with GetTickCount.  Since we're using long instead of uint, we can
				// adjust for this easily by adding back on the 49.7 days worth of milliseconds.
				// Note: This situation will correct itself the next time there is any input because then
				// the input time should be back to <= current time (assuming there's some input
				// every 49.7 days).
				if (currentTickCount < lastInputTickCount)
				{
					currentTickCount += uint.MaxValue;
				}

				long millisecondsSinceLastInput = currentTickCount - lastInputTickCount;
				result = TimeSpan.FromMilliseconds(millisecondsSinceLastInput);
			}

			return result;
		}

		public static void StartScreenSaver()
		{
			// http://stackoverflow.com/questions/5727977/how-to-start-screensaver-from-system-service
			// http://www.codeproject.com/Articles/12002/ScreenSaverNow-Starts-the-Screensaver
			const int WM_SYSCOMMAND = 0x112;
			const int SC_SCREENSAVE_VALUE = 0xF140;
			IntPtr SC_SCREENSAVE = new(SC_SCREENSAVE_VALUE);
			IntPtr hWnd = GetDesktopWindow();
			SendMessage(hWnd, WM_SYSCOMMAND, SC_SCREENSAVE, IntPtr.Zero);
		}

		public static bool FlashWindow(IWin32Window window, int flashCount)
		{
			bool result = false;

			if (window != null)
			{
				const uint FLASHW_CAPTION = 1;
				const uint FLASHW_TRAY = 2;
				const uint FLASHW_ALL = FLASHW_CAPTION | FLASHW_TRAY;

				FLASHWINFO info = new()
				{
					cbSize = FLASHWINFO.SizeOf,
					hwnd = window.Handle,
					dwFlags = FLASHW_ALL,
					uCount = flashCount,
					dwTimeout = 0,
				};

				result = FlashWindowEx(ref info);
			}

			return result;
		}

		#endregion

		#region Private User32.dll Imports

		[DllImport("user32.dll", SetLastError = false)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

		[DllImport("user32.dll", SetLastError = false)]
		private static extern IntPtr GetDesktopWindow();

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = false)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

		#endregion

		#region Private Types

		[StructLayout(LayoutKind.Sequential)]
		private struct LASTINPUTINFO
		{
			public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

			[MarshalAs(UnmanagedType.U4)]
			public int cbSize;

			[MarshalAs(UnmanagedType.U4)]
			public uint dwTime;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct FLASHWINFO
		{
			public static readonly int SizeOf = Marshal.SizeOf(typeof(FLASHWINFO));

			[MarshalAs(UnmanagedType.U4)]
			public int cbSize;

			public IntPtr hwnd;

			public uint dwFlags;

			[MarshalAs(UnmanagedType.U4)]
			public int uCount;

			public uint dwTimeout;
		}
		#endregion
	}
}
