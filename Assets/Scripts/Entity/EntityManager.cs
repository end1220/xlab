
using System.Collections.Generic;
using UnityEngine;



namespace Lite
{

	public class EntityManager : MonoBehaviour, IManager
	{
		public static EntityManager Instance { private set; get; }

		private Dictionary<int, GameEntity> actorControllerMap = new Dictionary<int, GameEntity>();

		private List<GameEntity> actorControllerList = new List<GameEntity>();

		// player controller wont be destroyed in game scene.
		private List<GameEntity> playerList = new List<GameEntity>();

		public GameEntity SelfController = null;

		private int idCreator = 9000;

		private int pawnUidCreator = 2000;


		public void Init()
		{
			Cleanup();
		}


		public void Tick()
		{
			for (int i = 0; i < actorControllerList.Count; ++i)
			{
				actorControllerList[i].FrameTick();
			}

			for (int i = 0; i < actorControllerList.Count; ++i)
			{
				GameEntity controller = actorControllerList[i];
				if (controller.ShouldBeDeleted)
				{
					actorControllerList.RemoveAt(i);
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
			for (int i = 0; i < actorControllerList.Count; ++i)
			{
				actorControllerList[i].Destroy();
			}
			actorControllerMap.Clear();
			actorControllerList.Clear();
			playerList.Clear();
			SelfController = null;
		}


		public void AddActor(GameEntity controller)
		{
			if (!actorControllerMap.ContainsKey(controller.Uid))
			{
				actorControllerMap.Add(controller.Uid, controller);
				actorControllerList.Add(controller);
			}
			else
			{
				Log.Error("ActorManager.AddActor : repeated ! Uid " + controller.Uid);
			}
		}


		public virtual void RemoveActor(int uid)
		{
			var controller = FindActor(uid);
			if (controller != null)
			{
				controller.Disable();
				actorControllerMap.Remove(uid);
				actorControllerList.Remove(controller);
			}
			else
			{
				Log.Error("ActorManager.RemoveActor : not exist !");
			}
		}


		public GameEntity FindActor(int uid)
		{
			GameEntity controller = null;
			actorControllerMap.TryGetValue(uid, out controller);
			return controller;
		}

		public int GetActorCount()
		{
			return actorControllerList.Count;
		}

		public GameEntity GetActorByIndex(int index)
		{
			return actorControllerList[index];
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
