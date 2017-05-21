

using Lite.Goap;

namespace Lite.Strategy
{
	public class PickUpTool : GoapAgentAction
	{
		public PickUpTool(Agent agent) :
			base(agent)
		{
			actionType = (int)ActionType.PickupTools;
			cost = 1;
		}

		protected override void OnSetupPreconditons()
		{
			preconditons.Set((int)WorldStateType.HasTool, false);
		}

		protected override void OnSetupEffects()
		{
			effects.Set((int)WorldStateType.HasTool, true);
		}

	}

}