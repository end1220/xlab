
using Lite.Goap;
using Lite.Common;


namespace Lite.Strategy
{

	public class Goal_MakeFirewood : GoapGoal
	{
		public Goal_MakeFirewood():
			base(GoalType.MakeFirewood) 
		{
		}

		protected override void OnInit()
		{
			// set goal state.
			goalState.Set((int)WorldStateType.CollectFirewood, true);
		}

		public override void Active(GoapPlan plan)
		{
			base.Active(plan);
			Log.Info("active MakeFirewood");
		}

		public override void Deactive()
		{
			Log.Info("deactive MakeFirewood");
		}

	}


}