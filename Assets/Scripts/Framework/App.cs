
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace Lite
{
	public class App : SingletonMono<App>
	{
		private Dictionary<string, BaseManager> mManagerDic = new Dictionary<string, BaseManager>();

		private bool canTickManagers = false;


		public void Initialize()
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			Application.targetFrameRate = AppDefine.GameFrameRate;

			this.AddManager<MessageManager>();
			this.AddManager<ResourceManager>();
			this.AddManager<NetworkManager>();
			this.AddManager<UIManager>();
			this.AddManager<EntityManager>();
			

			foreach (var mgr in mManagerDic.Values)
				mgr.OnInit();
		}


		public void StartManagers()
		{
			foreach (var mgr in mManagerDic.Values)
			{
				mgr.OnStart();
			}
			canTickManagers = true;
		}


		void OnDestroy()
		{
			foreach (var mgr in mManagerDic.Values)
			{
				mgr.OnDestroy();
			}
		}


		void Update()
		{
			if (canTickManagers)
			{
				var itor = mManagerDic.GetEnumerator();
				while (itor.MoveNext())
					itor.Current.Value.OnTick();
			}
		}


		private T AddManager<T>() where T : BaseManager, new()
		{
			T mgr = null;
			string name = typeof(T).ToString();
			if (!mManagerDic.ContainsKey(name))
			{
				mgr = new T();
				mManagerDic.Add(name, mgr);
			}
			return mgr;
		}


		public T GetManager<T>() where T : BaseManager
		{
			string name = typeof(T).ToString();
			BaseManager mgr = null;
			mManagerDic.TryGetValue(name, out mgr);
			return mgr as T;
		}

		// for quick access
		public static MessageManager msgManager { get { return App.Instance.GetManager<MessageManager>(); } }
		public static ResourceManager resManager { get { return App.Instance.GetManager<ResourceManager>(); } }
		public static UIManager uiManager { get { return App.Instance.GetManager<UIManager>(); } }
		public static NetworkManager networkManager { get { return App.Instance.GetManager<NetworkManager>(); } }
		public static EntityManager entityManager { get { return App.Instance.GetManager<EntityManager>(); } }

	}
}