
using UnityEngine;


namespace Lite
{

	public abstract class IAgent
	{
		protected long m_guid;

		public IAgent(long guid)
		{
			m_guid = guid;
		}

		public long Guid
		{
			get { return m_guid; }
		}

		public virtual void OnCreate() { }

		public virtual void OnDestroy() { }

		public virtual void OnUpdate() { }

	}

}