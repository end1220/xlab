
using Lite.Strategy;
using Lite.Common;

namespace Lite.Knowledge
{
	// ��Ϣ��Ӧ��
	public abstract class ISensor
	{
		public int id;

		public Agent owner;

		public bool enabled;

		public abstract void Update();
	}
}