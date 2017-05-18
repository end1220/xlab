

using System;
using System.Collections;
using System.Collections.Generic;


namespace Lite
{

	public class MessageManager : BaseManager
	{

		private Dictionary<string, List<IMessageListener>> mListenerMap = new Dictionary<string, List<IMessageListener>>();

		public override void OnInit()
		{
			mListenerMap.Clear();
		}

		public override void OnDestroy()
		{

		}

		public override void OnTick()
		{
			
		}

		public void RegisterListener(string eventName, IMessageListener listener)
		{
			List<IMessageListener> listenerList = null;
			if (!mListenerMap.TryGetValue(eventName, out listenerList))
			{
				listenerList = new List<IMessageListener>();
				mListenerMap.Add(eventName, listenerList);
			}
			if (!listenerList.Contains(listener))
				listenerList.Add(listener);
		}

		public void UnregisterListener(string eventName, IMessageListener listener)
		{
			List<IMessageListener> listenerList = null;
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
			List<IMessageListener> listenerList = null;
			if (mListenerMap.TryGetValue(evnt.name, out listenerList))
			{
				for (int i = 0; i < listenerList.Count; ++i)
				{
					IMessageListener listener = listenerList[i];
					if (listener != null)
						listener.OnMessage(evnt);
				}
			}
		}


	}

}