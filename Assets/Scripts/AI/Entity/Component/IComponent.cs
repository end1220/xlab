

namespace Lite
{

	public abstract class IComponent
	{
		protected GameEntity entity;

		public void Init(GameEntity ent)
		{
			entity = ent;
			OnInit();
		}

		public void Destroy()
		{
			OnDestroy();
		}

		public void Tick()
		{
			OnTick();
		}


		protected virtual void OnInit() { }
		protected virtual void OnDestroy() { }
		protected virtual void OnTick() { }

	}

}
