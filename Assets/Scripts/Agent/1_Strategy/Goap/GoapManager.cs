
using System.Collections;
using System.Collections.Generic;
using Lite.Goap;


namespace Lite.Strategy
{

	public class GoapManager
	{
		private Agent owner;
		private GoapMap map;
		private GoapAStarPlanner planner;
		private List<GoapGoal> goalList;
		private GoapGoal currentGoal;

		public GoapManager(Agent agent)
		{
			owner = agent;
			map = new GoapMap(GoapDefines.STATE_COUNT);
			map.BuildActionTable(agent);
			planner = new GoapAStarPlanner();
			planner.Setup(map);
			goalList = new List<GoapGoal>();
		}

		public void Update()
		{
			if (currentGoal != null)
			{
				currentGoal.Update();
				if (currentGoal.isAchived)
				{
					currentGoal.Deactive();
					goalList.Remove(currentGoal);
					currentGoal = null;
				}
			}
			else if (goalList.Count > 0)
			{
				GoapPlan plan = BuildPlan(goalList[0]);
				if (plan != null)
				{
					currentGoal = goalList[0];
					currentGoal.Active(plan);
				}
			}
		}

		public void AddGoal(GoapGoal goal)
		{
			if (ContainsGoal(goal.goalType))
				return;
			goalList.Add(goal);
		}

		public void RemoveGoal()
		{

		}

		public bool ContainsGoal(GoalType goalType)
		{
			for (int i = 0; i < goalList.Count; ++i)
			{
				if (goalList[i].goalType == goalType)
				{
					return true;
				}
			}
			return false;
		}

		public GoapPlan BuildPlan(GoapGoal goal)
		{
			GoapPlan plan = null;
			GoapAStarNode node = planner.Plan(owner.worldState, goal);
			if (node != null)
			{
				plan = new GoapPlan();
				while (node != null)
				{
					plan.AddAction(node.fromAction as GoapAgentAction);
					node = node.prev as GoapAStarNode;
				}
			}
			planner.Cleanup();
			return plan;
		}

	}


}