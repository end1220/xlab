
using Lite.Goap;
using Lite.Common;


namespace Lite.Strategy
{

	public class Goal_MakeOre : GoapGoal
	{
		public Goal_MakeOre() :
			base(GoalType.MakeOre) 
		{
		}

		protected override void OnInit()
		{
			// set goal state.
			goalState.Set((int)WorldStateType.CollectOre, true);
		}

		public override void Active(GoapPlan plan)
		{
			base.Active(plan);
			Log.Info("active MakeOre");
		}

		public override void Deactive()
		{
			Log.Info("deactive MakeOre");
		}

	}


}