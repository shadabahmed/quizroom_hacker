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
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using MattGriffith.Interop;

namespace MattGriffith.Windows.Forms
{
	
	/// <summary>
	/// A component that creates an icon in the status area, displays balloon ToolTips. 
	/// Also provides a means for applications to minimize themselves to the System Tray.
	/// The MinimizeToTray feature uses window animation if the user has window animation 
	/// turned on. Otherwise it minimizes and restores the window normally.
	/// </summary>
	/// <remarks>
	/// <p>To create a icon in the System Tray set the <c>Icon</c> and <c>Text</c> properties. Then set the <c>Visible</c> property to <c>true</c>.</p>
	///
	///	<p>To display a balloon ToolTip set the <c>Icon</c> property then call the <c>ShowBalloon()</c> method. </p>
	///
	///	<p>To minimize your window to the System Tray set the <c>Icon</c> and <c>Text</c> properties. Then call the <c>MinimizeToTray()</c> method. To restore the window call the <c>RestoreFromTray()</c> method. </p>
	///
	///	<p>Typically you will want to 
	///	catch the <c>WM_SYSCOMMAND</c> and <c>SC_MINIMIZE</c> messages in your <c>WndProc()</c>. Then you would call <c>MinimizeToTray()</c>.</p>
	///
	///	<code>
	///	protected override void WndProc(ref Message m)
	/// {
	///		if(m.Msg == Win32.WM_SYSCOMMAND)
	///			switch(m.WParam.ToInt32())		 
	///		{
	///			case Win32.SC_MINIMIZE :
	///				this.notifyIcon1.Text = &quot; NotifyIcon Test Application &quot;;
	///				this.notifyIcon1.MinimizeToTray(this.Handle);
	///				this.notifyIcon1.Visible = true;
	///				return;
	///			default:
	///				break;
	///		} 
	///		base.WndProc(ref m);
	/// } 
	///	</code>
	///
	///	<p>To restore the window you can provide an event handler for the NotifyIcon.DoubleClick event that calls RestoreFromTray().</p>
	///
	/// </remarks>
	[ToolboxBitmap(typeof(NotifyIcon), "MattGriffith.Windows.Forms.NotifyIcon.bmp"), 
	System.ComponentModel.DefaultPropertyAttribute("Text"), 
	System.ComponentModel.DefaultEventAttribute("MouseDown"), 
	System.ComponentModel.ToolboxItemFilterAttribute("MattGriffith.Windows.Forms")]
	public sealed class NotifyIcon : Component
	{
		private const int MaxBaloonTitleLength = 63;
		private const int MaxBaloonTextLength = 255;
		private const int MaxNotifyIconTipLength = 127;

		#region Message handler

		/// <summary>
		/// Private window class that handles the window messages for NotifyIcon.
		/// </summary>
		private class MessageHandler : Form
		{

			private EventHandler clickEventHandlerDelegate;
			public new event EventHandler Click
			{
				add
				{
					clickEventHandlerDelegate = (EventHandler)
						Delegate.Combine(clickEventHandlerDelegate, value);
				}
				remove
				{
					clickEventHandlerDelegate = (EventHandler)
						Delegate.Remove(clickEventHandlerDelegate, value);
				}
			}

			private EventHandler doubleClickEventHandlerDelegate;
			public new event EventHandler DoubleClick
			{
				add
				{
					doubleClickEventHandlerDelegate = (EventHandler)
						Delegate.Combine(doubleClickEventHandlerDelegate, value);
				}
				remove
				{
					doubleClickEventHandlerDelegate = (EventHandler)
						Delegate.Remove(doubleClickEventHandlerDelegate, value);
				}
			}

			private MouseEventHandler mouseDownEventHandlerDelegate;
			public new event MouseEventHandler MouseDown
			{
				add
				{
					mouseDownEventHandlerDelegate = (MouseEventHandler)
						Delegate.Combine(mouseDownEventHandlerDelegate, value);
				}
				remove
				{
					mouseDownEventHandlerDelegate = (MouseEventHandler)
						Delegate.Remove(mouseDownEventHandlerDelegate, value);
				}
			}

			private MouseEventHandler mouseMoveEventHandlerDelegate;
			public new event MouseEventHandler MouseMove
			{
				add
				{
					mouseMoveEventHandlerDelegate = (MouseEventHandler)
						Delegate.Combine(mouseMoveEventHandlerDelegate, value);
				}
				remove
				{
					mouseMoveEventHandlerDelegate = (MouseEventHandler)
						Delegate.Remove(mouseMoveEventHandlerDelegate, value);
				}
			}

			private MouseEventHandler mouseUpEventHandlerDelegate;
			public new event MouseEventHandler MouseUp
			{
				add
				{
					mouseUpEventHandlerDelegate = (MouseEventHandler)
						Delegate.Combine(mouseUpEventHandlerDelegate, value);
				}
				remove
				{
					mouseUpEventHandlerDelegate = (MouseEventHandler)
						Delegate.Remove(mouseUpEventHandlerDelegate, value);
				}
			}

			public event EventHandler Reload;
			public event EventHandler BalloonShow;
			public event EventHandler BalloonHide;
			public event EventHandler BalloonTimeout;
			public event EventHandler BalloonClick;

			protected new virtual void OnClick(EventArgs e)
			{
				if(this.clickEventHandlerDelegate != null)
					this.clickEventHandlerDelegate(this, e);
			}

			protected new virtual void OnDoubleClick(EventArgs e)
			{
				if(doubleClickEventHandlerDelegate != null)
					doubleClickEventHandlerDelegate(this, e);
			}
			
			protected new virtual void OnMouseDown(MouseEventArgs e)
			{
				if(mouseDownEventHandlerDelegate != null)
					mouseDownEventHandlerDelegate(this, e);
			}

			protected new virtual void OnMouseMove(MouseEventArgs e)
			{
				if(mouseMoveEventHandlerDelegate != null)
					mouseMoveEventHandlerDelegate(this, e);
			}

			protected new virtual void OnMouseUp(MouseEventArgs e)
			{
				if(mouseUpEventHandlerDelegate != null)
					mouseUpEventHandlerDelegate(this, e);
			}

			protected virtual void OnReload(EventArgs e)
			{
				if(null != Reload)
					Reload(this, e);
			}
			
			protected virtual void OnBalloonShow(EventArgs e)
			{
				if(null != BalloonShow)
					BalloonShow(this, e);
			}
            
			protected virtual void OnBalloonHide(EventArgs e)
			{
				if(null != BalloonHide)
					BalloonHide(this, e);
			}

			protected virtual void OnBalloonTimeout(EventArgs e)
			{
				if(null != BalloonTimeout)
					BalloonTimeout(this, e);
			}

			protected virtual void OnBalloonClick(EventArgs e)
			{
				if(null != BalloonClick)
					BalloonClick(this, e);
			}


			private int WM_TASKBARCREATED = Win32.RegisterWindowMessage("TaskbarCreated");

			public MessageHandler()
			{
				ShowInTaskbar = false;
				MakeToolWindow(this.Handle);
				StartPosition = FormStartPosition.Manual;
				Size = new Size(100, 100);
				Location = new Point(-500, -500);
				Show();
			}

			/// <summary>
			/// Turn this window into a tool window, so it doesn't show up in the Alt-tab list...
			/// </summary>
			public static void MakeToolWindow(IntPtr hWnd)
			{
				int windowStyle = Win32.GetWindowLong(hWnd, Win32.GWL_EXSTYLE);

				Win32.SetWindowLong(hWnd, Win32.GWL_EXSTYLE, windowStyle | Win32.WS_EX_TOOLWINDOW);
			}

			protected override void WndProc(ref Message m)
			{
				switch(m.Msg)
				{
					case Win32.WM_USER_TRAY:
					switch(m.LParam.ToInt32())
					{
						case Win32.WM_LBUTTONDBLCLK:
						case Win32.WM_RBUTTONDBLCLK:
						case Win32.WM_MBUTTONDBLCLK:
							OnDoubleClick(new MouseEventArgs(MouseButtons, 0, MousePosition.X, MousePosition.Y, 0));
							break;
						case Win32.WM_LBUTTONDOWN: 
						case Win32.WM_RBUTTONDOWN:
						case Win32.WM_MBUTTONDOWN:
							OnMouseDown(new MouseEventArgs(MouseButtons, 0, MousePosition.X, MousePosition.Y, 0));
							break;
						case Win32.WM_MOUSEMOVE:
							OnMouseMove(new MouseEventArgs(MouseButtons, 0, MousePosition.X, MousePosition.Y, 0));
							break;
						case Win32.WM_LBUTTONUP:
							OnMouseUp(new MouseEventArgs(MouseButtons.Left, 0, MousePosition.X, MousePosition.Y, 0));
							OnClick(new MouseEventArgs(MouseButtons.Left, 0, MousePosition.X, MousePosition.Y, 0));
							break;
						case Win32.WM_RBUTTONUP:
							OnMouseUp(new MouseEventArgs(MouseButtons.Right, 0, MousePosition.X, MousePosition.Y, 0));
							OnClick(new MouseEventArgs(MouseButtons.Right, 0, MousePosition.X, MousePosition.Y, 0));
							break;
						case Win32.WM_MBUTTONUP:
							OnMouseUp(new MouseEventArgs(MouseButtons.Middle, 0, MousePosition.X, MousePosition.Y, 0));
							OnClick(new MouseEventArgs(MouseButtons.Middle, 0, MousePosition.X, MousePosition.Y, 0));
							break;
						case Win32.NIN_BALLOONSHOW:
							OnBalloonShow(EventArgs.Empty);
							break;
						case Win32.NIN_BALLOONHIDE:
							OnBalloonHide(EventArgs.Empty);
							break;
						case Win32.NIN_BALLOONTIMEOUT:
							OnBalloonTimeout(EventArgs.Empty);
							break;
						case Win32.NIN_BALLOONUSERCLICK:
							OnBalloonClick(EventArgs.Empty);
							break;
						default:
							Debug.WriteLine(m.LParam.ToInt32());
							break;
					} // end switch(m.LParam.ToInt32())
						break;
					default:
						if(m.Msg == WM_TASKBARCREATED)
							OnReload(EventArgs.Empty);
						break;
				}
				base.WndProc(ref m);
			}
		} // end MessageHandler
		#endregion // end Message handler
	
		#region Component Designer generated code

		public NotifyIcon(System.ComponentModel.IContainer container)
		{

			// Required for Windows.Forms Class Composition Designer support
			container.Add(this);
			/*
						NID.hwnd = Messages.Handle;
						NID.cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA));
						NID.uFlags = NIF_ICON | NIF_TIP | NIF_MESSAGE;
						NID.uCallbackMessage = WM_USER_TRAY;
						NID.uVersion = NOTIFYICON_VERSION;
						NID.szTip = "";
						NID.uID = 0;
			*/
			this._Messages.Click			+= new EventHandler(Messages_Click);
			this._Messages.DoubleClick	+= new EventHandler(Messages_DoubleClick);
			this._Messages.MouseDown		+= new MouseEventHandler(Messages_MouseDown);
			this._Messages.MouseMove		+= new MouseEventHandler(Messages_MouseMove);
			this._Messages.MouseUp		+= new MouseEventHandler(Messages_MouseUp);
			this._Messages.Reload		+= new EventHandler(Messages_Reload);
			this._Messages.BalloonShow	+= new EventHandler(Messages_BalloonShow);
			this._Messages.BalloonHide	+= new EventHandler(Messages_BalloonHide);
			this._Messages.BalloonTimeout += new EventHandler(Messages_BalloonTimeout);
			this._Messages.BalloonClick	+= new EventHandler(Messages_BalloonClick);
			
		}

		public NotifyIcon()
		{
			//This call is required by the Component Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

			

		}

		//Component overrides dispose to clean up the component list.
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				//Visible = false;
				this.UpdateIcon(false);

				if(null != this._Messages)
					this._Messages.Dispose();

				if(components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Required by the Component Designer
		private System.ComponentModel.IContainer components;

		/*
			NOTE: The following procedure is required by the Component Designer
			It can be modified using the Component Designer.
			Do not modify it using the code editor.
		*/
		[System.Diagnostics.DebuggerStepThrough()] 
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}

		#endregion

		#region Private Member Fields
		//private NOTIFYICONDATA NID;
		private MessageHandler _Messages = new MessageHandler(); 

		/// <summary>
		/// Stores the Visible state before showing the balloon. The Visible state will get restored to
		/// this value when the balloon is hidden.
		/// </summary>
		private bool _VisibleBeforeBalloon;

		/// <summary>
		/// Indicates whether the Icon has been added to the tray or not.
		/// </summary>
		private bool _Added = false;

		private bool _Visible;
		private System.Drawing.Icon _Icon;
		private ContextMenu _ContextMenu;
		private string _Text;
		#endregion
		
		#region Public Events
		/// <summary>
		/// Occurs when the user clicks the icon in the status area.
		/// </summary>
		[DescriptionAttribute("Occurs when the user clicks the icon in the status area."),
		System.ComponentModel.CategoryAttribute("Action")]
		public event EventHandler Click;

		/// <summary>
		/// Occurs when the user double-clicks the icon in the status notification area of the taskbar.
		/// </summary>
		[DescriptionAttribute("Occurs when the user double-clicks the icon in the status notification area of the taskbar."),
		System.ComponentModel.CategoryAttribute("Action")]
		public event EventHandler DoubleClick;		

		/// <summary>
		/// Occurs when the user presses the mouse button while the pointer is over the icon in the status notification area of the taskbar.
		/// </summary>
		[DescriptionAttribute("Occurs when the user presses the mouse button while the pointer is over the icon in the status notification area of the taskbar."), 
		CategoryAttribute("Mouse")]
		public event MouseEventHandler MouseDown;

		/// <summary>
		/// Occurs when the user moves the mouse while the pointer is over the icon in the status notification area of the taskbar.
		/// </summary>
		[DescriptionAttribute("Occurs when the user moves the mouse while the pointer is over the icon in the status notification area of the taskbar."), 
		CategoryAttribute("Mouse")]
		public event MouseEventHandler MouseMove;

		/// <summary>
		/// Occurs when the user releases the mouse button while the pointer is over the icon in the status notification area of the taskbar.
		/// </summary>
		[DescriptionAttribute("Occurs when the user releases the mouse button while the pointer is over the icon in the status notification area of the taskbar."),
		CategoryAttribute("Mouse")]
		public event MouseEventHandler MouseUp;	

		/// <summary>
		/// Occurs when the balloon is shown (balloons are queued).
		/// </summary>
		[DescriptionAttribute("Occurs when the balloon is shown (balloons are queued)."), 
		System.ComponentModel.CategoryAttribute("Behavior")]
		public event EventHandler BalloonShow;

		/// <summary>
		/// Occurs when the balloon disappears—for example, when the icon is deleted. This message is not sent if the balloon is dismissed because of a timeout or a mouse click.
		/// </summary>
		[DescriptionAttribute("Occurs when the balloon disappears—for example, when the icon is deleted. This message is not sent if the balloon is dismissed because of a timeout or a mouse click."),
		System.ComponentModel.CategoryAttribute("Behavior")]
		public event EventHandler BalloonHide;

		/// <summary>
		/// Occurs when the balloon is dismissed because of a timeout.
		/// </summary>
		[DescriptionAttribute("Occurs when the balloon is dismissed because of a timeout."),
		System.ComponentModel.CategoryAttribute("Behavior")]
		public event EventHandler BalloonTimeout;	

		/// <summary>
		/// Occurs when the balloon is dismissed because of a mouse click.
		/// </summary>
		[DescriptionAttribute("Occurs when the balloon is dismissed because of a mouse click."),
		System.ComponentModel.CategoryAttribute("Action")]
		public event EventHandler BalloonClick;		
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets or sets the current icon.
		/// </summary>
		[Description("Gets or sets the current icon."), 
		CategoryAttribute("Appearance"), 
		System.ComponentModel.DefaultValueAttribute("")]
		public System.Drawing.Icon Icon
		{
			get
			{
				return _Icon;
			}
			set
			{
				if (this._Icon != value) 
				{
					this._Icon = value;
					this.UpdateIcon(this.Visible);
				}
			}
		}

		/// <summary>
		/// Gets or sets the ToolTip text displayed when the mouse hovers over a status area icon.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">The value is greater longer than 127
		/// characters.</exception>
		[Description("Gets or sets the ToolTip text displayed when the mouse hovers over a status area icon."),
		CategoryAttribute("Appearance")]
		public string Text
		{
			get
			{
				return this._Text;
			}
			set
			{
				if (value == null)
					value = string.Empty;

				if (value != null && value != this._Text) 
				{
					if (value != null && value.Length > MaxNotifyIconTipLength)
						throw new ArgumentOutOfRangeException("Text can not be longer than 127 characters.");

					this._Text = value;

					if (this._Added)
						UpdateIcon(true);
				}
			}
		}

		/// <summary>
		/// Gets or sets the context menu for the icon.
		/// </summary>
		[Description("Gets or sets the context menu for the icon."),
		CategoryAttribute("Behavior"), 
		System.ComponentModel.DefaultValueAttribute("")]
		public System.Windows.Forms.ContextMenu ContextMenu
		{
			get
			{
				return _ContextMenu;
			}
			set
			{
				_ContextMenu = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the icon is visible in the status notification 
		/// area of the taskbar.
		/// </summary>
		[Description("Gets or sets a value indicating whether the icon is visible in the status notification area of the taskbar."), 
		CategoryAttribute("Behavior"), 
		System.ComponentModel.DefaultValueAttribute(false)]
		public bool Visible
		{
			get { return _Visible; }
			set
			{
				if(this._Visible != value)
				{
					UpdateIcon(value);
					_Visible = value;
				}
			}
		}
		#endregion
		
		#region Public Methods
		/// <summary>
		/// Displays a Balloon ToolTip for the current NotifyIcon. Uses the default timeout of 30 seconds.
		/// </summary>
		/// <param name="iconStyle">The balloon ToolTip's icon to use. This icon is placed to the 
		/// left of the balloon ToolTip's title.</param>
		/// <param name="text">The text to display for the balloon ToolTip. The length of the text can not
		/// exceed 254 characters.</param>
		/// <param name="title">The text to display as a title for the balloon ToolTip. 
		/// This title appears in boldface above the text. It can have a maximum of 63 characters.
		/// Pass null or String.Empty to prevent the balloon from displaying a title.</param>
		/// <exception cref="ArgumentNullException">Text is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Text is longer than 254 characters.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Title is longer than 254 characters.</exception>
		public void ShowBalloon(BalloonIconStyle iconStyle, string text, string title)
		{
			ShowBalloon(iconStyle, text, title, 30000);
		}
		
		/// <summary>
		/// Displays a Balloon ToolTip for the current NotifyIcon.
		/// </summary>
		/// <param name="iconStyle">The balloon ToolTip's icon to use. This icon is placed to the 
		/// left of the balloon ToolTip's title.</param>
		/// <param name="text">The text to display for the balloon ToolTip. The length of the text can not
		/// exceed 254 characters.</param>
		/// <param name="title">The text to display as a title for the balloon ToolTip. 
		/// This title appears in boldface above the text. It can have a maximum of 63 characters.
		/// Pass null or String.Empty to prevent the balloon from displaying a title.</param>
		/// <param name="timeout">The number of milliseconds the balloon ToolTip should remain visible
		/// before it is hidden. The system enforces minimum and maximum timeout values. Timeout values that 
		/// are too large are set to the maximum value and values that are too small default to the 
		/// minimum value. The system minimum and maximum timeout values are currently set at 10 seconds 
		/// and 30 seconds, respectively.</param>
		/// <exception cref="ArgumentNullException">Text is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Text is longer than 254 characters.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Title is longer than 254 characters.</exception>
		public void ShowBalloon(BalloonIconStyle iconStyle, string text, 
			string title, int timeout)
		{	

			if(null == text)
				throw new ArgumentNullException("Text is not optional.", "text");
			if(null != text && text.Length > MaxBaloonTextLength)
				throw new ArgumentOutOfRangeException("The text is too long. Please provide a string with " +
					"fewer than 255 characters.", "text");
			if(null == title)
				title = string.Empty;
			if(null != title && title.Length > MaxBaloonTitleLength)
				throw new ArgumentOutOfRangeException("The title is too long. Please provide a string with " +
					"fewer than 64 characters.", "title");
			

			_VisibleBeforeBalloon = Visible;
			
			NOTIFYICONDATA nid;

			nid = new NOTIFYICONDATA();
			nid.uCallbackMessage = Win32.WM_USER_TRAY;
			nid.cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA));
			nid.uFlags = Win32.NIF_MESSAGE;
			nid.hwnd = this._Messages.Handle;
			nid.uID = 0;

			nid.hIcon = IntPtr.Zero;
			
			nid.uFlags = nid.uFlags | Win32.NIF_ICON;
			nid.hIcon = this.Icon.Handle;
			

			nid.uFlags = nid.uFlags | Win32.NIF_INFO;

			// Timeout value is sent in version
			nid.uVersion = timeout;

			nid.szInfo = text;
			nid.szInfoTitle = title;
			nid.dwInfoFlags = Convert.ToInt32(iconStyle);

			// Make sure our Visible property indicates true.
			if(!this.Visible)
				this._Visible = true;

			if (!this._Added) 
			{
				Win32.Shell_NotifyIcon(Win32.NIM_ADD, ref nid);
				this._Added = true; 
			}
			else if(this._Added)
				Win32.Shell_NotifyIcon(Win32.NIM_MODIFY, ref nid);
		}

		public void HideBalloon()
		{
			NOTIFYICONDATA nid;

			nid = new NOTIFYICONDATA();
			nid.uCallbackMessage = Win32.WM_USER_TRAY;
			nid.cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA));
			nid.uFlags = Win32.NIF_MESSAGE;
			nid.hwnd = this._Messages.Handle;
			nid.uID = 0;
			
			nid.hIcon = IntPtr.Zero;
			nid.hIcon = this.Icon.Handle;
			

			
			nid.uFlags = Win32.NIF_INFO;

			nid.szInfo = "\0";	
			nid.szInfoTitle = "\0";
			Win32.Shell_NotifyIcon(Win32.NIM_MODIFY, ref nid);
		}


		/// <summary>
		/// Minimizes the given window to the System Tray.
		/// </summary>
		/// <param name="hWnd">The handle of the window to minimize.</param>
		public void MinimizeToTray(IntPtr hWnd)
		{
			if (UseWindowAnimation())
			{
				RECT rectFrom = new RECT();
				RECT rectTo = new RECT();

				Win32.GetWindowRect(hWnd, ref rectFrom);
				GetTrayWndRect(ref rectTo);

				Win32.DrawAnimatedRects(hWnd, Win32.IDANI_CAPTION, ref rectFrom, ref rectTo);
			}

			HideWindow(hWnd);
			//SetWindowLong(hWnd, GWL_STYLE, GetWindowLong(hWnd, GWL_STYLE) &~ WS_VISIBLE);
		}

		/// <summary>
		/// Restores the specified window from the Windows System Tray.
		/// </summary>
		/// <param name="hWnd">The handle of the Window to restore.</param>
		/// <exception cref="ArgumentException">The handle is not a handle to a 
		/// <see cref="System.Windows.Forms.Form"/> instance.</exception>
		public void RestoreFromTray(IntPtr hWnd)
		{
			// We need to animate the restore if the user has Window Animation enabled
			if (UseWindowAnimation())
			{
				// Get the window's current rectangle so we know where to finish the animation
				RECT rectTo = new RECT();
				Win32.GetWindowRect(hWnd, ref rectTo);

				// Get the System Tray's rectangle so we know where to start the animation
				RECT rectFrom = new RECT();
				GetTrayWndRect(ref rectFrom);

				// Do the animation
				Win32.DrawAnimatedRects(hWnd, Win32.IDANI_CAPTION, ref rectFrom, ref rectTo);
			}
			
			// Get a form for the window
			Form window = Form.FromHandle(hWnd) as Form;
			if(null != window)
			{
				// Restore the window
				window.WindowState = FormWindowState.Normal;
				
				// ShowInTaskbar if necessary then display the window
				//window.ShowInTaskbar = showInTaskBar;
				window.Visible = true;
				window.Activate();
				window.BringToFront();
			}
			else
				throw new ArgumentException("The window handle is not a handle to a Form instance. " +
					"Please provide a valid handle.", "hWnd");

		}
		#endregion

		#region Private Helper Methods
		/// <summary>
		/// Private helper function that updates the System Tray Icon. 
		/// </summary>
		/// <param name="showIcon">If true the icon is shown in the System Tray based on the 
		/// current property values. If false the icon is removed from the System Tray.</param>
		private void UpdateIcon(bool showIcon)
		{
			
			if (this.DesignMode)
				return;

			//IntSecurity.UnrestrictedWindows.Demand();
			
			// Initialize the structure
			NOTIFYICONDATA nid = new NOTIFYICONDATA();
			nid.uCallbackMessage = Win32.WM_USER_TRAY;
			nid.uVersion = Win32.NOTIFYICON_VERSION;
			nid.cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA));
			nid.uFlags = Win32.NIF_MESSAGE;
			nid.hwnd = this._Messages.Handle;
			nid.uID = 0;
			nid.hIcon = IntPtr.Zero;
			nid.szTip = null;

			// If we have a valid icon, make sure we send it.
			if (this.Icon != null) 
			{
				nid.uFlags = nid.uFlags | Win32.NIF_ICON;
				nid.hIcon = this.Icon.Handle;
			}

			// Pass our Text property as the tooltip
			nid.uFlags = nid.uFlags | Win32.NIF_TIP;
			nid.szTip = this.Text;

			if (showIcon && this.Icon != null) 
			{	// Show the icon 
				if (!this._Added) 
				{ // We don't already have an Icon so we need to creat one.
					Win32.Shell_NotifyIcon(Win32.NIM_ADD, ref nid);
					this._Added = true;
					//return; 
				}
				else	// We already have an Icon so we'll just modify it.
					Win32.Shell_NotifyIcon(Win32.NIM_MODIFY, ref nid);
			}
			else if(this._Added) 
			{	// The caller wants to remove the icon and we have an existing icon to remove
				Win32.Shell_NotifyIcon(Win32.NIM_DELETE, ref nid);
				this._Added = false;
			}
		}

		/// <summary>
		/// Private helper method that indicates whether the user has Window Animation enabled.
		/// </summary>
		/// <returns>true if the user has Window Animation enabled. Otherwise returns false.</returns>
		private bool UseWindowAnimation()
		{
			ANIMATIONINFO animationInfo = new ANIMATIONINFO();
			
			animationInfo.cbSize = Marshal.SizeOf(animationInfo);

			Win32.SystemParametersInfo(Win32.SPI_GETANIMATION, animationInfo.cbSize, 
				ref animationInfo, 0);

			bool animateMinimize = (animationInfo.iMinAnimate != 0);
			return animateMinimize;
		}

		


		/// <summary>
		/// Private helper method that hides the window represented by the given window handle.
		/// </summary>
		/// <param name="hWnd">The handle of the window to hide.</param>
		private void HideWindow(IntPtr hWnd)
		{
			Form window = Form.FromHandle(hWnd) as Form;

			if(null != window)
			{
				//window.ShowInTaskbar = false;
				window.Visible = false;
			}
		}

		
		
		/// <summary>
		/// Private helper method that obtains the rectangle of the System Tray minus the Clock's rectangle.
		/// This rectangle is used during Minimize and Restore animations.
		/// </summary>
		/// <param name="lprect">The rectangle that receives the System Tray's rectangle values.</param>
		private void GetTrayWndRect(ref RECT lprect)
		{
			int defaultWidth = 150;
			int defaultHeight = 30;

			IntPtr hShellTrayWnd = Win32.FindWindow("Shell_TrayWnd", null);

			if(IntPtr.Zero != hShellTrayWnd)
			{	
				// We found the System Tray's handle, let's get its rectangle
				Win32.GetWindowRect(hShellTrayWnd, ref lprect);

				EnumChildProc callback = new EnumChildProc(FindTrayWindow);
				Win32.EnumChildWindows(hShellTrayWnd, callback, ref lprect);
			}
			else
			{
				// OK. Haven't found a thing. Provide a default rect based on the current work
				// area
			
				Rectangle workArea = SystemInformation.WorkingArea;
				lprect.right = workArea.Right;
				lprect.bottom = workArea.Bottom;
				lprect.left = workArea.Right - defaultWidth;
				lprect.top  = workArea.Bottom - defaultHeight;
			}
		}

		/// <summary>
		/// Callback for EnumChildWindows called in GetTrayWndRect.
		/// </summary>
		/// <param name="hwnd">The handle for the child window.</param>
		/// <param name="lParam">The Rectangle passed from GetTrayWndRect.</param>
		/// <returns>Returns true if EnumChildWindows should continue enumerating children.
		/// Returns false if EnumChildWindows should quit enumerating children.</returns>
		private bool FindTrayWindow(IntPtr hwnd, ref RECT lParam)
		{
			// Initialize a string to the max class name length. 
			string szClassName = new string(' ', 256);
			
			Win32.GetClassName(hwnd, szClassName, szClassName.Length - 1);
			
			// Did we find the Main System Tray? If so, then get its size and keep going
			if (szClassName.StartsWith("TrayNotifyWnd"))
			{
				//RECT lpRect = (RECT) lParam;
				Win32.GetWindowRect(hwnd, ref lParam);

				// We aren't done yet. Keep looking for children windows
				return true;
			}
			
			// Did we find the System Clock? If so, then adjust the size of the rectangle
			// so our rectangle will be between the outside edge of the tray and the edge
			// of the clock. In other words, our rectangle will not include the clock's 
			// rectangle. This works because the clock window will be found after the 
			// System Tray window. Once we have our new size can quit looking for remaining
			// children.
			if(szClassName.StartsWith("TrayClockWClass"))
			{
				//RECT lpRect = (RECT)lParam;
				
				RECT rectClock = new RECT();
				Win32.GetWindowRect(hwnd, ref rectClock);

				// if clock is above system tray adjust accordingly
				if (rectClock.bottom < lParam.bottom - 5) // 10 = random fudge factor.
				{
					lParam.top = rectClock.bottom;
				}
				else
				{
					lParam.right = rectClock.left;
				}

				// Quit looking for children
				return false;
			}
			
			// We aren't done yet. Keep looking for children windows
			return true;
		}

		/// <summary>
		/// Event handler for the MessageHandler's Click event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event parameters.</param>
		private void Messages_Click(object sender, EventArgs e) 
		{
			OnClick(e);
		}

		/// <summary>
		/// Event handler for the MessageHandler's DoubleClick event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event parameters.</param>
		private void Messages_DoubleClick(object sender, EventArgs e)
		{
			OnDoubleClick(e);
		}

		/// <summary>
		/// Event handler for the MessageHandler's MouseDown event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event parameters.</param>
		private void Messages_MouseDown(object sender, MouseEventArgs e)
		{
			OnMouseDown(e);
		}

		/// <summary>
		/// Event handler for the MessageHandler's MouseMove event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event parameters.</param>
		private void Messages_MouseMove(object sender, MouseEventArgs e)
		{
			OnMouseMove(e);
		}

		/// <summary>
		/// Event handler for the MessageHandler's MouseUp event.
		/// </summary>
		/// <remarks>If the right mouse button was clicked, then the ContextMenu will be shown
		/// at the current cursor location.</remarks>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event parameters.</param>
		private void Messages_MouseUp(object sender, MouseEventArgs e)
		{
			
			OnMouseUp(e);

			if(e.Button == MouseButtons.Right)
			{
				_Messages.Activate();

				Point zeroPoint = new Point(0, 0);

				int newX = Cursor.Position.X - 
					_Messages.PointToScreen(zeroPoint).X;

				int newY = Cursor.Position.Y - 
					_Messages.PointToScreen(zeroPoint).Y;

				Point position = new Point(newX, newY);

				if(_ContextMenu != null)
					_ContextMenu.Show(_Messages, position);
			}
		
		}

		/// <summary>
		/// Event handler for the MessageHandler's Reload event. 
		/// </summary>
		/// <remarks>The reload event is fired after the taskbar is recreated. All applications
		/// with icons in the system tray should recreate their icons after the taskbar 
		/// is recreated. This event handler must take the neccessary steps to make sure our
		/// icon is recreated if needed.</remarks>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event parameters.</param>
		private void Messages_Reload(object sender, EventArgs e)
		{
			// The taskbar was recreated, we must assume that any existing icons we had were
			// lost
			this._Added = false;

			// If we have a visible icon make sure it gets recreated.
			if(Visible)
			{
				UpdateIcon(true);
			}
		}

		/// <summary>
		/// Event handler for the MessageHandler's BalloonShow event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event parameters.</param>
		private void Messages_BalloonShow(object sender, EventArgs e)
		{
			OnBalloonShow(e);
		}

		/// <summary>
		/// Event handler for the MessageHandler's BalloonHide event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event parameters.</param>
		private void Messages_BalloonHide(object sender, EventArgs e)
		{
			OnBalloonHide(e);
		}

		/// <summary>
		/// Event handler for the MessageHandler's BalloonTimeout event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event parameters.</param>
		private void Messages_BalloonTimeout(object sender, EventArgs e)
		{
		
			// Hide the Icon if it was hidden before showing the balloon
			if(!_VisibleBeforeBalloon)
				Visible = false;

			//this.UpdateIcon(this.Visible);

			OnBalloonTimeout(e);
		}

		/// <summary>
		/// Event handler for the MessageHandler's BalloonClick event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event parameters.</param>
		private void Messages_BalloonClick(object sender, EventArgs e)
		{
			// Hide the Icon if it was hidden before showing the balloon
			if(!_VisibleBeforeBalloon)
				Visible = false;

			//this.UpdateIcon(this.Visible);

			OnBalloonClick(e);
		}

		/// <summary>
		/// Fires our Click event.
		/// </summary>
		/// <param name="e">The event parameters.</param>
		private void OnClick(EventArgs e)
		{
			if(Click != null)
				Click(this, e);
		}

		/// <summary>
		/// Fires our DoubleClick event.
		/// </summary>
		/// <param name="e">The event parameters.</param>
		private void OnDoubleClick(EventArgs e)
		{
			if(DoubleClick != null)
				DoubleClick(this, e);
		}
		
		/// <summary>
		/// Fires our MouseDown event.
		/// </summary>
		/// <param name="e">The event parameters.</param>
		private void OnMouseDown(MouseEventArgs e)
		{
			if(MouseDown != null)
				MouseDown(this, e);
		}

		/// <summary>
		/// Fires our MouseMove event.
		/// </summary>
		/// <param name="e">The event parameters.</param>
		private void OnMouseMove(MouseEventArgs e)
		{
			if(MouseMove != null)
				MouseMove(this, e);
		}

		/// <summary>
		/// Fires our MouseUp event.
		/// </summary>
		/// <param name="e">The event parameters.</param>
		private void OnMouseUp(MouseEventArgs e)
		{
			/*
			if(MouseButtons.Right == e.Button &&
				null != this.ContextMenu)
			{
				this.ContextMenu.Show(this._Messages, new Point(e.X, e.Y));
			}
			*/
			if(MouseUp != null)
				MouseUp(this, e);
		}
		
		/// <summary>
		/// Fires our BalloonShow event.
		/// </summary>
		/// <param name="e">The event parameters.</param>
		private void OnBalloonShow(EventArgs e)
		{
			if(BalloonShow != null)
				BalloonShow(this, e);
		}
        
		/// <summary>
		/// Fires our BalloonHide event.
		/// </summary>
		/// <param name="e">The event parameters.</param>
		private void OnBalloonHide(EventArgs e)
		{
			if(BalloonHide != null)
				BalloonHide(this, e);
		}

		/// <summary>
		/// Fires our BalloonTimeout event.
		/// </summary>
		/// <param name="e">The event parameters.</param>
		private void OnBalloonTimeout(EventArgs e)
		{
			if(BalloonTimeout != null)
				BalloonTimeout(this, e);
		}

		/// <summary>
		/// Fires our BalloonClick event.
		/// </summary>
		/// <param name="e">The event parameters.</param>
		private void OnBalloonClick(EventArgs e)
		{
			if(BalloonClick != null)
				BalloonClick(this, e);
		}
		#endregion
		
	} // end NotifyIcon

} // end MattGriffith.Windows.Forms
