
using UnityEngine;
using UnityEngine.UI;
using Lite;


public class StartUp : MonoBehaviour
{
	string msgText = "";

	void Awake()
	{
		App.Instance.Initialize();

	}

	void Start()
	{

	}

	void OnGUI()
	{
		GUI.Label(new Rect(20, 20, 960, 500), msgText);
	}


}
