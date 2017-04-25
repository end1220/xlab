

namespace Lite
{
	public struct Message
	{
		public string name;

		public object body;

		public Message(string name, object body)
		{
			this.name = name;
			this.body = body;
		}

	}

}