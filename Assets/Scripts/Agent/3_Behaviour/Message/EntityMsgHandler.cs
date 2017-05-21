
using System;
using System.IO;
using ProtoBuf;
using Lite.Protocol;


namespace Lite
{
	class EntityMsgHandler : IMessageHandler
	{
		public void Register()
		{
			var msgMgr = AppFacade.Instance.msgHandlerManager;
			msgMgr.RegisterMessageHandler((int)MsgID.sb_CreateEntity, OnCreateEntity);
		}

		int OnCreateEntity(byte[] bytes)
		{
			sb_CreateEntity msg = ProtobufUtil.DeSerialize<sb_CreateEntity>(bytes);

			Log.Info(string.Format("OnCreateEntity {0}, {1}, {2}, {3}", (int)msg.career, msg.x, msg.y, msg.z));

			int id = msg.career;
			if (id < 4)
				AvatarFactory.Instance.CreateAgent(id, msg.x / 100.0f, msg.y / 100.0f, msg.z / 100.0f);
			else
				AvatarFactory.Instance.CreateObj(id, msg.x / 100.0f, 0.5f, msg.z / 100.0f);

			return 0;
		}

		

	}
}