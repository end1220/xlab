using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lite
{
	public class EntityManager : BaseManager
	{
		private Dictionary<long, TestPlayer> mEntityMap;


		public EntityManager()
		{
			mEntityMap = new Dictionary<long, TestPlayer>();
		}

		public TestPlayer AddPlayer(long guid, float x, float y)
		{
			if (mEntityMap.ContainsKey(guid))
			{
				Log.Error("repeated player.");
				return mEntityMap[guid];
			}
			else
			{
				GameObject go = GameObject.Instantiate(Resources.Load("Cube")) as GameObject;
				TestPlayer player = go.GetComponent<TestPlayer>();
				player.transform.localPosition.Set(x, 1, y);
				mEntityMap.Add(guid, player);
				return player;
			}
			
		}


	}

}