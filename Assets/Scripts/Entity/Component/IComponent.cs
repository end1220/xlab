

namespace Lite
{
	/// <summary>
	/// 想利用组件思想避免继承体系的弊端。从这里开始吧！
	/// 分离Actor的独立逻辑模块，作为子类组件吧！
	/// </summary>
	public abstract class IComponent
	{
		protected GameEntity owner;

		// 要监听的事件id
		private int[] _listenEvents;

		public int Uid { get { return owner.Uid; } }
		// only invoked when created.
		public void _init(GameEntity ctrl)
		{
			owner = ctrl;
			OnInit();
		}

		public void _destroy()
		{
			OnDestroy();
		}

		// only invoked when created or reused.
		public void _enable()
		{
			if (_listenEvents != null && _listenEvents.Length > 0)
			{
				for (int i = 0; i < _listenEvents.Length; ++i)
					owner._AddListenerComponent(_listenEvents[i], this);
			}
			OnEnable();
		}

		// only invoked when destroyed.
		public void _disable()
		{
			if (_listenEvents != null && _listenEvents.Length > 0)
			{
				for (int i = 0; i < _listenEvents.Length; ++i)
					owner._RemoveListenerComponent(_listenEvents[i], this);
			}
			OnDisable();
		}

		public void _frameTick()
		{
			OnFrameTick();
		}

		public void _renderTick()
		{
			OnRenderTick();
		}

		/// <summary>
		/// Please invoke this in ComXXX.OnInit if ComXXX needs.
		/// Set the events ComXXX cares.
		/// </summary>
		protected void ListenEvents(params int[] events)
		{
			_listenEvents = events;
		}

		public void _handleEvent(int eventId, int arg1 = 0, int arg2 = 0, int arg3 = 0, int arg4 = 0, int arg5 = 0, int arg6 = 0)
		{
			OnEvent(eventId, arg1, arg2, arg3, arg4, arg5, arg6);
		}


		protected virtual void OnInit() { }
		protected virtual void OnDestroy() { }
		protected virtual void OnEnable() { }
		protected virtual void OnDisable() { }
		protected virtual void OnFrameTick() { }
		protected virtual void OnRenderTick() { }
		protected virtual void OnRecreated() { }
		protected virtual void OnEvent(int eventId, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6) { }
		public virtual void OnDrawGizmos() { }
	}

}