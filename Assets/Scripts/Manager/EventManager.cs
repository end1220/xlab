

using System;
using System.Collections;
using System.Collections.Generic;


namespace Lite
{

	public class EventManager : Manager
	{

		private Dictionary<string, List<IListener>> mListenerMap = new Dictionary<string, List<IListener>>();

		public override void Initialize()
		{
			mListenerMap.Clear();
		}

		public override void Destroy()
		{

		}

		public override void Update()
		{
			
		}

		public void RegisterListener(string eventName, IListener listener)
		{
			List<IListener> listenerList = null;
			if (!mListenerMap.TryGetValue(eventName, out listenerList))
			{
				listenerList = new List<IListener>();
				mListenerMap.Add(eventName, listenerList);
			}
			if (!listenerList.Contains(listener))
				listenerList.Add(listener);
		}

		public void UnregisterListener(string eventName, IListener listener)
		{
			List<IListener> listenerList = null;
			if (mListenerMap.TryGetValue(eventName, out listenerList))
			{
				if (listenerList.Contains(listener))
					listenerList.Remove(listener);
			}
		}

		public void SendMessage(string evntName, object msg)
		{
			Message evnt = new Message(evntName, msg);
			this.SendMessage(evnt);
		}

		public void SendMessage(Message evnt)
		{
			List<IListener> listenerList = null;
			if (mListenerMap.TryGetValue(evnt.name, out listenerList))
			{
				for (int i = 0; i < listenerList.Count; ++i)
				{
					IListener listener = listenerList[i];
					if (listener != null)
						listener.OnMessage(evnt);
				}
			}
		}


	}

}