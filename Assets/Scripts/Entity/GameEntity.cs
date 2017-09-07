
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Lite
{
	
	public class GameEntity
	{
		public int Uid = -1;

		private Dictionary<Type, IComponent> components = new Dictionary<Type, IComponent>();
		private Dictionary<int, List<IComponent>> eventComponents = new Dictionary<int, List<IComponent>>();

		public GameObject gameObject { get; private set; }
		public ComTransform transform { get; private set; }
		public ComBlackboard blackboard { get; private set; }

		public FollowBoard followBoard;

		public bool ShouldBeDeleted = false;



		public GameEntity()
		{
			transform = AddComponent<ComTransform>();
			blackboard = AddComponent<ComBlackboard>();
		}


		public void SetActorInfo(int uid)
		{
			Uid = uid;
		}

		/// <summary>
		/// Invoked when instantiated
		/// </summary>
		public void Init()
		{
			foreach (var item in components)
				item.Value._init(this);
			OnInit();
		}

		/// <summary>
		/// Invoked when instantiated and reused
		/// </summary>
		public void Enable()
		{
			ShouldBeDeleted = false;

			foreach (var item in components)
				item.Value._enable();

			BroadcastToComponents(MsgConst.Actor_Spawn);

			OnEnable();
		}

		/// <summary>
		/// Invoked when deleted from Actormanager
		/// </summary>
		public void Disable()
		{
			foreach (var item in components)
				item.Value._disable();

			OnDisable();
		}

		/// <summary>
		/// Invoked when really deleted
		/// </summary>
		public void Destroy()
		{
			foreach (var item in components)
				item.Value._destroy();
			components.Clear();

			OnDestroy();
		}


		public void RenderTick()
		{
			foreach (var item in components)
				item.Value._renderTick();
			OnRenderTick();
		}


		public void FrameTick()
		{
			foreach (var item in components)
				item.Value._frameTick();
			OnFrameTick();
		}


		protected virtual void OnInit() { }
		protected virtual void OnEnable() { }
		protected virtual void OnDisable() { }
		protected virtual void OnRenderTick() { }
		protected virtual void OnFrameTick() { }
		protected virtual void OnDestroy() { }


		/// <summary>
		/// 添加组件。注意组件的先后依赖顺序。
		/// </summary>
		public T AddComponent<T>() where T : IComponent, new()
		{
			IComponent com;
			if (!components.TryGetValue(typeof(T), out com))
			{
				com = new T();
				components.Add(typeof(T), com);
			}
			return com as T;
		}


		public T GetComponent<T>() where T : IComponent
		{
			IComponent com;
			components.TryGetValue(typeof(T), out com);
			return com as T;
		}


		public IComponent GetCom(string type)
		{
			Type t = Type.GetType(type);
			IComponent com;
			components.TryGetValue(t, out com);
			return com;
		}

		public bool HasComponent<T>() where T : IComponent
		{
			return components.ContainsKey(typeof(T));
		}

		// internal function
		public void _AddListenerComponent(int evt, IComponent com)
		{
			List<IComponent> coms;
			if (!eventComponents.TryGetValue(evt, out coms))
			{
				coms = new List<IComponent>();
				eventComponents.Add(evt, coms);
			}
			if (!coms.Contains(com))
				coms.Add(com);
		}

		// internal function
		public void _RemoveListenerComponent(int evt, IComponent com)
		{
			List<IComponent> coms;
			if (eventComponents.TryGetValue(evt, out coms))
			{
				if (coms.Contains(com))
					coms.Remove(com);
			}
		}

		/// <summary>
		/// 向关心eventId的组件广播事件。会立即执行。
		/// 合理安排参数位置和顺序，一旦确定就不能轻易变动。
		/// </summary>
		public void BroadcastToComponents(int eventId, int arg1 = 0, int arg2 = 0, int arg3 = 0, int arg4 = 0, int arg5 = 0, int arg6 = 0)
		{
			List<IComponent> coms;
			if (eventComponents.TryGetValue(eventId, out coms))
				foreach (var item in coms)
					item._handleEvent(eventId, arg1, arg2, arg3, arg4, arg5, arg6);
		}

		public void DrawComponentsGizmos()
		{
			foreach (var item in components)
				item.Value.OnDrawGizmos();
		}


		public void MoveTowards(Int3 direction)
		{
			blackboard.isMoving = true;

			BroadcastToComponents(MsgConst.Actor_ManualMove);
			OnBeginMove();
		}


		public void MoveTo(Int3 position)
		{
			blackboard.isMoving = true;
			var mov = GetComponent<ComMoving>();
			mov.MoveTo(position, 0);

			OnBeginMove();
		}

		public void ChargeTo(Int3 position, int speed)
		{
			blackboard.isMoving = true;
			var mov = GetComponent<ComMoving>();
			mov.MoveTo(position, speed);
		}

		public void TurnTo(Int3 direction)
		{
			
		}

		public void StopMove()
		{
			if (blackboard.isMoving)
			{
				blackboard.isMoving = false;
				OnStopMove();
			}
		}


		public virtual void OnBeginMove()
		{
			float speed = 1;
			BroadcastToComponents(MsgConst.Actor_MoveBegin, (int)(speed * 1000));
		}

		public virtual void OnStopMove()
		{
			BroadcastToComponents(MsgConst.Actor_MoveStop);
		}

		public virtual void OnHit(GameEntity hitter)
		{
			BroadcastToComponents(MsgConst.Actor_Hit, hitter.Uid);
		}

		public virtual void OnDie(GameEntity killer)
		{
			StopMove();
			BroadcastToComponents(MsgConst.Actor_Die, killer == null ? 0 : killer.Uid);
		}

		public virtual void OnKill(GameEntity victim)
		{
			BroadcastToComponents(MsgConst.Actor_Kill, victim.Uid);
		}

		public virtual void OnBeginAttack(GameEntity target)
		{
			int atkIndex = GetComponent<ComAbility>().currentAttackIndex + 1;
			float speed = blackboard.attackSpeed;
			BroadcastToComponents(MsgConst.Actor_AttackBegin, atkIndex, 1, (int)(speed * 1000));
		}

		public virtual void OnEndAttack()
		{
			BroadcastToComponents(MsgConst.Actor_AttackEnd);
		}



	}


}
