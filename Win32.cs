/*

  Copyright (c) 2002 Matt Griffith

  Permission is hereby granted, free of charge, to any person obtaining 
  a copy of this software and associated documentation files (the "Software"), 
  to deal in the Software without restriction, including without limitation 
  the rights to use, copy, modify, merge, publish, distribute, sublicense, 
  and/or sell copies of the Software, and to permit persons to whom the 
  Software is furnished to do so, subject to the following conditions:

  The above copyright notice and this permission notice shall be included in 
  all copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
  
*/

using System;
using System.Runtime.InteropServices;

namespace  MattGriffith.Interop
{
	/// <summary>
	/// Specifies the icon to display on a balloon ToolTip.
	/// </summary>
	public enum BalloonIconStyle
	{
		/// <summary>
		/// Display no icon.
		/// </summary>
		None = 0x0,		// NIIF_NONE

		/// <summary>
		/// Display an error icon.
		/// </summary>
		Error = 0x3,	// NIIF_ERROR

		/// <summary>
		/// Display an information icon.
		/// </summary>
		Info = 0x1,		// NIIF_INFO

		/// <summary>
		/// Display a warning icon.
		/// </summary>
		Warning = 0x2	// NIIF_WARNING
	}

	/// <summary>
	/// Contains information that the system needs to process taskbar status area messages. Used in calls
	/// to Shell_NotifyIcon().
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
	public struct NOTIFYICONDATA
	{
		/// <summary>
		/// Size of this structure, in bytes. 
		/// </summary>
		public int cbSize;

		/// <summary>
		/// Handle to the window that receives notification messages associated with an icon in the 
		/// taskbar status area. The Shell uses hWnd and uID to identify which icon to operate on 
		/// when Shell_NotifyIcon is invoked. 
		/// </summary>
		public IntPtr hwnd;

		/// <summary>
		/// Application-defined identifier of the taskbar icon. The Shell uses hWnd and uID to identify 
		/// which icon to operate on when Shell_NotifyIcon is invoked. You can have multiple icons 
		/// associated with a single hWnd by assigning each a different uID. 
		/// </summary>
		public int uID;

		/// <summary>
		/// Flags that indicate which of the other members contain valid data. This member can be 
		/// a combination of the NIF_XXX constants.
		/// </summary>
		public int uFlags;

		/// <summary>
		/// Application-defined message identifier. The system uses this identifier to send 
		/// notifications to the window identified in hWnd. 
		/// </summary>
		public int uCallbackMessage;

		/// <summary>
		/// Handle to the icon to be added, modified, or deleted. 
		/// </summary>
		public IntPtr hIcon;

		/// <summary>
		/// String with the text for a standard ToolTip. It can have a maximum of 64 characters including 
		/// the terminating NULL. For Version 5.0 and later, szTip can have a maximum of 
		/// 128 characters, including the terminating NULL.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)]
		public string szTip;

		/// <summary>
		/// State of the icon. 
		/// </summary>
		public int dwState;

		/// <summary>
		/// A value that specifies which bits of the state member are retrieved or modified. 
		/// For example, setting this member to NIS_HIDDEN causes only the item's hidden state to be retrieved. 
		/// </summary>
		public int dwStateMask;

		/// <summary>
		/// String with the text for a balloon ToolTip. It can have a maximum of 255 characters. 
		/// To remove the ToolTip, set the NIF_INFO flag in uFlags and set szInfo to an empty string. 
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=256)]
		public string szInfo;

		/// <summary>
		/// NOTE: This field is also used for the Timeout value. Specifies whether the Shell notify 
		/// icon interface should use Windows 95 or Windows 2000 
		/// behavior. For more information on the differences in these two behaviors, see 
		/// Shell_NotifyIcon. This member is only employed when using Shell_NotifyIcon to send an 
		/// NIM_VERSION message. 
		/// </summary>
		public int uVersion;

		/// <summary>
		/// String containing a title for a balloon ToolTip. This title appears in boldface 
		/// above the text. It can have a maximum of 63 characters. 
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=64)]
		public string szInfoTitle;

		/// <summary>
		/// Adds an icon to a balloon ToolTip. It is placed to the left of the title. If the 
		/// szTitleInfo member is zero-length, the icon is not shown. See 
		/// <see cref="MattGriffith.Interop.BalloonIconStyle">BalloonIconStyle</see> for more
		/// information.
		/// </summary>
		public int dwInfoFlags;
	}


	/// <summary>
	/// The ANIMATIONINFO structure specifies the animation effects associated with user actions. 
	/// This structure is used with the SystemParametersInfo function when the SPI_GETANIMATION or 
	/// SPI_SETANIMATION action value is specified.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
	public struct ANIMATIONINFO
	{
		/// <summary>
		/// Specifies the size of the structure, in bytes.
		/// </summary>
		public int cbSize; 

		/// <summary>
		/// Indicates that minimize and restore animation is enabled (if the member is 
		/// a nonzero value) or not enabled (if zero). 
		/// </summary>
		public int  iMinAnimate;
	}

	/// <summary>
	/// The RECT structure defines the coordinates of the upper-left and lower-right 
	/// corners of a rectangle.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
	public struct RECT 
	{ 
		/// <summary>
		/// Specifies the x-coordinate of the upper-left corner of the rectangle. 
		/// </summary>
		public int left; 

		/// <summary>
		/// Specifies the y-coordinate of the upper-left corner of the rectangle.
		/// </summary>
		public int top; 

		/// <summary>
		/// Specifies the x-coordinate of the lower-right corner of the rectangle. 
		/// </summary>
		public int right; 

		/// <summary>
		/// Specifies the y-coordinate of the lower-right corner of the rectangle. 
		/// </summary>
		public int bottom; 
	}

	/// <summary>
	/// The POINT structure defines the x- and y- coordinates of a point.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
	public struct POINT 
	{ 
		/// <summary>
		/// Specifies the x-coordinate of the point. 
		/// </summary>
		public int x; 

		/// <summary>
		/// Specifies the y-coordinate of the point. 
		/// </summary>
		public int y; 
	}

	/// <summary>
	/// The TPMPARAMS structure contains extended parameters for the TrackPopupMenuEx function.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
	public struct TPMPARAMS 
	{ 
		/// <summary>
		/// Size of structure, in bytes.
		/// </summary>
		public int cbSize; 

		/// <summary>
		/// Rectangle to exclude when positioning the window, in screen coordinates.
		/// </summary>
		public RECT rcExclude; 
	}

	/// <summary>
	/// The EnumChildProc delegate is an application-defined callback function used with the 
	/// EnumChildWindows function. It receives the child window handles.
	/// </summary>
	public delegate bool EnumChildProc(IntPtr hwnd, ref RECT lParam);

	/// <summary>
	/// Contains various usefull Win32 API Functions declarations and constants.
	/// </summary>
	public class Win32
	{
		#region Constants
		public const int NIF_MESSAGE = 0x1;
		public const int NIF_ICON = 0x2;
		public const int NIF_STATE = 0x8;
		public const int NIF_INFO = 0x10;
		public const int NIF_TIP = 0x4;
		public const int NIM_ADD = 0x0;
		public const int NIM_MODIFY = 0x1;
		public const int NIM_DELETE = 0x2;
		public const int NIM_SETVERSION = 0x4;
		public const int NOTIFYICON_VERSION = 5;

		public const int WM_USER = 0x400;
		public const int WM_USER_TRAY = WM_USER + 1;
		public const int WM_MOUSEMOVE = 0x200;
		public const int WM_LBUTTONDOWN = 0x201;
		public const int WM_LBUTTONUP = 0x202;
		public const int WM_LBUTTONDBLCLK = 0x203;
		public const int WM_RBUTTONDOWN = 0x204;
		public const int WM_RBUTTONUP = 0x205;
		public const int WM_RBUTTONDBLCLK = 0x206;
		public const int WM_MBUTTONDOWN = 0x207;
		public const int WM_MBUTTONUP = 0x208;
		public const int WM_MBUTTONDBLCLK = 0x209;

		public const int WM_SYSCOMMAND = 0x112;
		public const int SC_MINIMIZE = 0xF020;
		public const int SC_MAXIMIZE = 0xF030;

		public const int NIN_BALLOONSHOW = 0x402;
		public const int NIN_BALLOONHIDE = 0x403;
		public const int NIN_BALLOONTIMEOUT = 0x404;
		public const int NIN_BALLOONUSERCLICK = 0x405;

		public const int SPI_GETANIMATION = 0x48;

		public const int IDANI_OPEN = 0x1;
		public const int IDANI_CLOSE = 0x2;
		public const int IDANI_CAPTION = 0x3;

		public const int GWL_EXSTYLE = -20;
		public const int WS_EX_TOOLWINDOW = 0x00000080;
		public const int WS_EX_APPWINDOW = 0x00040000;

		#endregion

		#region Public Function Declarations
		
		[DllImport("Shell32", CharSet=CharSet.Auto)]
		public static extern short Shell_NotifyIcon(
			int dwMessage, ref NOTIFYICONDATA lpData);

		[DllImport("User32", CharSet=CharSet.Auto)]
		public static extern int RegisterWindowMessage(
			[MarshalAs(UnmanagedType.LPTStr)]string lpString);

		[DllImport("User32", CharSet=CharSet.Auto)]
		public static extern int SystemParametersInfo(
			int uAction,
			int uParam,
			ref ANIMATIONINFO lpvParam,
			int fuWinIni);

		[DllImport("User32", CharSet=CharSet.Auto)]
		public static extern int DrawAnimatedRects(
			IntPtr hwnd,            // handle to clipping window
			int idAni,				// type of animation
			ref RECT lprcFrom,		// rectangle coordinates (minimized)
			ref RECT lprcTo			// rectangle coordinates (restored)
				);

		[DllImport("User32", CharSet=CharSet.Auto)]
		public static extern int GetWindowRect(
			IntPtr hWnd,      // handle to window
			ref RECT lpRect   // window coordinates
			);

		[DllImport("User32", CharSet=CharSet.Auto)]
		 public static extern IntPtr FindWindow(
			[MarshalAs(UnmanagedType.LPTStr)] string lpClassName,  // class name
			[MarshalAs(UnmanagedType.LPTStr)] string lpWindowName  // window name
			);

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern int GetClassName(
			IntPtr hwnd, 
			[MarshalAs(UnmanagedType.LPTStr)] string lpClassName, 
			//char[] lpClassName,
			int capacity);

		[DllImport("user32.dll")]
		public static extern int GetCursorPos(
			ref POINT lpPoint   // cursor position
			);
		
		[DllImport("user32.dll")]
		public static extern int SetForegroundWindow(
			IntPtr hWnd   // handle to window
			);

		[DllImport("user32.dll")]
		public static extern int TrackPopupMenuEx(
			IntPtr hmenu,       // handle to shortcut menu
			int fuFlags,      // options
			int x,             // horizontal position
			int y,             // vertical position
			IntPtr hwnd,         // handle to window
			ref TPMPARAMS lptpm  // area not to overlap
			);

		[DllImport("user32.dll")]
		public static extern int PostMessage(
			IntPtr hWnd,      // handle to destination window
			int Msg,       // message
			int wParam,  // first message parameter
			int lParam   // second message parameter
			);

		[DllImport("user32.dll")]
		public static extern int EnumChildWindows(
			IntPtr hWndParent,         // handle to parent window
			EnumChildProc lpEnumFunc,  // callback function
			ref RECT lParam            // application-defined value
			);

		[DllImport("user32.dll")]
		public static extern int SetWindowLong(
			IntPtr hWnd,       // handle to window
			int nIndex,      // offset of value to set
			int dwNewLong   // new value
			);

		[DllImport("user32.dll")]
		public static extern int GetWindowLong(
			IntPtr hWnd,  // handle to window
			int nIndex  // offset of value to retrieve
			);

		[DllImport("KERNEL32")]
		public static extern short QueryPerformanceCounter(ref long performanceCounter);  

		[DllImport("KERNEL32")]
		public static extern short QueryPerformanceFrequency(ref long currentFrequency);

		#endregion // Public Function Declarations
		
	}
}
