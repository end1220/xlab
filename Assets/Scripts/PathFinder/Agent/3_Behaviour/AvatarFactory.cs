
using UnityEngine;

using Lite.Bev;


namespace Lite
{

	public class AvatarFactory : Singleton<AvatarFactory>
	{
		/*Miner,
		Logger,
		WoodCutter,
		Blacksmith,

		Tree,
		Ore*/
		string[] botFilePath = { "Prefabs/Dwarf/skeleton_archer", "Prefabs/Dwarf/skeleton_archer", "Prefabs/Dwarf/skeleton_archer", "Prefabs/Dwarf/skeleton_archer",
							   "Prefabs/Tree", "Prefabs/Ore" };

		public Agent CreateAgent(int id, float x, float y, float z)
		{
			var agent = new Agent(GuidGenerator.NextLong());
			AppFacade.Instance.bevAgentManager.AddAgent(agent);

			var prefab = Resources.Load(botFilePath[id]);
			var go = GameObject.Instantiate(prefab) as GameObject;
			go.transform.localScale = Vector3.one;

			var agentCom = go.AddComponent<AgentComponent>();
			agentCom.agent = agent;
			agent.agentComponent = agentCom;
			var loco = go.AddComponent<LocomotionComponent>();
			agent.locomotion = loco;
			var animCom = go.AddComponent<AnimationComponent>();
			animCom.Init(agent);
			agent.animComponent = animCom;

			go.layer = LayerMask.NameToLayer(AppDefine.LayerBot);

			loco.SetPosition(new Vector3(x, y, z));

			return agent;
		}

		public Agent CreateObj(int id, float x, float y, float z)
		{
			var agent = new Agent(GuidGenerator.NextLong());
			AppFacade.Instance.bevAgentManager.AddAgent(agent);

			var prefab = Resources.Load(botFilePath[id]);
			var go = GameObject.Instantiate(prefab) as GameObject;
			go.transform.localScale = Vector3.one;
			go.transform.position = new Vector3(x, y, z);

			var agentCom = go.AddComponent<AgentComponent>();
			agentCom.agent = agent;
			agent.agentComponent = agentCom;

			go.layer = LayerMask.NameToLayer(AppDefine.LayerBot);

			return agent;
		}

	}

}