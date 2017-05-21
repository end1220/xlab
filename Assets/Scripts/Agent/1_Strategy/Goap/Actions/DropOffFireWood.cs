

using Lite.Goap;

namespace Lite.Strategy
{

	public class DropOffFirewood : GoapAgentAction
	{
		public DropOffFirewood(Agent agent) :
			base(agent)
		{
			actionType = (int)ActionType.DropOffFirewood;
			cost = 1;
		}

		protected override void OnSetupPreconditons()
		{
			preconditons.Set((int)WorldStateType.HasFirewood, true);
		}

		protected override void OnSetupEffects()
		{
			effects.Set((int)WorldStateType.HasFirewood, false);
			effects.Set((int)WorldStateType.CollectFirewood, true);
		}

	}

}