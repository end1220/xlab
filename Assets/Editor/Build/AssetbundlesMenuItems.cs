using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Lite
{
	public class AssetBundlesMenuItems
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


		[MenuItem("Tools/AssetBundle/Refresh AB Names", false, 3)]
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


		[MenuItem("Tools/AssetBundle/Build AssetBundles", false, 4)]
		static public void BuildAll()
		{
			try
			{
				BuildAssetBundles.BuildAll();
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.ToString());
			}
		}


		/*[MenuItem("Tools/AssetBundle/Clear AB Names", false, 5)]
		static public void ClearABNames()
		{
			try
			{
				BuildAssetBundles.ClearAssetBundleNames();
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.ToString());
			}
		}*/


	}
}