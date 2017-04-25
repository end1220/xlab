

namespace Lite
{

	public class AppFacade : Singleton<AppFacade>
	{
		public Knowledge.SensorManager sensorManager { private set; get; }

		public Strategy.AgentManager stgAgentManager { private set; get; }

		public Cmd.CommandManager commandManager { private set; get; }

		public Bev.AgentManager bevAgentManager { private set; get; }

		public MessageHandlerManager msgHandlerManager { private set; get; }

		long lastUpdateTime = 0;

		public AppFacade()
		{

		}

		public void Init()
		{
			sensorManager = new Knowledge.SensorManager();
			sensorManager.Init();

			stgAgentManager = new Strategy.AgentManager();
			stgAgentManager.Init();

			commandManager = new Cmd.CommandManager();
			commandManager.Init();

			bevAgentManager = new Bev.AgentManager();
			bevAgentManager.Init();

			msgHandlerManager = new MessageHandlerManager();
			msgHandlerManager.Init();
		}

		public void Update()
		{
			long ms = GameTimer.tickTime;
			if (ms - lastUpdateTime >= 200)
			{
				sensorManager.Update();
				stgAgentManager.Update();
				lastUpdateTime = ms;
			}

			commandManager.Update();

		}

	}

}