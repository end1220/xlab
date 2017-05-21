using UnityEngine;


namespace Lite.Bev
{
	public abstract class AgentAction
	{
		public ActionType actionType;
		private bool isFinished;

		public AgentAction()
		{
			actionType = ActionType.Default;
			isFinished = false;
		}

		public void Finish()
		{
			isFinished = true;
		}

		public bool IsFinished()
		{
			return isFinished;
		}

		public void Active(Agent agent) { OnActive(agent); }
		public void Deactive(Agent agent) { OnDeactive(agent); }
		public void Process(Agent agent) { OnProcess(agent); }
		public abstract void OnActive(Agent agent);
		public abstract void OnDeactive(Agent agent);
		public abstract void OnProcess(Agent agent);

	}

	/*public class Nothing : AgentAction
	{
		private static Nothing _inst;
		public static Nothing Inst
		{ 
			get 
			{ 
				if (_inst == null) 
					_inst = new Nothing(); 
				return _inst; 
			} 
		}
	}*/

}