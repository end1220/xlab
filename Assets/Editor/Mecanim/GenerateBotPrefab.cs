using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEditorInternal;
using System;
using System.Collections;
using System.Collections.Generic;

using Lite;


enum PrecessingState
{
	Ready,
	Doing,
	Finished,
}

public class GenerateBotPrefab : EditorWindow
{
	PrecessingState mProcessingState = PrecessingState.Ready;

	[MenuItem("Tools/Bot/GenBotPrefab")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		GenerateBotPrefab window = (GenerateBotPrefab)EditorWindow.GetWindow(typeof(GenerateBotPrefab));
		window.Show();
	}

	void OnGUI()
	{

		EditorGUILayout.LabelField("功能描述", "按照配置自动生成bot prefab");
		EditorGUILayout.LabelField("配置路径", "Assets/Resources/Template/AutoGenMecanim.txt");

		string mProcessString = "";
		switch(mProcessingState)
		{
			case PrecessingState.Ready:
				mProcessString = "准备就绪";
				break;
			case PrecessingState.Doing:
				mProcessString = "正在生成...";
				break;
			case PrecessingState.Finished:
				mProcessString = "生成完毕";
				break;
		}
		EditorGUILayout.LabelField("进度状态", mProcessString);

		if (GUILayout.Button("开始生成", GUILayout.Width(255)))
		{
			mProcessingState = PrecessingState.Doing;
			StartGenerate();
		}
	}

	void StartGenerate()
	{
		string configFilePath = "Template/AutoGenMecanim";
		Dictionary<int, IData> configDic = ConfigLoader.LoadConfig(configFilePath, typeof(AutoGenMecanim_Data));
		foreach(IData cfg in configDic.Values)
		{
			AutoGenMecanim_Data agmTbl = cfg as AutoGenMecanim_Data;
			string srcPath = agmTbl.sourcePath;
			string destPath = agmTbl.destPath;

			List<AnimationClip> clips = FindAnimationClips(srcPath.Substring(0, srcPath.LastIndexOf("/")));
			if (clips.Count == 0)
			{
				Debug.LogError("no clips at path : " + srcPath);
				continue;
			}

			string name = srcPath.Substring(srcPath.LastIndexOf("/") + 1).Replace(".fbx", "");
			UnityEditor.Animations.AnimatorController controller = null;
			controller = GenerateController(name, "Assets/" + destPath, clips);

			GameObject prefab = GeneratePrefab("Assets/" + srcPath, "Assets/" + destPath, controller, agmTbl);

			AssetDatabase.SaveAssets();
		}

		mProcessingState = PrecessingState.Finished;
	}

	List<AnimationClip> FindAnimationClips(string sourcePath)
	{
		string assetPath = "Assets/" + sourcePath.Substring(0, sourcePath.LastIndexOf("/"));
		string fullPath = Application.dataPath + "/" + sourcePath;
		string[] foundFiles = System.IO.Directory.GetFiles(fullPath, "*.fbx", System.IO.SearchOption.TopDirectoryOnly);

		List<AnimationClip> clips = new List<AnimationClip>();
		foreach (string oneFile in foundFiles)
		{
			string filePath = "Assets" + oneFile.Replace(Application.dataPath, "").Replace('\\', '/');
			AnimationClip oneClip = (AnimationClip)AssetDatabase.LoadAssetAtPath(filePath, typeof(AnimationClip));
			if (oneClip != null && oneClip.name.IndexOf("@") == -1)
			{
				clips.Add(oneClip);
			}
		}

		return clips;
	}

	UnityEditor.Animations.AnimatorController GenerateController(string name, string destPath, List<AnimationClip> clips)
	{
		string controllerPath = destPath + "Controller/" + name + ".controller";

		UnityEditor.Animations.AnimatorController animatorController = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
		UnityEditor.Animations.AnimatorControllerLayer layer = animatorController.layers[0];
		UnityEditor.Animations.AnimatorStateMachine sm = layer.stateMachine;
		Vector3 anyStatePosition = sm.anyStatePosition;
		float OFFSET_X = 220;
		float OFFSET_Y = 60;
		float ITEM_PER_LINE = 6;
		float originX = anyStatePosition.x + OFFSET_X;
		float originY = anyStatePosition.y - OFFSET_Y * ITEM_PER_LINE / 2;
		float x = originX;
		float y = originY;
		for (int i = 0; i < clips.Count; ++i)
		{
			AnimationClip newClip = clips[i];
			UnityEditor.Animations.AnimatorState state = sm.AddState(newClip.name.ToLower(), new Vector3(x, y, 0));
			UnityEditor.Animations.AnimatorStateTransition trans = sm.AddAnyStateTransition(state);
			state.motion = newClip;
			if (newClip.name == "idle")
				sm.defaultState = state;

			y += OFFSET_Y;
			if (i != 0 && i % ITEM_PER_LINE == 0)
			{
				x += OFFSET_X;
				y = originY;
			}
		}

		return animatorController;
	}

	GameObject GeneratePrefab(string sourcePath, string prefabPath, 
		UnityEditor.Animations.AnimatorController animatorController,
		AutoGenMecanim_Data genTbl)
	{
		string name = sourcePath.Substring(sourcePath.LastIndexOf("/") + 1).Replace(".fbx", "");
		prefabPath = prefabPath + name + ".prefab";

		// logic
		GameObject logicObj = new GameObject(name);

		// avatar
		GameObject fbxObj = AssetDatabase.LoadAssetAtPath(sourcePath, typeof(GameObject)) as GameObject;
		GameObject avatarObj = GameObject.Instantiate(fbxObj);
		avatarObj.name = name;
		avatarObj.transform.parent = logicObj.transform;
		Animator animator = avatarObj.GetComponent<Animator>();
		if (animator == null)
			animator = avatarObj.AddComponent<Animator>();
		animator.applyRootMotion = false;
		animator.runtimeAnimatorController = animatorController;
		//avatarObj.AddComponent<AnimationEventReceiver>();

		
		// hud root
		/*GameObject hudRoot = new GameObject(ObjectDefines.HudRootName);
		hudRoot.transform.parent = logicObj.transform;
		hudRoot.transform.localPosition = new Vector3(0, genTbl.height, 0);

		// foot root
		GameObject footRoot = new GameObject(ObjectDefines.FootRootName);
		footRoot.transform.parent = logicObj.transform;
		footRoot.transform.localPosition = Vector3.zero;

		// foot root
		GameObject shadowRoot = new GameObject(ObjectDefines.ShadowRootName);
		shadowRoot.transform.parent = logicObj.transform;
		shadowRoot.transform.localPosition = Vector3.zero;*/

		// logic components
		CharacterController charController = logicObj.AddComponent<CharacterController>();
		charController.radius = genTbl.radius;
		charController.height = genTbl.height;
		charController.slopeLimit = 45;
		charController.stepOffset = 0.3f;
		charController.skinWidth = 0.08f;
		charController.center = new Vector3(0, 0.87f, 0);

		// finally the prafab
		GameObject prefab = PrefabUtility.CreatePrefab(prefabPath, logicObj);

		GameObject.DestroyImmediate(logicObj, true);

		return prefab;
	}

}