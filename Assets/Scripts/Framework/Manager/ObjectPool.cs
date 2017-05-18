
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lite
{
	public class ObjectPool : Singleton<ObjectPool>
	{

		public enum ObjectType
		{
			Min = 0,

			Role,
			Effect,
			UI,

			Max,
		}


		public class ObjectInfo
		{
			public ObjectType objectType;
		}


		public class PoolInfo
		{
			// use gameobject as key to prevent the case that the same object be put into pool repeatedly.
			public Dictionary<GameObject, ObjectInfo> objectsDic = new Dictionary<GameObject, ObjectInfo>();
		}


		// use name of resource as key.
		private Dictionary<string, PoolInfo> poolInfoDic = new Dictionary<string, PoolInfo>();

		private GameObject poolGameObjectRoot = null;


		public GameObject GetGO(string assetBundle, string resName)
		{
			PoolInfo poolInfo = null;
			if (!poolInfoDic.TryGetValue(resName, out poolInfo))
			{
				poolInfo = new PoolInfo();
				poolInfoDic.Add(resName, poolInfo);
			}

			GameObject go = null;
			ObjectInfo objectInfo = null;
			foreach(KeyValuePair<GameObject, ObjectInfo> pair in poolInfo.objectsDic)
			{
				go = pair.Key;
				objectInfo = pair.Value;
				break;
			}

			if (go == null)
			{
				var loadedObj = App.resManager.LoadAsset<GameObject>(assetBundle, resName);
				// instantiate one.
				go = GameObject.Instantiate(loadedObj) as GameObject;
			}
			else
			{
				poolInfo.objectsDic.Remove(go);
				OnGet(go, objectInfo);
			}
			
			return go;
		}

		public void PutGO(string resName, GameObject go, ObjectType objectType)
		{
			if (resName == null || resName == "" || go == null || objectType <= ObjectType.Min || objectType >= ObjectType.Max)
			{
				Debug.LogError("failed to call ObjectPool.PutGo: params are invalid.");
				return;
			}

			if (poolGameObjectRoot == null)
			{
				poolGameObjectRoot = new GameObject("PoolRoot");
				poolGameObjectRoot.transform.localPosition = new Vector3(0, -5000, 0);
			}

			PoolInfo poolInfo = null;
			if (!poolInfoDic.TryGetValue(resName, out poolInfo))
			{
				poolInfo = new PoolInfo();
				poolInfoDic.Add(resName, poolInfo);
			}

			ObjectInfo objectInfo = new ObjectInfo();
			objectInfo.objectType = objectType;

			if (poolInfo.objectsDic.ContainsKey(go))
			{
				poolInfo.objectsDic[go] = objectInfo;
			}
			else
			{
				poolInfo.objectsDic.Add(go, objectInfo);
			}

			OnPut(go, objectInfo);
		}

		private void OnGet(GameObject go, ObjectInfo objectInfo)
		{
			go.SetActive(true);
			go.transform.parent = null;

			ObjectType objectType = objectInfo.objectType;
			switch (objectType)
			{
				case ObjectType.Role:
					break;
				case ObjectType.Effect:
					break;
				case ObjectType.UI:
					break;
				default:
					break;
			}
		}

		private void OnPut(GameObject go, ObjectInfo objectInfo)
		{
			go.SetActive(false);
			go.transform.parent = poolGameObjectRoot.transform;

			ObjectType objectType = objectInfo.objectType;
			switch (objectType)
			{
				case ObjectType.Role:
					break;
				case ObjectType.Effect:
					break;
				case ObjectType.UI:
					break;
				default:
					break;
			}
		}

	}


}