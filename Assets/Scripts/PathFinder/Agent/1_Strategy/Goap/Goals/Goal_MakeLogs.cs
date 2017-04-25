
using Lite.Goap;
using Lite.Common;


namespace Lite.Strategy
{

	public class Goal_MakeLogs : GoapGoal
	{
		public Goal_MakeLogs() :
			base(GoalType.MakeLogs) 
		{
		}

		protected override void OnInit()
		{
			// set goal state.
			goalState.Set((int)WorldStateType.CollectLogs, true);
		}

		public override void Active(GoapPlan plan)
		{
			base.Active(plan);
			Log.Info("active MakeLogs");
		}

		public override void Deactive()
		{
			Log.Info("deactive MakeLogs");
		}

	}


}