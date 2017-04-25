
using UnityEngine;
using System.Collections;

using Lite;

public class MagicBox : MonoBehaviour
{
	private static GameObject prefab = null;
	private float scale = 0.5f;

	[System.NonSerialized]
	public float size = 1.0f;
	[System.NonSerialized]
	public int recursive = 4;

	void Start()
	{
		/*if (prefab == null)
			prefab = Resources.Load("prefabs/MagicBox") as GameObject;
		Invoke("_gen", 0.5f);*/
	}

	private void _gen()
	{
		if (size < 1.0f / Mathf.Pow(2, recursive-1))
			return;

		float offset = 0.5f + transform.localScale.x * 0.25f;
		Vector3[] childrenPos = new Vector3[6] { 
				new Vector3(0, offset, 0), 
				new Vector3(0, -offset, 0),
				new Vector3(-offset, 0, 0),
				new Vector3(offset, 0, 0),
				new Vector3(0, 0, offset),
				new Vector3(0, 0, -offset)};
		
		for (int i = 0; i < 6; i++)
		{
			var go = GameObject.Instantiate(prefab) as GameObject;
			go.transform.parent = transform;
			go.transform.localPosition = childrenPos[i];
			go.transform.localScale = new Vector3(scale, scale, scale);
			go.GetComponent<MagicBox>().size = size * scale;
		}
	}

}