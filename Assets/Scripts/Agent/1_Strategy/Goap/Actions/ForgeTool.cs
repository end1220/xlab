

using Lite.Goap;

namespace Lite.Strategy
{

	public class ForgeTool : GoapAgentAction
	{
		public ForgeTool(Agent agent) :
			base(agent)
		{
			actionType = (int)ActionType.ForgeTool;
			cost = 1;
		}

		protected override void OnSetupPreconditons()
		{
			preconditons.Set((int)WorldStateType.HasOre, true);
		}

		protected override void OnSetupEffects()
		{
			effects.Set((int)WorldStateType.HasNewTools, true);
		}

	}

}