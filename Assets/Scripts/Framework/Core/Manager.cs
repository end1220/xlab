

namespace Lite
{
	public abstract class Manager : IListener
	{
		public virtual void Initialize() { }

		public virtual void Destroy() { }

		public virtual void Start() { }

		public virtual void Update() { }

		public void OnMessage(Message evnt)
		{
		}

	}

}