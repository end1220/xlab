
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace Lite
{
	public class App : SingletonMono<App>
	{
		private Dictionary<string, IManager> mManagerDic = new Dictionary<string, IManager>();


		public void Initialize()
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			Application.targetFrameRate = AppDefine.GameFrameRate;

			this.AddManager<ResourceManager>();
			this.AddManager<NetworkManager>();
			this.AddManager<EntityManager>();
			

			foreach (var mgr in mManagerDic.Values)
				mgr.Init();
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
			var itor = mManagerDic.GetEnumerator();
			while (itor.MoveNext())
				itor.Current.Value.Tick();
		}


		private T AddManager<T>() where T : MonoBehaviour, IManager, new()
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


		public T GetManager<T>() where T : MonoBehaviour, IManager
		{
			string name = typeof(T).ToString();
			IManager mgr = null;
			mManagerDic.TryGetValue(name, out mgr);
			return mgr as T;
		}

		// for quick access
		public static ResourceManager resManager { get { return App.Instance.GetManager<ResourceManager>(); } }
		public static NetworkManager networkManager { get { return App.Instance.GetManager<NetworkManager>(); } }
		public static EntityManager entityManager { get { return App.Instance.GetManager<EntityManager>(); } }

	}
}