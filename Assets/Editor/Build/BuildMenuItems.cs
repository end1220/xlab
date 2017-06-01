
using UnityEngine;
using UnityEditor;
using System.Collections;


namespace Lite
{
	public class BuildMenuItems
	{
		const string kSimulationMode = "Tools/AssetBundle/Simulation Mode";


		[MenuItem(kSimulationMode, false, 1)]
		public static void ToggleSimulationMode()
		{
			ResourceManager.SimulateAssetBundleInEditor = !ResourceManager.SimulateAssetBundleInEditor;
		}


		[MenuItem(kSimulationMode, true, 2)]
		public static bool ToggleSimulationModeValidate()
		{
			Menu.SetChecked(kSimulationMode, ResourceManager.SimulateAssetBundleInEditor);
			return true;
		}


		[MenuItem("Tools/AssetBundle/Refresh AssetBundle Names", false, 3)]
		static public void RefreshABNames()
		{
			try
			{
				BuildAssetBundles.RefreshAssetBundleNames();
			}
			catch(System.Exception e)
			{
				Debug.LogError(e.ToString());
			}
		}


		//===============================Build Asset Bundle=========================

		[MenuItem("Tools/AssetBundle/Build Android", false, 4)]
		static public void BuildABAndroid()
		{
			try
			{
				EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

				BuildAssetBundles.Build();
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.ToString());
			}
		}

		[MenuItem("Tools/AssetBundle/Build iOS", false, 5)]
		static public void BuildABIOS()
		{
			try
			{
				EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);

				BuildAssetBundles.Build();
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.ToString());
			}
		}

		[MenuItem("Tools/AssetBundle/Build Windows64", false, 6)]
		static public void BuildABWin64()
		{
			try
			{
				EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);

				BuildAssetBundles.Build();
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.ToString());
			}
		}


		//===============================Build Player=========================
		
		
		[MenuItem("Tools/Build/Android apk", false, 1)]
		static public void BuildAndroid()
		{
			try
			{
				EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

				BuildPlayer.Build();
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.ToString());
			}
		}

		[MenuItem("Tools/Build/Windows64 exe", false, 2)]
		static public void BuildWin64()
		{
			try
			{
				EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);

				BuildPlayer.Build();
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.ToString());
			}
		}


	}

}