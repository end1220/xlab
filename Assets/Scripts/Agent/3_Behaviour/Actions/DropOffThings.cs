using UnityEngine;


namespace Lite.Bev
{

	public class DropOffThings : AgentAction
	{
		public Vector3 target;

		public DropOffThings()
		{
			actionType = ActionType.DropOffThings;
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