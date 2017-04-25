using UnityEngine;


namespace Lite.Bev
{

	public class Work : AgentAction
	{
		public Vector3 target;

		public Work()
		{
			actionType = ActionType.Work;
		}

		public override void OnActive(Agent agent)
		{
			agent.animComponent.moveSpeed = 0;
		}

		public override void OnDeactive(Agent agent)
		{

		}

		public override void OnProcess(Agent agent)
		{

		}

	}

}