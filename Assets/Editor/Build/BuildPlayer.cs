
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Lite
{
	public class BuildPlayer
	{

		public static void Build()
		{
			var outputPath = EditorUtility.SaveFolderPanel("Choose Location of Output", "", "");
			if (outputPath.Length == 0)
				return;

			string[] levels = GetLevelsFromBuildSettings();
			if (levels.Length == 0)
			{
				Debug.Log("Nothing to build.");
				return;
			}

			string targetName = GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget);
			if (targetName == null)
				return;

			BuildAssetBundles.Build();

			BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
			BuildPipeline.BuildPlayer(levels, outputPath + targetName, EditorUserBuildSettings.activeBuildTarget, option);
		}

		static string[] GetLevelsFromBuildSettings()
		{
			List<string> levels = new List<string>();
			for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
			{
				if (EditorBuildSettings.scenes[i].enabled)
					levels.Add(EditorBuildSettings.scenes[i].path);
			}

			return levels.ToArray();
		}

		public static string GetBuildTargetName(BuildTarget target)
		{
			switch (target)
			{
				case BuildTarget.Android:
					return "/666.apk";
				case BuildTarget.StandaloneWindows:
				case BuildTarget.StandaloneWindows64:
					return "/666.exe";
				case BuildTarget.StandaloneOSXIntel:
				case BuildTarget.StandaloneOSXIntel64:
				case BuildTarget.StandaloneOSXUniversal:
					return "/666.app";
				default:
					Debug.Log("Target not implemented.");
					return null;
			}
		}
	
	}


}