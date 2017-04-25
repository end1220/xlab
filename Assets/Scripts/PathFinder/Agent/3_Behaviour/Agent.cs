
using UnityEngine;


namespace Lite.Bev
{


	public enum MotionType
	{
		None = 0,
		Idle,
		Die,
		Walk,
		Run,
		Sprint,
		Jump,
		Attack,
	}


	public class Agent : IAgent
	{
		public AgentComponent agentComponent;

		public LocomotionComponent locomotion;

		public AnimationComponent animComponent;

		public MotionType currentMotionType;

		public BevBlackboard blackboard;


		public Agent(long guid) :
			base(guid)
		{
			blackboard = new BevBlackboard(this);
		}

		public void PushAction(Bev.AgentAction action)
		{
			agentComponent.PushAction(action);
		}

	}

}