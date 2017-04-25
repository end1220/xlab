

namespace Lite.Goap
{
	public abstract class GoapAction : Lite.Graph.GraphEdge
	{
		public WorldState preconditons;
		public WorldState effects;
		public int cost;

		public GoapAction(int maxStateCount):
			base(0,0,0)
		{
			preconditons = new WorldState(maxStateCount);
			effects = new WorldState(maxStateCount);
			cost = 0;
			OnSetupPreconditons();
			OnSetupEffects();
		}

		protected abstract void OnSetupPreconditons();
		protected abstract void OnSetupEffects();

	}

}