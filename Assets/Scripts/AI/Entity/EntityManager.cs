
using System.Collections.Generic;
using UnityEngine;


namespace Lite
{


	public class EntityManager : MonoBehaviour, IManager
	{
		public static EntityManager Instance;

		private Dictionary<int, GameEntity> GameEntityMap = new Dictionary<int, GameEntity>();

		private List<GameEntity> GameEntityList = new List<GameEntity>();


		private int idCreator = 9000;

		private int pawnUidCreator = 2000;


		public void Init()
		{
			Cleanup();
		}



		public void Tick()
		{
			for (int i = 0; i < GameEntityList.Count; ++i)
			{
				GameEntityList[i].Tick();
			}

			for (int i = 0; i < GameEntityList.Count; ++i)
			{
				GameEntity controller = GameEntityList[i];
				if (controller.ShouldBeDeleted)
				{
					GameEntityList.RemoveAt(i);
					--i;
					RemoveActor(controller.Uid);
				}
			}
		}


		public void Destroy()
		{
			Cleanup();
		}


		public void Cleanup()
		{
			for (int i = 0; i < GameEntityList.Count; ++i)
			{
				GameEntityList[i].Destroy();
			}
			GameEntityMap.Clear();
			GameEntityList.Clear();
		}


		public void AddActor(GameEntity controller)
		{
			if (!GameEntityMap.ContainsKey(controller.Uid))
			{
				GameEntityMap.Add(controller.Uid, controller);
				GameEntityList.Add(controller);
			}
			else
			{
				Log.Error("ActorManager.AddActor : repeated !");
			}
		}


		public virtual void RemoveActor(int uid)
		{
			var controller = FindActor(uid);
			if (controller != null)
			{
				controller.Destroy();
				GameEntityMap.Remove(uid);
				GameEntityList.Remove(controller);
			}
			else
			{
				Log.Error("ActorManager.RemoveActor : not exist !");
			}
		}


		public GameEntity FindActor(int uid)
		{
			GameEntity controller = null;
			GameEntityMap.TryGetValue(uid, out controller);
			return controller;
		}

		public int GetActorCount()
		{
			return GameEntityList.Count;
		}

		public GameEntity GetActorByIndex(int index)
		{
			return GameEntityList[index];
		}


		public int GenerateUid()
		{
			return idCreator++;
		}

		public int GeneratePawnUid()
		{
			return pawnUidCreator++;
		}

	}

}
