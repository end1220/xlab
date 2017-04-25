
using Lite.AStar;


namespace Lite.Goap
{

	public class GoapAStarPlanner : AStarPathPlanner
	{
		GoapAStarGoal goal;

		public GoapAStarNode Plan(WorldState from, GoapAStarGoal to)
		{
			goal = to;

			GoapAStarMap goapMap = map as GoapAStarMap;
			AStarNode startNode = goapMap.CreateGoapNode();
			((GoapAStarNode)startNode).fromAction = null;
			((GoapAStarNode)startNode).currentState.Copy(from);
			to.MergeToNodeGoalState(((GoapAStarNode)startNode).goalState);

			AStarNode endNode = DoAStar(startNode);

			// build action list.
			endNode = ReverseNodeList(endNode) as GoapAStarNode;
			if (endNode != null)
				endNode = endNode.prev;

			return endNode as GoapAStarNode;
		}

		private AStarNode ReverseNodeList(AStarNode head)
		{
			AStarNode node = head;
			AStarNode prevNode = null;
			while (node != null)
			{
				AStarNode tmpNode = node;
				node = node.prev;
				tmpNode.prev = prevNode;
				prevNode = tmpNode;
			}
			return prevNode;
		}

		protected override bool CheckArrived(AStarNode node)
		{
			GoapAStarNode goapNode = node as GoapAStarNode;
			return goal.IsSatisfied(goapNode.currentState);
		}

		protected override int CalCostG(AStarNode prevNode, AStarNode currentNode)
		{
			return prevNode.g + ((GoapAStarNode)currentNode).fromAction.cost;
		}

		protected override int CalCostH(AStarNode node)
		{
			return ((GoapAStarNode)node).goalState.CountDifference(((GoapAStarNode)node).currentState);
		}

	}

}