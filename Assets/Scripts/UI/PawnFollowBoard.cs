using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Lite;


public class PawnFollowBoard : FollowBoard
{
	private Image hpBar = null;
    private Image shieldBar = null;
    private Text level = null;

    private List<GameObject> cutlist = new List<GameObject>();
    GameObject cutModel = null;

    Text logTxt = null;
    GameObject logContent = null;

    public bool IsDebugBuff = true;


    void Start()
	{
        hpBar = transform.FindChild("bloodbg/blood").GetComponent<Image>();
        level = transform.FindChild("level/Text").GetComponent<Text>();
        cutModel = transform.FindChild("bloodbg/bloodcut").gameObject;
        shieldBar = transform.FindChild("bloodbg/shield").GetComponent<Image>();
        logTxt = transform.FindChild("bloodbg/Panel/LogView").gameObject.GetComponent<Text>();
        logContent = transform.FindChild("bloodbg/Panel").gameObject;
        IsDebugBuff = true;
        logContent.SetActive(IsDebugBuff);

        var pawn = EntityManager.Instance.FindActor(actorId);
		level.text = pawn.blackboard.level.ToString();

		if(pawn == EntityManager.Instance.SelfController)
		{
			transform.SetSiblingIndex(1000);
		}
    }


	void LateUpdate()
	{
		var pawn = EntityManager.Instance.FindActor(actorId);
		if (pawn == null)
		{
			GameObject.Destroy(this.gameObject);
			return;
		}

		if(pawn.blackboard.health <= 0)
		{
			transform.position = new Vector3(9999,9999,9999);
			return;
		}

		// follow position
		Transform agentTransform = pawn.gameObject.transform;
		Vector2 agentScreenPos = Camera.main.WorldToScreenPoint(agentTransform.position + new Vector3(0, 2, 0));
		transform.position = agentScreenPos + new Vector2(-70, 10);

    }

	string tests = "";


	public override void OnFrameTick()
	{
		var pawn = EntityManager.Instance.FindActor(actorId);
		if (pawn == null)
		{
			GameObject.Destroy(this.gameObject);
			return;
		}

		// show attributes
		//AgentBlackboard bb = targetAgent.blackboard;
		
	}


}