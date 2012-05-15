using System;
using System.Windows.Forms;
using System.Text;
using System.Threading;

namespace Quizroom_Hacker
{
	/// <summary>
	/// Summary description for NativeWindowEx.
	/// </summary>
	public class NativeWindowEx : NativeWindow
	{
		WaitCallback callbackThread = new WaitCallback(QuizRoom.ProcessBotMessage);
		bool isRunning;
		public bool IsRunning
		{
			get{return isRunning;}
			set{isRunning = value;}
		}

		public NativeWindowEx()
		{
			CreateParams cp = new CreateParams();
			CreateHandle(cp);
		}

		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name="FullTrust")]
		protected override void WndProc(ref Message m)
		{
			if(m.Msg==QuizRoom.WM_COPYDATA)
			{
				if(isRunning)
				{
					QuizRoom.COPYDATASTRUCT dataStruct=(QuizRoom.COPYDATASTRUCT)m.GetLParam(typeof(QuizRoom.COPYDATASTRUCT));
					ThreadPool.QueueUserWorkItem(callbackThread,dataStruct.lpData);
				}
				return;
			}
			base.WndProc (ref m);
		}
	}
}