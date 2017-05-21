
using System.Collections;
using System.Collections.Generic;


namespace Lite
{
	public abstract class IAgentManager<T> where T : IAgent
	{
		protected Dictionary<long, T> m_agentMap = new Dictionary<long, T>();

		public virtual void Init()
		{
			
		}

		public virtual void Update()
		{
			IDictionaryEnumerator iter = m_agentMap.GetEnumerator();
			while (iter.MoveNext())
			{
				T agent = iter.Entry.Value as T;
				agent.OnUpdate();
			}
		}

		public virtual void Destroy()
		{
			IDictionaryEnumerator iter = m_agentMap.GetEnumerator();
			while (iter.MoveNext())
			{
				T agent = iter.Entry.Value as T;
				agent.OnDestroy();
			}
			m_agentMap.Clear();
		}

		public virtual void AddAgent(T agent)
		{
			if (!m_agentMap.ContainsKey(agent.Guid))
			{
				m_agentMap.Add(agent.Guid, agent);
				agent.OnCreate();
			}
			else
			{
				Log.Error("IAgentManager.AddAgent : repeated !");
			}
		}

		public virtual void RemoveAgent(long guid)
		{
			var agent = FindAgent(guid);
			if (agent != null)
			{
				agent.OnDestroy();
				m_agentMap.Remove(guid);
			}
			else
			{
				Log.Error("IAgentManager.RemoveAgent : not exist !");
			}
		}

		public T FindAgent(long guid)
		{
			T agent;
			m_agentMap.TryGetValue(guid, out agent);
			return agent;
		}


	}

}