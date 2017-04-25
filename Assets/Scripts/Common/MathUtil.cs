
using System;

using UnityEngine;


namespace Lite
{
	public class MathUtil
	{
		public const double PI = Math.PI;

		private static System.Random rnd = new System.Random(System.DateTime.Now.Millisecond);

		/// <summary>
		/// get a random integer in [min, max].
		/// </summary>
		/// <param name="min">min value</param>
		/// <param name="max">max value</param>
		/// <returns></returns>
		public static int RandInt(int min, int max)
		{
			return rnd.Next(min, max + 1);
		}

		/// <summary>
		/// get a random float in [0, 1).
		/// </summary>
		/// <returns></returns>
		public static float RandFloat()
		{
			return (rnd.Next(0, int.MaxValue)) / (int.MaxValue + 1.0f);
		}

		/// <summary>
		/// get a random float in (-1, 1).
		/// </summary>
		/// <returns></returns>
		public static float RandClamp()
		{
			return RandFloat() - RandFloat();
		}

		public static float Distance(Vector3 p1, Vector3 p2)
		{
			return Vector3.Distance(p1, p2);
		}

		public static float Distance2D(Vector3 p1, Vector3 p2)
		{
			p1.Set(p1.x, 0, p1.z);
			p2.Set(p2.x, 0, p2.z);
			return Vector3.Distance(p1, p2);
		}

		public static float DistanceSqr(Vector3 p1, Vector3 p2)
		{
			return (p1 - p2).sqrMagnitude;
		}

		public static float DistanceSqr2D(Vector3 p1, Vector3 p2)
		{
			p1.Set(p1.x, 0, p1.z);
			p2.Set(p2.x, 0, p2.z);
			return (p1 - p2).sqrMagnitude;
		}

	}

}