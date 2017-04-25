using UnityEngine;


namespace Lite.Bev
{

	public class MoveToPosition : AgentAction
	{
		public Vector3 targetPosition;
		public MoveSpeed speedType;

		public MoveToPosition()
		{
			actionType = ActionType.MoveTo;
		}

		public override void OnActive(Agent agent)
		{
			agent.locomotion.StartMove(targetPosition, GetSpeed(agent, speedType));
			agent.animComponent.moveSpeed = GetClampSpeed(speedType);
			Log.Info("enter run");
		}

		public override void OnDeactive(Agent agent)
		{

		}

		public override void OnProcess(Agent agent)
		{
			float deltaTime = GameTimer.deltaTime;
			float distanceSqr = MathUtil.DistanceSqr2D(targetPosition, agent.locomotion.position);
			float speed = GetSpeed(agent, speedType);
			if (distanceSqr < speed * speed * deltaTime)
			{
				agent.animComponent.moveSpeed = 0;
				Finish();
				Log.Info("arrived");
				this.Finish();
			}
			else
			{
				if (agent.animComponent.moveSpeed < 0.00001f)
				{
					agent.animComponent.moveSpeed = GetClampSpeed(speedType);
					Log.Info("replay");
				}
			}
		}

		private float GetClampSpeed(Bev.MoveSpeed speed)
		{
			float name;
			switch (speed)
			{
				case Bev.MoveSpeed.Slow:
					name = 0.33f;
					break;
				case Bev.MoveSpeed.Normal:
					name = 0.66f;
					break;
				case Bev.MoveSpeed.Fast:
					name = 1f;
					break;
				default:
					name = 0.66f;
					break;
			}
			return name;
		}

		private float GetSpeed(Agent agent, Bev.MoveSpeed speedType)
		{
			float speed = 0;
			switch (speedType)
			{
				case Bev.MoveSpeed.Slow:
					speed = agent.locomotion.maxWalkSpeed;
					break;
				case Bev.MoveSpeed.Normal:
					speed = agent.locomotion.maxRunSpeed;
					break;
				case Bev.MoveSpeed.Fast:
					speed = agent.locomotion.maxSprintSpeed;
					break;
				default:
					speed = agent.locomotion.maxRunSpeed;
					break;
			}
			return speed;
		}


	}

}