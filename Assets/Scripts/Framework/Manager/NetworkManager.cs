using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace Lite
{
	using PacketPair = KeyValuePair<ushort, Packet>;

	public class NetworkManager : BaseManager
	{
		private SocketClient mSocketClient;
		static readonly object mLockObject = new object();
		static Queue<PacketPair> mMessageQueue = new Queue<PacketPair>();

		SocketClient SocketClient
		{
			get
			{
				if (mSocketClient == null)
					mSocketClient = new SocketClient();
				return mSocketClient;
			}
		}

		public void OnInitialize()
		{
			SocketClient.Init();
		}

		public void OnStart()
		{
			//Util.CallMethod("CSharpPort.Network_OnStart");
		}

		public void OnDestroy()
		{
			//Util.CallMethod("CSharpPort.Network_OnDestroy");
			SocketClient.Destroy();
			Debug.Log("~NetworkManager was destroy");
		}

		public static void PushPacket(ushort msgId, Packet packet)
		{
			lock (mLockObject)
			{
				mMessageQueue.Enqueue(new PacketPair(msgId, packet));
			}
		}

		public void OnTick()
		{
			if (mMessageQueue.Count > 0)
			{
				while (mMessageQueue.Count > 0)
				{
					PacketPair pair = mMessageQueue.Dequeue();
					//App.eventManager.SendMessage(MessageDefine.DISPATCH_MESSAGE, pair);
					Packet packet = pair.Value;
					//string str = Encoding.UTF8.GetString(packet.data);
					//Util.CallMethod("CSharpPort.Network_OnMessage", packet.msgId, packet.data, packet.data.Length);
				}
			}
		}

		public void SendConnect()
		{
			SocketClient.SendConnect();
		}

		public void SendBytes(ushort msgId, byte[] buffer)
		{
			var bb = new ByteBuffer();
			bb.WriteShort(msgId);
			bb.WriteBytes(buffer);
			SocketClient.SendMessage(bb);
		}

		public void SendString(ushort msgId, string str)
		{
			var bb = new ByteBuffer();
			bb.WriteShort(msgId);
			bb.WriteString(str);
			SocketClient.SendMessage(bb);

			/*var message = bb.ToBytes();
			ByteBuffer newbb = new ByteBuffer(message);
			Packet packet = new Packet();
			packet.length = (ushort)message.Length;
			packet.msgId = newbb.ReadShort();
			packet.stamp = 0;
			//packet.data = newbb.ReadString();
			Util.CallMethod("Network", "onMessage", packet.msgId, newbb.ReadString());
			//NetworkManager.PushPacket(packet.msgId, packet);
			*/
		}

		public void TestString(string str)
		{
			/*var bb = new ByteBuffer();
			bb.WriteString(str);
			byte[] b = bb.ReadBytes();
			Log.Info("TestString()"+str);*/
		}

	}
}