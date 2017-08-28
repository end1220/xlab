
using System;
using System.Collections.Generic;


namespace Lite
{
	public abstract class GameEntity
	{
		public int Uid = -1;

		public bool ShouldBeDeleted;

		private Dictionary<Type, IComponent> components = new Dictionary<Type, IComponent>();


		public void Tick()
		{
			try
			{
				OnTick();

				foreach (var com in components.Values)
					com.Tick();
			}
			catch (System.Exception e)
			{
				Log.Error(e.ToString());
			}
		}


		public void Destroy()
		{
			foreach (var com in components.Values)
			{
				com.Destroy();
			}
			components.Clear();

			OnDestroy();
		}


		public T AddComponent<T>() where T : IComponent, new()
		{
			if (!components.ContainsKey(typeof(T)))
			{
				T com = new T();
				components.Add(typeof(T), com);
				com.Init(this);
				return com;
			}
			else
				return components[typeof(T)] as T;
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


		public virtual void OnInit()
		{
			
		}


		protected virtual void OnRenderTick()
		{

		}


		protected virtual void OnTick()
		{

		}


		protected virtual void OnDestroy()
		{

		}


		

	}

}
