

using Lite.Cmd;


namespace Lite.Strategy
{
	public abstract class GoapAgentAction : Lite.Goap.GoapAction
	{
		public int actionType;

		protected Agent owner;

		private bool isFinished = false;

		private bool isFailed = false;

		public GoapAgentAction(Agent agent) :
			base(GoapDefines.STATE_COUNT)
		{
			owner = agent;
		}

		public void ApplyEffects()
		{
			owner.worldState.Merge(this.effects);
		}

		public virtual void OnActive()
		{
		}

		public virtual void OnDeactive()
		{
		}

		public void SetFinished()
		{
			isFinished = true;
			ApplyEffects();
		}

		public bool IsFinished()
		{
			return isFinished;
		}

		public virtual void Update() { }

	}

}