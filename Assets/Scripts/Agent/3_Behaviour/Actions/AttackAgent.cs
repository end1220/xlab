using UnityEngine;


namespace Lite.Bev
{
	public class AttackAgent : AgentAction
	{
		public Agent targetAgent;

		public AttackAgent()
		{
			actionType = ActionType.Attack;
		}

		public override void OnActive(Agent agent)
		{
			Log.Info("enter attack");
		}

		public override void OnDeactive(Agent agent)
		{

		}

		public override void OnProcess(Agent agent)
		{
			if (targetAgent != null)
			{
				Vector3 faceDir = agent.locomotion.forward;
				Vector3 desiredDir = targetAgent.locomotion.position - agent.locomotion.position;
				if (Vector3.Angle(faceDir, desiredDir) > 10)
				{
					float deltaTime = GameTimer.deltaTime;
					Vector3 dir = Vector3.Slerp(faceDir, desiredDir, 5 * deltaTime);
					agent.locomotion.SetForward(dir);
				}
				else
				{
					if (!agent.animComponent.attack1)
					{
						agent.animComponent.attack1 = true;
						Log.Info("play attack");
					}
				}
			}
			else
			{
				this.Finish();
			}
		}

	}

}