

using Lite.ui;


namespace Lite
{

	public class UIManager : BaseManager
	{
		WindowManager mWindowMgr = null;

		public override void OnInit()
		{
			mWindowMgr = new WindowManager();
			mWindowMgr.Init();
		}

		public override void OnDestroy()
		{
			this.Cleanup();
		}

		public IWindow GetWindow(string windowName)
		{
			WindowInfo info = this.GetWindowInfo(windowName);
			if (info != null)
				return mWindowMgr.GetWindow(info);
			return null;
		}

		public static void RegisterWindow(string filePath, int showMode, int openAction, int backgroundMode)
		{
			GameUI.AddWindowInfo(filePath, (ui.ShowMode)showMode, (ui.OpenAction)openAction, (ui.BackgroundMode)backgroundMode);
		}

		public IWindow OpenWindow(string windowName)
		{
			WindowInfo info = this.GetWindowInfo(windowName);
			if (info != null)
				return this.OpenWindow(info, null);
			else
				return null;
		}

		public IWindow OpenWindow(WindowInfo windowInfo, IContext context = null)
		{
			return mWindowMgr.OpenWindow(windowInfo, context);
		}

		public void CloseWindow(string windowName)
		{
			WindowInfo info = this.GetWindowInfo(windowName);
			if (info != null)
				mWindowMgr.CloseWindow(info);
		}

		public void CloseWindow(IWindow script)
		{
			mWindowMgr.CloseWindow(script);
		}

		public void SetMainWindow(string name)
		{
			var info = GetWindowInfo(name);
			if (info != null)
				mWindowMgr.mainWindowInfo = info;
		}

		public void BackToMainWindow()
		{
			mWindowMgr.BackToMainWindow();
		}

		public void Cleanup()
		{
			mWindowMgr.Cleanup();
		}

		private WindowInfo GetWindowInfo(string windowName)
		{
			WindowInfo info = GameUI.GetWindowInfo(windowName);
			if (info == null)
			{
				Log.Error(string.Format("Cannot find WindowInfo named : %s", windowName));
			}
			return info;
		}


	}

}