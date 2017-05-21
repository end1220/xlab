using UnityEngine;
using System.Collections;

public class RuntimeStatus : MonoBehaviour
{
	private const float offsetX = 3;
	private const float textWidth = 150;
	private const float fontHeight = 20;
	private const float refreshTime = 0.5f;

	private float lastTime;
	private int totalFrames = 0;
	private float fps;

	void Start()
	{
		lastTime = Time.realtimeSinceStartup;
		totalFrames = 0;
	}

	void OnGUI()
	{
		GUI.color = Color.green;
		int index = 0;
		index++;
		GUI.Label(new Rect(offsetX, Screen.height - fontHeight * index, textWidth, fontHeight), "fps:" + fps.ToString("f1"));
		/*index++;
		GUI.Label(new Rect(offsetX, Screen.height - fontHeight * index, textWidth, fontHeight), "fps:" + fps.ToString("f1"));
		index++;
		GUI.Label(new Rect(offsetX, Screen.height - fontHeight * index, textWidth, fontHeight), "fps:" + fps.ToString("f1"));
		*/
	}

	void Update()
	{
		++totalFrames;

		if (Time.realtimeSinceStartup > lastTime + refreshTime)
		{
			fps = totalFrames / (Time.realtimeSinceStartup - lastTime);
			totalFrames = 0;
			lastTime = Time.realtimeSinceStartup;
		}
	}
}
