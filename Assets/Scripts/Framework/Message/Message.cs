

namespace Lite
{
	public struct Message
	{
		public int Type;

		public MsgLevel Level;

		public object[] Args;


		public Message(int name, MsgLevel level = MsgLevel.Append)
		{
			Type = name;
			Level = level;
			Args = null;
		}

		public Message(int name, object[] args, MsgLevel level = MsgLevel.Append)
		{
			Type = name;
			Args = args;
			Level = level;
		}



	}
}