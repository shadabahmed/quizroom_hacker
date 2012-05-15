using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Quizroom_Hacker
{
	/// <summary>
	/// Summary description for QuizRoom.
	/// </summary>
	public class QuizRoom
	{

		#region Variables
		public static Hashtable wordList;
		private static bool isWordListModified;
		private static NativeWindowEx nativeWindow;
		private static int responseDelay;
		private static int interchoiceDelay;
		private static string chatNick;
		private static MyDelegate ShowBalloon=new MyDelegate(MainFormClass.MainForm.ShowBalloon);
		private static bool hideNotifications;
		private static bool learningMode;
		private static uint sessionScore;
		#endregion

		#region Properties
		public static int ResponseDelay
		{
			get{return responseDelay;}
			set{responseDelay=value;}
		}

		public static int InterChoiceDelay
		{
			get{return interchoiceDelay;}
			set{interchoiceDelay=value;}
		}

		public static string ChatNick
		{
			get{return chatNick;}
			set{chatNick=value;}
		}

		public static bool IsWordListModified
		{
			get{return isWordListModified;}
		}

		public static bool HideNotifications
		{
			get{return hideNotifications;}
			set{hideNotifications=value;}
		}

		public static bool LearningMode
		{
			set{learningMode=value;}
			get{return learningMode;}
		}
		#endregion

		#region Handles
		static IntPtr dcppHandle=IntPtr.Zero;
		static IntPtr chatWindowHandle=IntPtr.Zero;
		static IntPtr editWindowHandle=IntPtr.Zero;
		#endregion
	
		#region Properties
		public static IntPtr DcppHandle
		{
			get{return dcppHandle;}
		}

		public static IntPtr ChatWindowHandle
		{
			get{return chatWindowHandle;}
		}

		public static IntPtr EditWindowHandle
		{
			get{return editWindowHandle;}
		}
		#endregion

		#region Win32 Consts
		public const UInt32 WM_SETTEXT		 = 0x000C;
		public const UInt32 WM_CHAR			 = 0x0102;
		public const UInt32 WM_KEYDOWN		 = 0x0100;
		public const UInt32 WM_CONTEXTMENU	 = 0x007B;
		public const UInt32 WM_COPY           = 0x0301;
		public const UInt32 WM_COPYDATA       = 0x004A;

		public static UInt32 MY_MESSAGE = 0;

		#endregion

		#region Win32 Imports
		
		[DllImport("user32.dll", SetLastError = true)]
		static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
		
		[DllImport("user32.dll")]
		static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, IntPtr lParam);
		
		[DllImport("user32.dll")]
		static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, [Out] StringBuilder lParam);
		
		[DllImport("user32.dll", SetLastError = true)]
		static extern bool PostMessage(IntPtr hWnd, uint Msg,uint wParam,[Out] StringBuilder lParam);
		
		[DllImport("user32.dll", SetLastError = true)]
		static extern bool PostMessage(IntPtr hWnd, uint Msg, uint wParam,IntPtr lParam);
		
		[DllImport("user32.dll")]
		private static extern bool SetWindowText(IntPtr hWnd, string lpString);
		
		[DllImport("user32.dll", SetLastError=true, CharSet=CharSet.Auto)]
		static extern uint RegisterWindowMessage(string lpString);
	
		[DllImport("user32.dll", SetLastError = true)]
		static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className,  IntPtr windowTitle);
			
		#endregion

		#region Hook Dll Import
		[DllImport("Hook.dll", SetLastError = true)]
		public static extern int InjectDll(IntPtr hWnd ,IntPtr myAppHwnd);
		
		[DllImport("Hook.dll", SetLastError = true)]
		public static extern string GetString();
		
		#endregion

		#region Structs
		public struct COPYDATASTRUCT
		{
			public UInt32 dwData;
			public IntPtr cbData;
			public string lpData;
		}
		#endregion

		#region Methods

		public static void Stop()
		{
			lastUnjumble=null;
			nativeWindow.IsRunning=false;
		}
		
		public static bool GetHandles()
		{
			IntPtr oldChatWindowHandle=chatWindowHandle;
			if((dcppHandle=FindWindow("DC++",null))==IntPtr.Zero)
			{
				MessageBox.Show("DC++ not running !","Error!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return false;
			}
			if((chatWindowHandle=FindWindowEx(dcppHandle,IntPtr.Zero,"MDIClient",null))==IntPtr.Zero)
			{
				MessageBox.Show("Tabs not found.Make sure you are using unmodified DC++!","Error!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return false;
			}
			if((chatWindowHandle=FindWindowEx(chatWindowHandle,IntPtr.Zero,null,"#[QuizRoom] - HiT Hi FiT Hai"))!=IntPtr.Zero)
			{
					editWindowHandle=chatWindowHandle;
					chatWindowHandle=FindWindowEx(chatWindowHandle,IntPtr.Zero,"Edit",null);
					editWindowHandle=FindWindowEx(editWindowHandle,chatWindowHandle,"Edit",null);
			}
			else
			{
					MessageBox.Show("Quizroom not open ! Please open and join the quizroom before starting.","Error!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return false;
			}
			if(oldChatWindowHandle!=chatWindowHandle)
			{
				nativeWindow=new NativeWindowEx();
				InjectDll(chatWindowHandle,nativeWindow.Handle);
			}
			nativeWindow.IsRunning=true;
			sessionScore=0;
			return true;	
		}
		
		public static string Sort(string data) 
		{
			char [] dataArray=data.ToLower().ToCharArray();
			Array.Sort(dataArray);
			return new String(dataArray);
		}
		
		private static string jumble;
		private static string lastUnjumble;
		
		public static void ProcessBotMessage(object messageObject)
		{
			string message=(messageObject as string);
			if(message.StartsWith(" Jumble: "))
			{
				//jumble= message.Substring(message.IndexOf(':',9)+1,message.Length-message.IndexOf(':',9)-1); 
				jumble = message.Substring(23,message.Length-27); 
				message=(jumble).Replace(" ","").ToLower();
				message=Sort(message);
				if(wordList.ContainsKey(message))
				{
					ArrayList choices=wordList[message] as ArrayList;
					StringBuilder choiceList=new StringBuilder("Correct choice(s) : ",200);
					foreach(string choice in choices)
						choiceList.Append(choice+" ");
					MainFormClass.MainForm.Invoke(ShowBalloon,new object[]{choiceList.ToString(),"Jumble is : "+jumble});
					
					if(!learningMode)
					{
						Thread.Sleep(responseDelay);
						foreach(string word in choices)
						{
							foreach(uint c in word)
								PostMessage(editWindowHandle,WM_CHAR,c,(IntPtr)0x1F0001);
							PostMessage(editWindowHandle,WM_KEYDOWN,0xD,(IntPtr)0x1C0001);
							Thread.Sleep(interchoiceDelay);
						}
					}
				}
				else
				{
					MainFormClass.MainForm.Invoke(ShowBalloon,new object[]{"Word not in list ! Correct answer will be added.","Jumble is : "+jumble});				
				}
			}
			else if(message.StartsWith("The Jumble Phrase was"))
			{
				lastUnjumble=message.Substring(23);
				message=message.Substring(23).ToLower();
				string key=Sort(message);
				if(wordList.ContainsKey(key))
				{
					ArrayList arrayList= wordList[key] as ArrayList;
					if(!arrayList.Contains(message))
					{
						arrayList.Add(message);
						isWordListModified=true;
						lastUnjumble+=" (Added to List)";
					}
				}
				else
				{
					ArrayList arrayList=new ArrayList(5);
					arrayList.Add(message);
					wordList.Add(key,arrayList);
					isWordListModified=true;
					lastUnjumble+=" (Added to List)";
				}
			}
			else if(message.IndexOf("recieves 6 points")>0)
			{
				if(lastUnjumble!=null)
				{
					string winner=message.Substring(0,message.IndexOf(' '));
					message=message.Substring(message.IndexOf(' ')+1);
					string score=message.Substring(message.IndexOf(' ')+1,message.IndexOf('.')-message.IndexOf(' ')-1);
					message=message.Substring(message.IndexOf('.')+2);
					if(winner!=chatNick)
					{
						MainFormClass.MainForm.Invoke(ShowBalloon,new object[]{String.Format("Winner : {0}\nScores : {1}\nStats : {2}",winner,score,message),"Jumble was : "+lastUnjumble});
					}
					else
					{
						try
						{
							int index=score.IndexOf("and ");
							if(index>=0)
							{
								sessionScore+=uint.Parse(score.Substring(index+4,score.IndexOf(" time")-index-4));
							}
							sessionScore+=uint.Parse(score.Substring(0,score.IndexOf(" points")));
						}
						catch
						{

						}
						MainFormClass.MainForm.Invoke(ShowBalloon,new object[]{String.Format("Scored : {0}\nSession score : {1} points\nStats : {2}",score,sessionScore,message),"You Win ! Jumble was : "+lastUnjumble});
					}
				}

			}
		}
		
		public static void ProcessBotMessage(string message)
		{
			if(message.StartsWith(" Jumble: "))
			{
				jumble = message.Substring(9,message.IndexOf('*')-9).Trim(); 
				message=(jumble).Replace(" ","").ToLower();
				message=Sort(message);
				if(wordList.ContainsKey(message))
				{
					ArrayList choices=wordList[message] as ArrayList;
					if(!hideNotifications)
					{
						StringBuilder choiceList=new StringBuilder("Correct choice(s) : ",200);
						foreach(string choice in choices)
							choiceList.Append(choice+" ");
						MainFormClass.MainForm.Invoke(ShowBalloon,new object[]{choiceList.ToString(),"Jumble is : "+jumble});
					}
					if(!learningMode)
					{
						Thread.Sleep(responseDelay);
						foreach(string word in choices)
						{
							foreach(uint c in word)
								PostMessage(editWindowHandle,WM_CHAR,c,(IntPtr)0x1F0001);
							PostMessage(editWindowHandle,WM_KEYDOWN,0xD,(IntPtr)0x1C0001);
							Thread.Sleep(interchoiceDelay);
						}
					}
				}
				else if(!hideNotifications)
				{
					MainFormClass.MainForm.Invoke(ShowBalloon,new object[]{"Word not in list ! Correct answer will be added.","Jumble is : "+jumble});				
				}
			}
			else if(message.StartsWith("The Jumble Phrase was"))
			{
				lastUnjumble=message.Substring(23);
				message=message.Substring(23).ToLower();
				string key=Sort(message);
				if(wordList.ContainsKey(key))
				{
					ArrayList arrayList= wordList[key] as ArrayList;
					if(!arrayList.Contains(message))
					{
						arrayList.Add(message);
						isWordListModified=true;
						lastUnjumble+=" (Added to List)";
					}
				}
				else
				{
					ArrayList arrayList=new ArrayList(5);
					arrayList.Add(message);
					wordList.Add(key,arrayList);
					isWordListModified=true;
					lastUnjumble+=" (Added to List)";
				}
			}
			else if(!hideNotifications && message.IndexOf("recieves 6 points")>0)
			{
				if(lastUnjumble!=null)
				{
					string winner=message.Substring(0,message.IndexOf(' '));
					message=message.Substring(message.IndexOf(' ')+1);
					string score=message.Substring(message.IndexOf(' ')+1,message.IndexOf('.')-message.IndexOf(' ')-1);
					message=message.Substring(message.IndexOf('.')+2);
					if(winner!=chatNick)
					{
						MainFormClass.MainForm.Invoke(ShowBalloon,new object[]{String.Format("Winner : {0}\nScores : {1}\nStats : {2}",winner,score,message),"Jumble was : "+lastUnjumble});
					}
					else
					{
						try
						{
							int index=score.IndexOf("and ");
							if(index>=0)
							{
								sessionScore+=uint.Parse(score.Substring(index+4,score.IndexOf(" time")-index-4));
							}
							sessionScore+=uint.Parse(score.Substring(0,score.IndexOf(" points")));
						}
						catch
						{

						}
						MainFormClass.MainForm.Invoke(ShowBalloon,new object[]{String.Format("Scored : {0}\nSession score : {1} points\nStats : {2}",score,sessionScore,message),"You Win ! Jumble was : "+lastUnjumble});
					}
				}

			}
		}

		public static void LoadDictionary()
		{
			try
			{
				StreamReader streamReader=new StreamReader("cedt.dic");
				wordList = new Hashtable(65536);
				string fileLine;
				while((fileLine=streamReader.ReadLine())!=null)
				{
					string [] words=fileLine.Split(new char[]{' '});
					foreach(string word in words)
					if(word.Length<25)
					{
						string key=Sort(word);
						if(!wordList.ContainsKey(key))
						{
							ArrayList arrayList=new ArrayList(5);
							arrayList.Add(word);
							wordList.Add(key,arrayList);
						}
						else
						{
							ArrayList arrayList= wordList[key] as ArrayList;
							if(!arrayList.Contains(word))
								arrayList.Add(word);
						}
					}
				}
				streamReader.Close();
			}
			catch(Exception e)
			{
				MessageBox.Show(MainFormClass.MainForm,e.Message,"Error!",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			isWordListModified=false;
		}

		public static void SaveDictionary()
		{
			try
			{
				FileStream stream=new FileStream("cedt.dic",FileMode.Create);
				StreamWriter streamWriter=new StreamWriter(stream);
				foreach(ArrayList arrayList in wordList.Values)
				{
					StringBuilder line=new StringBuilder(100,1000);
					line.Append(((string)arrayList[0]));
					for(int index=1;index<arrayList.Count;index++)
					{
						line.Append(" "+((string)arrayList[index]));
					}
					streamWriter.WriteLine(line);
				}
				streamWriter.Flush();
				streamWriter.Close();
				stream.Close();
			}
			catch(Exception e)
			{
				MessageBox.Show(e.Message,"Error!",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}	
		}

		public static void Close()
		{
			if(nativeWindow!=null)
				nativeWindow.DestroyHandle();
		}

		public QuizRoom()
		{
			
		}
		#endregion

	}
	public delegate void MyDelegate(string jumble,string choices);
}