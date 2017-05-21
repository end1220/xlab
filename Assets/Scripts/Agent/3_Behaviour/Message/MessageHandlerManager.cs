
using System;
using System.Collections.Generic;

using Lite.Cmd;


namespace Lite
{
	using MsgHandlerFunc = Func<byte[], int>;

	interface IMessageHandler
	{
		void Register();
	}

	public abstract class IAgentActionHandler
	{
		public abstract Strategy.GoapAgentAction ToAction(byte[] Command);

		public abstract void OnAction(Strategy.GoapAgentAction action);

		public void OnAction(byte[] Command)
		{
			Strategy.GoapAgentAction action = ToAction(Command);
			OnAction(action);
		}

	}

	public class MessageHandlerManager
	{
		private List<IMessageHandler> m_normalHandlerList = new List<IMessageHandler>();
		private Dictionary<int, MsgHandlerFunc> m_normalHandlerFuncMap = new Dictionary<int, MsgHandlerFunc>();

		private Dictionary<int, IAgentActionHandler> m_agentActionHandlerMap = new Dictionary<int, IAgentActionHandler>();

		public MessageHandlerManager()
		{
			
		}

		public void Init()
		{
			m_normalHandlerList.Add(new EntityMsgHandler());

			foreach (var handler in m_normalHandlerList)
			{
				handler.Register();
			}
		}

		public void RegisterActionHandler(int CommandType, IAgentActionHandler handler)
		{
			m_agentActionHandlerMap.Add(CommandType, handler);
		}

		public void UnregisterActionHandler(int CommandType)
		{
			m_agentActionHandlerMap.Remove(CommandType);
		}

		public void HandleAgentAction(Strategy.GoapAgentAction action)
		{
			IAgentActionHandler handler = null;
			m_agentActionHandlerMap.TryGetValue(action.actionType, out handler);
			if (handler != null)
			{
				handler.OnAction(action);
			}
		}

		public void HandleAgentAction(byte[] cmd)
		{
			ByteBuffer bb = new ByteBuffer(cmd);
			int typeID = bb.ReadInt();
			long agentID = bb.ReadLong();
			byte[] buffer = bb.ReadBytes();
			//Strategy.AgentAction action = ProtobufUtil.DeSerialize<Strategy.AgentAction>(buffer);

			IAgentActionHandler handler = null;
			m_agentActionHandlerMap.TryGetValue(typeID, out handler);
			if (handler != null)
			{
				handler.OnAction(buffer);
			}
		}

		public void RegisterMessageHandler(int msgId, MsgHandlerFunc handler)
		{
			m_normalHandlerFuncMap.Add(msgId, handler);
		}

		public void HandleMessage(byte[] data)
		{
			try
			{
				ByteBuffer buffer = new ByteBuffer(data);
				int msgId = buffer.ReadInt();

				MsgHandlerFunc func = null;
				m_normalHandlerFuncMap.TryGetValue(msgId, out func);
				if (func != null)
				{
					func(buffer.ReadBytes());
				}
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
		}

	}

}