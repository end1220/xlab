using System;
using System.Collections;
using System.Collections.Generic;


namespace Lite.AStar
{
	
	public class GridPathPlanner : AStarPathPlanner
	{
		private int startX;
		private int startY;
		private int endX;
		private int endY;
		private GridAStarNode startNode;
		private GridAStarNode targetNode;

		public Point2D[] FindPath(int startX, int startY, int endX, int endY)
		{
			this.startX = endX;
			this.startY = endY;
			this.endX = startX;
			this.endY = startY;

			GridAStarMap gridMap = (GridAStarMap)map;
			startNode = gridMap.GetNodeByIndex(this.startX, this.startY);
			targetNode = gridMap.GetNodeByIndex(this.endX, this.endY);

			GridAStarNode endNode = DoAStar(startNode) as GridAStarNode;

			// build path points.
			int pointCount = 0;
			GridAStarNode pathNode = endNode;
			while (pathNode != null)
			{
				pointCount++;
				pathNode = pathNode.prev as GridAStarNode;
			}
			Point2D[] pointArray = new Point2D[pointCount];
			pathNode = endNode;
			int index = 0;
			while (pathNode != null)
			{
				pointArray[index++] = new Point2D(pathNode.x, pathNode.y);
				pathNode = pathNode.prev as GridAStarNode;
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
			int dx = Math.Abs(((GridAStarNode)prevNode).x - ((GridAStarNode)currentNode).x);
			int dy = Math.Abs(((GridAStarNode)prevNode).y - ((GridAStarNode)currentNode).y);
			int dist = dx > dy ? 14 * dy + 10 * (dx - dy) : 14 * dx + 10 * (dy - dx);
			return prevNode.g + dist;
		}

		protected override int CalCostH(AStarNode node)
		{
			int dx = Math.Abs(endX - ((GridAStarNode)node).x);
			int dy = Math.Abs(endY - ((GridAStarNode)node).y);
			int dist = dx > dy ? 14 * dy + 10 * (dx - dy) : 14 * dx + 10 * (dy - dx);
			return dist;
		}

	}


}