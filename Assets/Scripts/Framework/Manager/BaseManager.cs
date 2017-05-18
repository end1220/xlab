using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lite
{

	public class BaseManager : IManager, IMessageListener
	{

		public virtual void OnInit()
		{
			//throw new System.NotImplementedException();
		}

		public virtual void OnDestroy()
		{
			//throw new System.NotImplementedException();
		}

		public virtual void OnStart()
		{
			//throw new System.NotImplementedException();
		}

		public virtual void OnTick()
		{
			//throw new System.NotImplementedException();
		}

		public virtual void OnMessage(Message msg)
		{
			//throw new System.NotImplementedException();
		}

	}

}