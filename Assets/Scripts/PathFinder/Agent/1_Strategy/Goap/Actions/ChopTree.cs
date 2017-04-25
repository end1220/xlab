

using Lite.Goap;

namespace Lite.Strategy
{
	public class ChopTree : GoapAgentAction
	{
		public ChopTree(Agent agent) : 
			base(agent)
		{
			actionType = (int)ActionType.ChopTree;
			cost = 1;
		}

		protected override void OnSetupPreconditons()
		{
			preconditons.Set((int)WorldStateType.HasTool, true);
			preconditons.Set((int)WorldStateType.HasLogs, false);
		}

		protected override void OnSetupEffects()
		{
			effects.Set((int)WorldStateType.HasLogs, true);
		}

	}

}