
using ProtoBuf;

namespace Lite
{
	public enum MsgID
	{
		Default = 0,

		sb_CreateEntity,
	}

	[ProtoContract]
	public class sb_CreateEntity : IMessage
	{
		[ProtoMember(1)]
		public int career;
		[ProtoMember(2)]
		public int x;
		[ProtoMember(3)]
		public int y;
		[ProtoMember(4)]
		public int z;

		public sb_CreateEntity()
		{
			id = (int)MsgID.sb_CreateEntity;
		}

		protected override byte[] ToBytes()
		{
			return ProtobufUtil.Serialize<sb_CreateEntity>(this);
		}

	}

}