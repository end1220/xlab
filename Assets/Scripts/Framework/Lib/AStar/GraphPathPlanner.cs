using System;
using System.Collections;
using System.Collections.Generic;


namespace Lite.AStar
{
	

	public class GraphPathPlanner : AStarPathPlanner
	{
		private int start;
		private int end;
		private GraphAStarNode startNode;
		private GraphAStarNode targetNode;

		public Point2D[] FindPath(int start, int end)
		{
			this.start = end;
			this.end = start;

			startNode = map.GetNodeByID(this.start) as GraphAStarNode;
			targetNode = map.GetNodeByID(this.end) as GraphAStarNode;

			GraphAStarNode endNode = DoAStar(startNode) as GraphAStarNode;

			// build path points.
			int pointCount = 0;
			GraphAStarNode pathNode = endNode;
			while (pathNode != null)
			{
				pointCount++;
				pathNode = pathNode.prev as GraphAStarNode;
			}
			Point2D[] pointArray = new Point2D[pointCount];
			pathNode = endNode;
			int index = 0;
			while (pathNode != null)
			{
				pointArray[index++] = new Point2D(pathNode.x, pathNode.y);
				pathNode = pathNode.prev as GraphAStarNode;
			}
			Cleanup();
			return pointArray;
		}

		protected override bool CheckArrived(AStarNode node)
		{
			return node.id == targetNode.id;
		}

		protected override int CalCostG(AStarNode prevNode, AStarNode currentNode)
		{
			return prevNode.g + map.GetEdge(prevNode.id, currentNode.id).cost;
		}

		protected override int CalCostH(AStarNode node)
		{
			int dx = Math.Abs(targetNode.x - ((GraphAStarNode)node).x);
			int dy = Math.Abs(targetNode.y - ((GraphAStarNode)node).y);
			int dist = (int)(dx > dy ? 1.4f * dy + (dx - dy) : 1.4f * dx + (dy - dx));
			return dist;
		}

	}


}