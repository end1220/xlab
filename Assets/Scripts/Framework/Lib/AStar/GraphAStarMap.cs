
using System;
using System.Collections;
using System.Collections.Generic;

using Lite.Graph;


namespace Lite.AStar
{
	public class GraphAStarMap : AStarMap
	{
		public override int GetNeighbourNodeCount(AStarNode node)
		{
			List<GraphEdge> edgeList = GetEdgeList(node.id);
			return edgeList != null ? edgeList.Count : 0;
		}

		public override AStarNode GetNeighbourNode(AStarNode node, int index)
		{
			List<GraphEdge> edgeList = GetEdgeList(node.id);
			return GetNodeByID(edgeList[index].to) as AStarNode;
		}

		public GraphAStarNode GetNodeAt(int posx, int posy)
		{
			foreach (var nd in nodeTable.Values)
			{
				GraphAStarNode node = nd as GraphAStarNode;
				if (node.x == posx && node.y == posy)
					return node;
			}
			return null;
		}

	}
}

