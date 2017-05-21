
using System;
using System.Collections;
using System.Collections.Generic;

using Lite.AStar;


namespace Lite.Goap
{
	// worldstate is node, action is edge
	// 因为worldstate组合结果数目比较大，即node数目大，所以map不构建图结构，而是在运行时生成临时的节点。

	public abstract class GoapAStarMap : AStarMap
	{
		private int maxStateCount = 0;
		private List<GoapAction> actionTable = new List<GoapAction>();
		private List<GoapAction> neighbourEdgeList = new List<GoapAction>();
		private Queue<GoapAStarNode> nodePool = new Queue<GoapAStarNode>();

		private int nodeIdCounter = 0;

		public GoapAStarMap(int stateCount)
		{
			maxStateCount = stateCount;
		}

		public void AddAction(GoapAction action)
		{
			actionTable.Add(action);
		}

		public abstract void BuildActionTable(IAgent agent);

		public override int GetNeighbourNodeCount(AStarNode node)
		{
			neighbourEdgeList.Clear();
			GoapAStarNode goapNode = node as GoapAStarNode;
			for (int i = 0; i < actionTable.Count; ++i)
			{
				GoapAction action = actionTable[i];
				if (goapNode.currentState.Contains(action.preconditons))
					neighbourEdgeList.Add(action);
			}
			return neighbourEdgeList.Count;
		}

		public override AStarNode GetNeighbourNode(AStarNode node, int index)
		{
			GoapAStarNode goapNode = node as GoapAStarNode;
			GoapAction action = neighbourEdgeList[index];
			GoapAStarNode neighbour = CreateGoapNode();
			neighbour.fromAction = action;
			neighbour.currentState.Copy(goapNode.currentState);
			neighbour.currentState.Merge(action.effects);
			neighbour.goalState.Copy(goapNode.goalState);
			return neighbour;
		}

		public override void Cleanup()
		{
			nodeIdCounter = 0;
			neighbourEdgeList.Clear();
		}

		public override void RecycleNode(AStarNode node)
		{
			node.Reset();
			nodePool.Enqueue((GoapAStarNode)node);
		}

		public GoapAStarNode CreateGoapNode()
		{
			GoapAStarNode node = GetNodeFromPool();
			return node;
		}

		private GoapAStarNode GetNodeFromPool()
		{
			GoapAStarNode node;
			if (nodePool.Count > 0)
			{
				node = nodePool.Dequeue();
				node.Reset();
				Log.Info("Get Node From Pool...");
			}
			else
			{
				node = new GoapAStarNode(nodeIdCounter++, maxStateCount);
			}
			return node;
		}

	}
}

