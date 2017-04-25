

using System;
using UnityEngine;


namespace Lite
{

	public class ResourceManager : Manager
	{
		public override void Initialize()
		{

		}

		public override void Destroy()
		{

		}
		public GameObject LoadRes(string pathName)
		{
			var go = Resources.Load(pathName) as GameObject;
			return go;
		}

	}

}