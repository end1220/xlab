
using System.Collections;
using System.Collections.Generic;

using Lite.Goap;

namespace Lite.Strategy
{

	public class GoapPlan
	{
		private List<GoapAgentAction> actionList;
		private GoapAgentAction currentAction;
		private int currentIndex;
		public bool isFinished { private set; get; }
		private bool isFirstTimeExcute;

		public GoapPlan()
		{
			actionList = new List<GoapAgentAction>();
			currentIndex = 0;
			isFinished = false;
			isFirstTimeExcute = true;
		}

		public void AddAction(GoapAgentAction action)
		{
			actionList.Add(action);
		}

		public void Excute()
		{
			if (actionList.Count > 0)
			{
				if (currentAction == null)
				{
					currentAction = actionList[currentIndex];
					currentAction.OnActive();
				}
				
				currentAction.Update();

				if (currentAction.IsFinished())
				{
					currentAction.OnDeactive();
					currentAction.ApplyEffects();
					currentIndex++;
					if (currentIndex >= actionList.Count)
					{
						isFinished = true;
					}
				}
			}
			else
			{
				isFinished = true;
			}
		}

	}

}

