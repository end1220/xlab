

using Lite.Goap;
using Lite.Knowledge;
using Lite.Common;


namespace Lite.Strategy
{
	public enum Career
	{
		Miner,
		Logger,
		WoodCutter,
		Blacksmith,

		Tree,
		Ore
	}

	public class Agent : IAgent
	{
		public Career career { private set; get; }

		public string name;
		public int x;
		public int y;
		public int z;

		public ISensor sensor { private set; get; }

		public Blackboard<int> blackboard { private set; get; }

		private GoapManager goapManager;

		public WorldState worldState { private set; get; }

		public int numLogs = 0;
		public int numFirewood = 0;
		public int numOre = 0;
		public int numTools = 0;


		public Agent(long guid, Career career) :
			base(guid)
		{
			this.career = career;
			blackboard = new Blackboard<int>();
			goapManager = new GoapManager(this);
			worldState = new WorldState(GoapDefines.STATE_COUNT);
		}

		public override void OnCreate()
		{
			AppFacade.Instance.sensorManager.AddSensor<SimpleAgentSensor>(this);
		}

		public override void OnDestroy()
		{
			
		}

		public override void OnUpdate()
		{
			UpdateWorldState();
			goapManager.Update();
		}

		public void AddGoal(GoapGoal goal)
		{
			goapManager.AddGoal(goal);
		}

		private void UpdateWorldState()
		{
			worldState.Set((int)WorldStateType.HasTool, numTools>0);
			worldState.Set((int)WorldStateType.HasFirewood, numFirewood>0);
			worldState.Set((int)WorldStateType.HasOre, numOre>0);
			worldState.Set((int)WorldStateType.HasLogs, numLogs>0);
		}

	}

}