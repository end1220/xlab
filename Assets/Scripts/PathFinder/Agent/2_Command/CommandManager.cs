

using System.Collections.Generic;
using Lite.Strategy;




namespace Lite.Cmd
{

	public class CommandManager
	{
		private Queue<byte[]> commandList = new Queue<byte[]>();

		public void Init()
		{

		}

		public void OnActionActive()
		{

		}

		/*public void PushCommand(GoapAgentAction action)
		{
			if (CheckAction(action))
			{
				if (ConstDefine.STAND_ALONE)
					AppFacade.Instance.msgHandlerManager.HandleAgentAction(action);
				else
					commandList.Enqueue(action._ToBytes());
			}
		}*/

		public void Update()
		{
			if (commandList.Count > 0)
			{
				byte[] cmd = commandList.Dequeue();
				AppFacade.Instance.msgHandlerManager.HandleAgentAction(cmd);
			}
		}

		private bool CheckAction(GoapAgentAction action)
		{
			return true;
		}

		public void PushMessage(IMessage msg)
		{
			if (ConstDefine.STAND_ALONE)
				AppFacade.Instance.msgHandlerManager.HandleMessage(msg._ToBytes());
			else
				AppFacade.Instance.msgHandlerManager.HandleMessage(msg._ToBytes());
		}

	}

}