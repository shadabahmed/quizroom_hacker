using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Quizroom_Hacker
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainFormClass : System.Windows.Forms.Form
	{
		
		#region Singleton Implementation

		private static MainFormClass mainForm;

		/// <summary>
		/// Gets the singleton form variable.
		/// </summary>
		public static MainFormClass MainForm
		{
			get
			{
				if(mainForm==null)
					mainForm=new MainFormClass();
				return mainForm;
			}
		}
		
		#endregion

		#region Variables
		private System.Windows.Forms.Button startButton;
		
		private System.Windows.Forms.GroupBox settingsGroupBox;
		private System.Windows.Forms.TextBox nickTextBox;
		private System.Windows.Forms.Label nickLabel;
		private System.Windows.Forms.TrackBar responseDelayTrackBar;
		private System.Windows.Forms.GroupBox responseDelayGroupBox;
		private System.Windows.Forms.GroupBox interchoiceDelayGroupBox;
		private System.Windows.Forms.TrackBar interchoiceDelayTrackBar;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem menuItem4;
		private MattGriffith.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.CheckBox notificationCheckBox;
		private System.Windows.Forms.CheckBox modeCheckBox;
		private System.Windows.Forms.MenuItem aboutMenuItem;
		private System.Windows.Forms.MenuItem showMainMenuItem;
		private System.Windows.Forms.MenuItem hidenotificationsMenuItem;
		private System.Windows.Forms.MenuItem exitMenuItem;
		private System.Windows.Forms.MenuItem learnmodeMenuItem;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.MenuItem showLastMenuItem;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown timerIntervalNumericUpDown;
		private System.ComponentModel.IContainer components;
			
		#endregion

		#region Contructor


		public MainFormClass()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			WaitCallback async = new WaitCallback(DictLoader);
			ThreadPool.QueueUserWorkItem(async);
			//new MethodInvoker(DictLoader).BeginInvoke(null,null);
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		void DictLoader(object asyncObject)
		{
			QuizRoom.LoadDictionary();
			MainFormClass.MainForm.Invoke(new MethodInvoker(DictLoaded));
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainFormClass));
			this.startButton = new System.Windows.Forms.Button();
			this.settingsGroupBox = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.timerIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.notificationCheckBox = new System.Windows.Forms.CheckBox();
			this.responseDelayGroupBox = new System.Windows.Forms.GroupBox();
			this.responseDelayTrackBar = new System.Windows.Forms.TrackBar();
			this.nickLabel = new System.Windows.Forms.Label();
			this.nickTextBox = new System.Windows.Forms.TextBox();
			this.interchoiceDelayGroupBox = new System.Windows.Forms.GroupBox();
			this.interchoiceDelayTrackBar = new System.Windows.Forms.TrackBar();
			this.modeCheckBox = new System.Windows.Forms.CheckBox();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.showMainMenuItem = new System.Windows.Forms.MenuItem();
			this.showLastMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.learnmodeMenuItem = new System.Windows.Forms.MenuItem();
			this.hidenotificationsMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.aboutMenuItem = new System.Windows.Forms.MenuItem();
			this.exitMenuItem = new System.Windows.Forms.MenuItem();
			this.notifyIcon = new MattGriffith.Windows.Forms.NotifyIcon(this.components);
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.settingsGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.timerIntervalNumericUpDown)).BeginInit();
			this.responseDelayGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.responseDelayTrackBar)).BeginInit();
			this.interchoiceDelayGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.interchoiceDelayTrackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// startButton
			// 
			this.startButton.Enabled = false;
			this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.startButton.Location = new System.Drawing.Point(64, 296);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(160, 23);
			this.startButton.TabIndex = 0;
			this.startButton.Text = "Loading Dictionary ...";
			this.startButton.Click += new System.EventHandler(this.startButton_Click);
			// 
			// settingsGroupBox
			// 
			this.settingsGroupBox.Controls.Add(this.label2);
			this.settingsGroupBox.Controls.Add(this.timerIntervalNumericUpDown);
			this.settingsGroupBox.Controls.Add(this.label1);
			this.settingsGroupBox.Controls.Add(this.notificationCheckBox);
			this.settingsGroupBox.Controls.Add(this.responseDelayGroupBox);
			this.settingsGroupBox.Controls.Add(this.nickLabel);
			this.settingsGroupBox.Controls.Add(this.nickTextBox);
			this.settingsGroupBox.Controls.Add(this.interchoiceDelayGroupBox);
			this.settingsGroupBox.Controls.Add(this.modeCheckBox);
			this.settingsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.settingsGroupBox.Location = new System.Drawing.Point(8, 8);
			this.settingsGroupBox.Name = "settingsGroupBox";
			this.settingsGroupBox.Size = new System.Drawing.Size(280, 280);
			this.settingsGroupBox.TabIndex = 1;
			this.settingsGroupBox.TabStop = false;
			this.settingsGroupBox.Text = "Settings";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(216, 204);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 8;
			this.label2.Text = "seconds";
			// 
			// timerIntervalNumericUpDown
			// 
			this.timerIntervalNumericUpDown.Location = new System.Drawing.Point(168, 204);
			this.timerIntervalNumericUpDown.Maximum = new System.Decimal(new int[] {
																					   20,
																					   0,
																					   0,
																					   0});
			this.timerIntervalNumericUpDown.Minimum = new System.Decimal(new int[] {
																					   2,
																					   0,
																					   0,
																					   0});
			this.timerIntervalNumericUpDown.Name = "timerIntervalNumericUpDown";
			this.timerIntervalNumericUpDown.Size = new System.Drawing.Size(44, 20);
			this.timerIntervalNumericUpDown.TabIndex = 7;
			this.timerIntervalNumericUpDown.Value = new System.Decimal(new int[] {
																					 4,
																					 0,
																					 0,
																					 0});
			this.timerIntervalNumericUpDown.ValueChanged += new System.EventHandler(this.timerIntervalNumericUpDown_ValueChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 208);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(116, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Notification Timeout :";
			// 
			// notificationCheckBox
			// 
			this.notificationCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.notificationCheckBox.Location = new System.Drawing.Point(28, 240);
			this.notificationCheckBox.Name = "notificationCheckBox";
			this.notificationCheckBox.TabIndex = 5;
			this.notificationCheckBox.Text = "Hide Notifications";
			this.notificationCheckBox.CheckStateChanged += new System.EventHandler(this.notificationCheckBox_CheckStateChanged);
			// 
			// responseDelayGroupBox
			// 
			this.responseDelayGroupBox.Controls.Add(this.responseDelayTrackBar);
			this.responseDelayGroupBox.Location = new System.Drawing.Point(12, 56);
			this.responseDelayGroupBox.Name = "responseDelayGroupBox";
			this.responseDelayGroupBox.Size = new System.Drawing.Size(252, 64);
			this.responseDelayGroupBox.TabIndex = 3;
			this.responseDelayGroupBox.TabStop = false;
			this.responseDelayGroupBox.Text = "Response Delay : 0ms";
			// 
			// responseDelayTrackBar
			// 
			this.responseDelayTrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.responseDelayTrackBar.LargeChange = 200;
			this.responseDelayTrackBar.Location = new System.Drawing.Point(3, 16);
			this.responseDelayTrackBar.Maximum = 5000;
			this.responseDelayTrackBar.Name = "responseDelayTrackBar";
			this.responseDelayTrackBar.Size = new System.Drawing.Size(246, 45);
			this.responseDelayTrackBar.SmallChange = 50;
			this.responseDelayTrackBar.TabIndex = 2;
			this.responseDelayTrackBar.TickFrequency = 200;
			this.responseDelayTrackBar.ValueChanged += new System.EventHandler(this.responseDelayTrackBar_ValueChanged);
			// 
			// nickLabel
			// 
			this.nickLabel.Location = new System.Drawing.Point(16, 28);
			this.nickLabel.Name = "nickLabel";
			this.nickLabel.Size = new System.Drawing.Size(80, 16);
			this.nickLabel.TabIndex = 1;
			this.nickLabel.Text = "Nickname :";
			// 
			// nickTextBox
			// 
			this.nickTextBox.Location = new System.Drawing.Point(104, 24);
			this.nickTextBox.Name = "nickTextBox";
			this.nickTextBox.Size = new System.Drawing.Size(160, 20);
			this.nickTextBox.TabIndex = 0;
			this.nickTextBox.Text = "";
			// 
			// interchoiceDelayGroupBox
			// 
			this.interchoiceDelayGroupBox.Controls.Add(this.interchoiceDelayTrackBar);
			this.interchoiceDelayGroupBox.Location = new System.Drawing.Point(12, 128);
			this.interchoiceDelayGroupBox.Name = "interchoiceDelayGroupBox";
			this.interchoiceDelayGroupBox.Size = new System.Drawing.Size(252, 64);
			this.interchoiceDelayGroupBox.TabIndex = 4;
			this.interchoiceDelayGroupBox.TabStop = false;
			this.interchoiceDelayGroupBox.Text = "Delay between correct choices: 0ms";
			// 
			// interchoiceDelayTrackBar
			// 
			this.interchoiceDelayTrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.interchoiceDelayTrackBar.LargeChange = 100;
			this.interchoiceDelayTrackBar.Location = new System.Drawing.Point(3, 16);
			this.interchoiceDelayTrackBar.Maximum = 2000;
			this.interchoiceDelayTrackBar.Name = "interchoiceDelayTrackBar";
			this.interchoiceDelayTrackBar.Size = new System.Drawing.Size(246, 45);
			this.interchoiceDelayTrackBar.SmallChange = 50;
			this.interchoiceDelayTrackBar.TabIndex = 2;
			this.interchoiceDelayTrackBar.TickFrequency = 100;
			this.interchoiceDelayTrackBar.ValueChanged += new System.EventHandler(this.interchoiceDelayTrackBar_ValueChanged);
			// 
			// modeCheckBox
			// 
			this.modeCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.modeCheckBox.Location = new System.Drawing.Point(144, 240);
			this.modeCheckBox.Name = "modeCheckBox";
			this.modeCheckBox.Size = new System.Drawing.Size(120, 24);
			this.modeCheckBox.TabIndex = 2;
			this.modeCheckBox.Text = "Disable Auto-Answer";
			this.modeCheckBox.CheckStateChanged += new System.EventHandler(this.modeCheckBox_CheckStateChanged);
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.showMainMenuItem,
																						this.showLastMenuItem,
																						this.menuItem7,
																						this.learnmodeMenuItem,
																						this.hidenotificationsMenuItem,
																						this.menuItem4,
																						this.aboutMenuItem,
																						this.exitMenuItem});
			// 
			// showMainMenuItem
			// 
			this.showMainMenuItem.Index = 0;
			this.showMainMenuItem.Text = "Show Main Window";
			this.showMainMenuItem.Click += new System.EventHandler(this.notifyIcon_DoubleClick);
			// 
			// showLastMenuItem
			// 
			this.showLastMenuItem.Index = 1;
			this.showLastMenuItem.Text = "Show Last Jumble";
			this.showLastMenuItem.Click += new System.EventHandler(this.showLastMenuItem_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 2;
			this.menuItem7.Text = "-";
			// 
			// learnmodeMenuItem
			// 
			this.learnmodeMenuItem.Index = 3;
			this.learnmodeMenuItem.Text = "Disable Auto-Answer";
			this.learnmodeMenuItem.Click += new System.EventHandler(this.learnmodeMenuItem_Click);
			// 
			// hidenotificationsMenuItem
			// 
			this.hidenotificationsMenuItem.Index = 4;
			this.hidenotificationsMenuItem.Text = "Hide Notifications";
			this.hidenotificationsMenuItem.Click += new System.EventHandler(this.hidenotificationsMenuItem_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 5;
			this.menuItem4.Text = "-";
			// 
			// aboutMenuItem
			// 
			this.aboutMenuItem.Index = 6;
			this.aboutMenuItem.Text = "About";
			this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
			// 
			// exitMenuItem
			// 
			this.exitMenuItem.Index = 7;
			this.exitMenuItem.Text = "Exit";
			this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
			// 
			// notifyIcon
			// 
			this.notifyIcon.ContextMenu = this.contextMenu;
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "";
			this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
			// 
			// timer
			// 
			this.timer.Interval = 4000;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// MainFormClass
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(290, 330);
			this.Controls.Add(this.settingsGroupBox);
			this.Controls.Add(this.startButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "MainFormClass";
			this.Text = "QuizRoom Hacker";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MainFormClass_Closing);
			this.SizeChanged += new System.EventHandler(this.MainFormClass_SizeChanged);
			this.settingsGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.timerIntervalNumericUpDown)).EndInit();
			this.responseDelayGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.responseDelayTrackBar)).EndInit();
			this.interchoiceDelayGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.interchoiceDelayTrackBar)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Main Method
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.EnableVisualStyles();
			Application.Run(MainFormClass.MainForm);
		}
		#endregion

		#region EventHandlers

		private void SaveConfig()
		{
			try
			{
				FileStream stream=new FileStream("config.ini",FileMode.Create);
				StreamWriter writer = new StreamWriter(stream);
				writer.WriteLine("[Configuration]");
				writer.WriteLine("Nick = {0}",nickTextBox.Text);
				writer.WriteLine("Response Delay = {0}",responseDelayTrackBar.Value);
				writer.WriteLine("Delay between correct choices = {0}",interchoiceDelayTrackBar.Value);
				writer.WriteLine("Notification Timeout = {0}",timerIntervalNumericUpDown.Value);
				writer.WriteLine("Hide Notifications = {0}",notificationCheckBox.Checked);
				writer.WriteLine("Disable Auto-Answer = {0}",modeCheckBox.Checked);
				writer.Flush();
				writer.Close();
				stream.Close();
			}
			catch{}
		}

		private void LoadConfig()
		{
			try
			{
				StreamReader reader= new StreamReader("config.ini");
				if(reader.ReadLine()!="[Configuration]")
					return;
				nickTextBox.Text = reader.ReadLine().Substring(7);
				responseDelayTrackBar.Value=int.Parse(reader.ReadLine().Substring(17));
				interchoiceDelayTrackBar.Value=int.Parse(reader.ReadLine().Substring(32));
				timerIntervalNumericUpDown.Value=decimal.Parse(reader.ReadLine().Substring(23));
				notificationCheckBox.Checked=bool.Parse(reader.ReadLine().Substring(21));
				modeCheckBox.Checked=bool.Parse(reader.ReadLine().Substring(22));
				reader.Close();

			}
			catch{}
		}

		
		private void DictLoaded()
		{
			if(File.Exists(Application.StartupPath+"\\config.ini"))
				LoadConfig();
			startButton.Enabled=true;
			startButton.Text="Start";
			startButton.Focus();
			if(nickTextBox.Text.Length==0)
				nickTextBox.Text="SERVER™";
		}

		private void startButton_Click(object sender, System.EventArgs e)
		{
			if(startButton.Text=="Start")
			{
				if(QuizRoom.GetHandles())
				{
					notifyIcon.MinimizeToTray(Handle);
					QuizRoom.ChatNick=nickTextBox.Text;
					QuizRoom.ResponseDelay=responseDelayTrackBar.Value;
					nickTextBox.Enabled=false;
					startButton.Text="Stop";
					notifyIcon.Visible=true;
					notifyIcon.ShowBalloon(MattGriffith.Interop.BalloonIconStyle.Info,"QuizRoom Hacker Started !","Waiting for jumble phrase");
					timer.Enabled=true;
					timer.Start();
				}
			}
			else
			{
				nickTextBox.Enabled=true;
				QuizRoom.Stop();
				startButton.Text="Start";
				notifyIcon.Visible=false;
			}
		}
		
		private string lastJumble,lastChoices;

		public void ShowBalloon(string jumble,string choices)
		{
			lastJumble=jumble;
			lastChoices=choices;
			if(!notificationCheckBox.Checked)
			{
				timer.Enabled=false;
				timer.Enabled=true;
				timer.Start();
				notifyIcon.ShowBalloon(MattGriffith.Interop.BalloonIconStyle.Info,jumble,choices);
	
			}
		}

		private void responseDelayTrackBar_ValueChanged(object sender, System.EventArgs e)
		{
			responseDelayGroupBox.Text="Response Delay : "+responseDelayTrackBar.Value+"ms";
			QuizRoom.ResponseDelay=responseDelayTrackBar.Value;
		}

		private void interchoiceDelayTrackBar_ValueChanged(object sender, System.EventArgs e)
		{
			interchoiceDelayGroupBox.Text="Delay between correct choices: "+interchoiceDelayTrackBar.Value+"ms";
			QuizRoom.InterChoiceDelay=interchoiceDelayTrackBar.Value;
		}

		private void MainFormClass_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(QuizRoom.IsWordListModified)
			{
				QuizRoom.Close();
				QuizRoom.SaveDictionary();
			}
			SaveConfig();
			notifyIcon.Visible=false;
		}

		private void notifyIcon_DoubleClick(object sender, System.EventArgs e)
		{
			Visible=true;		
			notifyIcon.RestoreFromTray(Handle);
		}

		private void notifyIcon_BalloonTimeout(object sender, System.EventArgs e)
		{
			notifyIcon.Visible=false;
			notifyIcon.Visible=true;
		}

		private void MainFormClass_SizeChanged(object sender, System.EventArgs e)
		{
			if(startButton.Text=="Stop")
				if(WindowState==FormWindowState.Minimized)
				{
					notifyIcon.MinimizeToTray(Handle);
				}
		}

		private void notificationCheckBox_CheckStateChanged(object sender, System.EventArgs e)
		{
			QuizRoom.HideNotifications=notificationCheckBox.Checked;
			hidenotificationsMenuItem.Checked=notificationCheckBox.Checked;
		}

		private void modeCheckBox_CheckStateChanged(object sender, System.EventArgs e)
		{
			QuizRoom.LearningMode=modeCheckBox.Checked;
			learnmodeMenuItem.Checked=modeCheckBox.Checked;
		}

		private void learnmodeMenuItem_Click(object sender, System.EventArgs e)
		{
			learnmodeMenuItem.Checked=!learnmodeMenuItem.Checked;
			modeCheckBox.Checked=learnmodeMenuItem.Checked;
		}

		private void hidenotificationsMenuItem_Click(object sender, System.EventArgs e)
		{
			hidenotificationsMenuItem.Checked=!hidenotificationsMenuItem.Checked;
			notificationCheckBox.Checked=hidenotificationsMenuItem.Checked;
		}

		private void exitMenuItem_Click(object sender, System.EventArgs e)
		{
			Close();
			Application.Exit();
		}

		private void aboutMenuItem_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(this,"QuizRoom Hacker v1.0 \nCreated by Shadab Ahmed aka SERVER™","QuizRoom Hacker",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}

		private void timer_Tick(object sender, System.EventArgs e)
		{
			notifyIcon.HideBalloon();
			timer.Enabled=false;
		}

		private void showLastMenuItem_Click(object sender, System.EventArgs e)
		{
			if(lastJumble!=null && lastChoices!=null)
			{
				timer.Enabled=false;
				timer.Enabled=true;
				timer.Start();
				notifyIcon.ShowBalloon(MattGriffith.Interop.BalloonIconStyle.Info,lastJumble,lastChoices);
			}
		}
		#endregion

		private void timerIntervalNumericUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			timer.Interval=(int)timerIntervalNumericUpDown.Value*1000;
		}	
	}
}