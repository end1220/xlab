
using Lite.Strategy;
using Lite.Common;

namespace Lite.Knowledge
{
	// 信息感应器
	public abstract class ISensor
	{
		public int id;

		public Agent owner;

		public bool enabled;

		public abstract void Update();
	}
}