

using Lite.Goap;

namespace Lite.Strategy
{

	public class DropOffOre : GoapAgentAction
	{
		public DropOffOre(Agent agent) :
			base(agent)
		{
			actionType = (int)ActionType.DropOffOre;
			cost = 1;
		}

		protected override void OnSetupPreconditons()
		{
			preconditons.Set((int)WorldStateType.HasOre, true);
		}

		protected override void OnSetupEffects()
		{
			effects.Set((int)WorldStateType.HasOre, false);
			effects.Set((int)WorldStateType.CollectOre, true);
		}

	}

}