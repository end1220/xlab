

using Lite.Goap;

namespace Lite.Strategy
{
	public class PickUpLogs : GoapAgentAction
	{
		public PickUpLogs(Agent agent) :
			base(agent)
		{
			actionType = (int)ActionType.PickupLogs;
			cost = 2;
		}

		protected override void OnSetupPreconditons()
		{
			preconditons.Set((int)WorldStateType.HasLogs, false);
		}

		protected override void OnSetupEffects()
		{
			effects.Set((int)WorldStateType.HasLogs, true);
		}

	}

}