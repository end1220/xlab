
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace Lite
{
	public class App : SingletonMono<App>
	{
		private Dictionary<string, Manager> mManagerDic = new Dictionary<string, Manager>();

		private bool canUpdate = false;

		// for quick access
		public static EventManager		eventManager = null;
		public static ResourceManager	resManager = null;
		public static UIManager			uiManager = null;
		public static NetworkManager	networkManager = null;
		public static EntityManager entityManager = null;


		public void Initialize()
		{
			eventManager = this.AddManager<EventManager>();
			resManager = this.AddManager<ResourceManager>();
			networkManager = this.AddManager<NetworkManager>();
			uiManager = this.AddManager<UIManager>();
			entityManager = this.AddManager<EntityManager>();
			

			foreach (var mgr in mManagerDic.Values)
				mgr.Initialize();

			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			Application.targetFrameRate = AppDefine.GameFrameRate;
		}

		public void StartManagers()
		{
			IDictionaryEnumerator itor = mManagerDic.GetEnumerator();
			while (itor.MoveNext())
			{
				Manager mgr = (Manager)(itor.Entry.Value);
				mgr.Start();
			}
			canUpdate = true;
		}

		void OnDestroy()
		{
			foreach (var mgr in mManagerDic.Values)
			{
				mgr.Destroy();
			}
		}

		void Update()
		{
			if (!canUpdate)
				return;

			IDictionaryEnumerator itor = mManagerDic.GetEnumerator();
			while (itor.MoveNext())
			{
				Manager mgr = (Manager)(itor.Entry.Value);
				mgr.Update();
			}
		}

		private T AddManager<T>() where T : Manager, new()
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


		/*public T GetManager<T>() where T : Manager
		{
			string name = typeof(T).ToString();
			Manager mgr = null;
			mManagerDic.TryGetValue(name, out mgr);
			return mgr as T;
		}*/

	}
}