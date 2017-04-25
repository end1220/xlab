using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Lite;
using Lite.AStar;
using Lite.Graph;
using Lite.Bev;


public class SteeringTest : MonoBehaviour
{
	AppFacade app;

	long bot1_id;

	long bot2_id;

	long bot3_id;

	string[] botFilePath = { "Prefabs/Dwarf/dwarf_01"/*, "Prefabs/Dwarf/dwarf_02", "Prefabs/Dwarf/dwarf_03"*/ };

	Material lineMat;

	

	void Start()
	{
		app = AppFacade.Instance;
		app.Init();

		lineMat = new Material(Shader.Find("Diffuse"));
		lineMat.color = Color.green;
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(20, 20, 60, 30), "bot"))
		{
			if (bot1_id == 0)
				bot1_id = AddBot().Guid;
			else if (bot2_id == 0)
				bot2_id = AddBot().Guid;
			else if (bot3_id == 0)
				bot3_id = AddBot().Guid;
		}

		if (GUI.Button(new Rect(20, 60, 60, 30), "cmd"))
		{
			Agent agent = app.bevAgentManager.FindAgent(bot1_id);
			/*var target = new Vector3(MathUtil.RandFloat() * 15, 0, MathUtil.RandFloat() * 15);
			MoveTo mvt = new MoveTo(target, MoveSpeed.Normal);//(MoveSpeed)MathUtil.RandInt(0,2));
			agent.PushAction(mvt);*/

			/*Lite.Bev.AttackAgent atk = new Lite.Bev.AttackAgent();
			atk.targetAgent = app.behaviourFacade.FindAgent(bot2_id);
			agent.PushAction(atk);*/
			agent.animComponent.attack1 = true;
		}

		if (GUI.Button(new Rect(20, 100, 60, 30), "idle"))
		{
			Agent agent = app.bevAgentManager.FindAgent(bot1_id);
			agent.animComponent.moveSpeed = 0;
		}

		if (GUI.Button(new Rect(20, 140, 60, 30), "run"))
		{
			Agent agent = app.bevAgentManager.FindAgent(bot1_id);
			agent.animComponent.moveSpeed = 1;
		}

	}

	Agent AddBot()
	{
		var agent = new Agent(GuidGenerator.NextLong());
		app.bevAgentManager.AddAgent(agent);

		var go = new GameObject("dwarf");

		var prefab = Resources.Load(botFilePath[MathUtil.RandInt(0, botFilePath.Length - 1)]);
		var goAvatar = GameObject.Instantiate(prefab) as GameObject;
		goAvatar.transform.parent = go.transform;
		goAvatar.transform.position = Vector3.zero;
		goAvatar.transform.localScale = Vector3.one;

		var agentCom = go.AddComponent<AgentComponent>();
		agentCom.agent = agent;
		agent.agentComponent = agentCom;
		var loco = go.AddComponent<LocomotionComponent>();
		agent.locomotion = loco;
		var animCom = go.AddComponent<AnimationComponent>();
		animCom.Init(agent);
		agent.animComponent = animCom;

		goAvatar.layer = LayerMask.NameToLayer(AppDefine.LayerBot);

		float x = MathUtil.RandClamp() * 5;
		float y = 0;
		float z = MathUtil.RandClamp() * 5;
		loco.SetPosition(new Vector3(x, y, z));

		return agent;
	}

}