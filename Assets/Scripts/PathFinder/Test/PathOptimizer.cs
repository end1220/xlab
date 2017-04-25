
using System;
using System.Collections.Generic;
using UnityEngine;
using Lite.AStar;


namespace TwGame
{

	public static class PathOptimizer
	{
		public static GridAStarMap map;
		// 合并共线点
		public static Point2D[] CombinePoints(Point2D[] olds)
		{
			if (olds == null || olds.Length < 3)
				return olds;

			List<Point2D> optimalPoints = new List<Point2D>();
			optimalPoints.Add(olds[0]);

			Point2D ptA = olds[0];
			Point2D ptB = olds[1];
			Point2D ptC;

			for (int i = 2; i < olds.Length; ++i)
			{
				ptC = olds[i];
				if (ptB.x - ptA.x != ptC.x - ptB.x || ptB.y - ptA.y != ptC.y - ptB.y)
				{
					optimalPoints.Add(ptB);
				}

				ptA = ptB;
				ptB = ptC;

				if (i == olds.Length - 1)
					optimalPoints.Add(olds[i]);
			}

			return optimalPoints.ToArray();
		}


		// smooth path
		public static Point2D[] SmoothPath(Point2D[] olds)
		{
			if (olds == null || olds.Length < 3)
				return olds;

			List<Point2D> optimalPoints = new List<Point2D>();
			optimalPoints.Add(olds[0]);

			int checkIndex = 0;
			for (int i = 1; i < olds.Length; ++i)
			{
				bool ret = IsPassableBetween(olds[checkIndex], olds[i]);
				if (!ret)
				{
					checkIndex = i - 1;
					optimalPoints.Add(olds[i - 1]);
				}

				if (i == olds.Length - 1)
					optimalPoints.Add(olds[i]);
			}

			return optimalPoints.ToArray();
		}


		public static bool IsPassableBetween(Point2D from, Point2D to)
		{
			// y = a*x + b
			float a = 0;
			int dx = to.x - from.x;
			int dy = to.y - from.y;
			if (Math.Abs(dx) > Math.Abs(dy))
			{
				a = (float)dy / dx;
				if (from.x > to.x)
				{
					Point2D tmp = from;
					from = to;
					to = tmp;
				}
				for (int x = from.x + 1; x < to.x + 1; x++)
				{
					float cx = (float)x - 0.5f;
					float y = from.y + a * (cx - from.x);
					int neary = y % 1 > 0.5f ? (int)(y + 0.5f) : (int)y;

					if (Mathf.Approximately(neary, y))
					{
						if (!IsPassable(x, neary))
							return false;
					}
					else if (neary > y)
					{
						if (!IsPassable(x - 1, neary))
							return false;
						if (!IsPassable(x - 1, neary - 1))
							return false;
						if (!IsPassable(x, neary))
							return false;
						if (!IsPassable(x, neary - 1))
							return false;
					}
					else
					{
						if (!IsPassable(x - 1, neary + 1))
							return false;
						if (!IsPassable(x - 1, neary))
							return false;
						if (!IsPassable(x, neary + 1))
							return false;
						if (!IsPassable(x, neary))
							return false;
					}
				}
			}
			else
			{
				a = (float)dx / dy;
				if (from.y > to.y)
				{
					Point2D tmp = from;
					from = to;
					to = tmp;
				}
				for (int y = from.y + 1; y < to.y + 1; y++)
				{
					float cy = (float)y - 0.5f;
					float x = from.x + a * (cy - from.y);
					int nearx = x % 1 > 0.5f ? (int)(x + 0.5f) : (int)x;

					if (Mathf.Approximately(nearx, x))
					{
						if (!IsPassable(nearx, y))
							return false;
					}
					else if (nearx > x)
					{
						if (!IsPassable(nearx, y - 1))
							return false;
						if (!IsPassable(nearx - 1, y - 1))
							return false;
						if (!IsPassable(nearx, y))
							return false;
						if (!IsPassable(nearx - 1, y))
							return false;
					}
					else
					{
						if (!IsPassable(nearx + 1, y - 1))
							return false;
						if (!IsPassable(nearx, y - 1))
							return false;
						if (!IsPassable(nearx + 1, y))
							return false;
						if (!IsPassable(nearx, y))
							return false;
					}
				}
			}

			return true;
		}


		private static bool IsPassable(int x, int y)
		{
			return map.IsNodePassable(x,y);// BattleMap.Instance.IsPassable(x, y);
		}

	}

}
