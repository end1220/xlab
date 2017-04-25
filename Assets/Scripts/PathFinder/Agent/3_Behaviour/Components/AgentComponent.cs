
using System.Collections.Generic;
using UnityEngine;


namespace Lite.Bev
{

	public class AgentComponent : IComponent
	{
		public Agent agent;

		private Queue<AgentAction> actionQueue = new Queue<AgentAction>();

		private AgentAction currentAction;


		public override void OnStart()
		{
			
		}

		public override void OnUpdate()
		{
			ProcessActions();
		}

		public void PushAction(AgentAction action)
		{
			actionQueue.Enqueue(action);
		}

		private void ProcessActions()
		{
			if (currentAction == null || currentAction.IsFinished())
			{
				if (actionQueue.Count > 0)
				{
					AgentAction action = actionQueue.Dequeue();
					if (currentAction != null)
						currentAction.Deactive(agent);
					currentAction = action;
					currentAction.Active(agent);
				}
				else
				{
					currentAction = null;
				}
			}

			if (currentAction != null)
			{
				currentAction.OnProcess(agent);
			}
		}

	}

}