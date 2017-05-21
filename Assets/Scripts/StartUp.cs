
using UnityEngine;
using UnityEngine.UI;
using Lite;


public class StartUp : MonoBehaviour, IMessageListener
{
	string msgText = "";

	void Awake()
	{
		App.Instance.Initialize();

		App.msgManager.RegisterListener(MessageDefine.UPDATE_MESSAGE, this);
		App.msgManager.RegisterListener(MessageDefine.UPDATE_EXTRACT, this);
		App.msgManager.RegisterListener(MessageDefine.UPDATE_DOWNLOAD, this);
		App.msgManager.RegisterListener(MessageDefine.UPDATE_PROGRESS, this);

		//InputField field = null;
		//field.text;
		App.Instance.StartManagers();
	}

	void Start()
	{
		
	}

	void OnGUI()
	{
		GUI.Label(new Rect(20, 20, 960, 500), msgText);
	}

	public void OnMessage(Message msg)
	{
		switch (msg.name)
		{
			case MessageDefine.UPDATE_MESSAGE:
				msgText = msg.body.ToString();
				break;
			case MessageDefine.UPDATE_EXTRACT:
				msgText = msg.body.ToString();
				break;
			case MessageDefine.UPDATE_DOWNLOAD:
				msgText = msg.body.ToString();
				break;
			case MessageDefine.UPDATE_PROGRESS:
				msgText = msg.body.ToString();
				break;
		}
	}

	void OnDestroy()
	{
		App.msgManager.UnregisterListener(MessageDefine.UPDATE_MESSAGE, this);
	}

}
