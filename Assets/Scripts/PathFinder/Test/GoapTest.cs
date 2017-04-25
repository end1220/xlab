using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Lite;
using Lite.Strategy;
using Lite.Goap;
using Lite.Knowledge;


public class GoapTest : MonoBehaviour
{

	void Start()
	{
		AppFacade.Instance.Init();

		Agent agent;

		agent = EntityFactory.Instance.CreateAgent(Career.Tree, -500, 0, -500);

		agent = EntityFactory.Instance.CreateAgent(Career.Ore, -500, 0, 500);

		agent = EntityFactory.Instance.CreateAgent(Career.Blacksmith, 0, 0, 0);
		agent.AddGoal(new Goal_MakeTools());
		/*
		agent = EntityFactory.Instance.CreateAgent(Career.Logger, 0, 0, 0);
		agent.AddGoal(new Goal_MakeLogs());

		agent = EntityFactory.Instance.CreateAgent(Career.Miner, 0, 0, 0);
		agent.AddGoal(new Goal_MakeOre());

		agent = EntityFactory.Instance.CreateAgent(Career.WoodCutter, 0, 0, 0);
		agent.AddGoal(new Goal_MakeFirewood());
		*/
	}

	void Update()
	{
		AppFacade.Instance.Update();
	}

	/*long mills = 0;
	void OnGUI()
	{
		
		if (GUI.Button(new Rect(10, 10, 40, 20), "T"))
		{
			Stopwatch watch = new Stopwatch();
			watch.Start();
			watch.Stop();
			mills = watch.ElapsedMilliseconds;
		}
		GUI.Label(new Rect(50, 0, 100, 30), "ms " + mills);

	}*/

}