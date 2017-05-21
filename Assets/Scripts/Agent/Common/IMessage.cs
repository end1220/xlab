


namespace Lite
{
	public abstract class IMessage
	{
		public int id;

		public byte[] _ToBytes()
		{
			ByteBuffer bb = new ByteBuffer();
			bb.WriteInt(id);
			bb.WriteBytes(ToBytes());
			return bb.ToBytes();
		}

		protected abstract byte[] ToBytes();

	}

}
