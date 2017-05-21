
using Lite.Goap;


namespace Lite.Strategy
{
	public abstract class GoapGoal : GoapAStarGoal
	{
		public GoalType goalType;
		protected WorldState goalState;
		private GoapPlan plan;
		public bool isAchived { private set; get; }

		public GoapGoal(GoalType gtp)
		{
			goalType = gtp;
			plan = null;
			goalState = new WorldState(GoapDefines.STATE_COUNT);
			OnInit();
		}

		public override bool IsSatisfied(WorldState state)
		{
			return state.Contains(goalState);
		}

		public override void MergeToNodeGoalState(WorldState nodeGoalState)
		{
			nodeGoalState.Merge(this.goalState);
		}

		protected abstract void OnInit();

		public virtual void Active(GoapPlan plan)
		{
			this.plan = plan;
		}

		public virtual void Deactive()
		{

		}

		public void Update()
		{
			if (plan != null)
			{
				plan.Excute();
				if (plan.isFinished)
				{
					isAchived = true;
				}
			}
		}

	}

}
