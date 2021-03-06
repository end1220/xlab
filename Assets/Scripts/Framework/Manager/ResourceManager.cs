﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;



using UObject = UnityEngine.Object;

namespace Lite
{

	public class LoadedAssetBundle
	{
		public AssetBundle assetBundle;
		public int referencedCount;

		public LoadedAssetBundle(AssetBundle assetBundle)
		{
			this.assetBundle = assetBundle;
			this.referencedCount = 1;
		}
	}


	public class ResourceManager : MonoBehaviour, IManager
	{
		string m_BaseDownloadingURL = "Assets/StreamingAssets/";

		string[] m_ActiveVariants = { };

		public AssetBundleManifest AssetBundleManifestObject;

#if UNITY_EDITOR
		static int m_SimulateAssetBundleInEditor = -1;
		const string kSimulateAssetBundles = "SimulateAssetBundles";
#endif

		Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
		Dictionary<string, WWW> m_DownloadingWWWs = new Dictionary<string, WWW>();
		Dictionary<string, string> m_DownloadingErrors = new Dictionary<string, string>();
		List<AssetBundleLoadOperation> m_InProgressOperations = new List<AssetBundleLoadOperation>();
		Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();

		// hot update
		List<string> _downloadFiles = new List<string>();

#if UNITY_EDITOR
		// Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
		public static bool SimulateAssetBundleInEditor
		{
			get
			{
				if (m_SimulateAssetBundleInEditor == -1)
					m_SimulateAssetBundleInEditor = UnityEditor.EditorPrefs.GetBool(kSimulateAssetBundles, true) ? 1 : 0;

				return m_SimulateAssetBundleInEditor != 0;
			}
			set
			{
				int newValue = value ? 1 : 0;
				if (newValue != m_SimulateAssetBundleInEditor)
				{
					m_SimulateAssetBundleInEditor = newValue;
					UnityEditor.EditorPrefs.SetBool(kSimulateAssetBundles, value);
				}
			}
		}
#endif

		public void Init()
		{
			
		}


		public void Tick()
		{
			UpdateLoading();
		}


		public void Destroy()
		{

		}


		public AssetBundleLoadManifestOperation OnInitialize()
		{
			string url = "file://" + Application.persistentDataPath;
			SetSourceAssetBundleURL(url);
			//SetSourceAssetBundleURL("http://www.MyWebsite/MyAssetBundles");

#if UNITY_EDITOR
			if (SimulateAssetBundleInEditor)
				return null;
#endif
			string manifestAssetBundleName = AppDefine.AppName;
			LoadAssetBundle(manifestAssetBundleName, true);
			var operation = new AssetBundleLoadManifestOperation(manifestAssetBundleName, "AssetBundleManifest", typeof(AssetBundleManifest));
			m_InProgressOperations.Add(operation);
			return operation;
		}


		public void SetSourceAssetBundleURL(string absolutePath)
		{
			m_BaseDownloadingURL = absolutePath;
		}

		
		public LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName, out string error)
		{
			if (m_DownloadingErrors.TryGetValue(assetBundleName, out error))
				return null;

			LoadedAssetBundle bundle = null;
			m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
			if (bundle == null)
				return null;

			// No dependencies are recorded, only the bundle itself is required.
			string[] dependencies = null;
			if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
				return bundle;

			// Make sure all dependencies are loaded
			foreach (var dependency in dependencies)
			{
				if (m_DownloadingErrors.TryGetValue(assetBundleName, out error))
					return bundle;

				// Wait all the dependent assetBundles being loaded.
				LoadedAssetBundle dependentBundle;
				m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
				if (dependentBundle == null)
					return null;
			}

			return bundle;
		}


		// Remaps the asset bundle name to the best fitting asset bundle variant.
		protected string RemapVariantName(string assetBundleName)
		{
			string[] bundlesWithVariant = AssetBundleManifestObject.GetAllAssetBundlesWithVariant();

			string[] split = assetBundleName.Split('.');

			int bestFit = int.MaxValue;
			int bestFitIndex = -1;
			// Loop all the assetBundles with variant to find the best fit variant assetBundle.
			for (int i = 0; i < bundlesWithVariant.Length; i++)
			{
				string[] curSplit = bundlesWithVariant[i].Split('.');
				if (curSplit[0] != split[0])
					continue;

				int found = System.Array.IndexOf(m_ActiveVariants, curSplit[1]);

				// If there is no active variant found. We still want to use the first 
				if (found == -1)
					found = int.MaxValue - 1;

				if (found < bestFit)
				{
					bestFit = found;
					bestFitIndex = i;
				}
			}

			if (bestFit == int.MaxValue - 1)
			{
				Log.Warning("Ambigious asset bundle variant chosen because there was no matching active variant: " + bundlesWithVariant[bestFitIndex]);
			}

			if (bestFitIndex != -1)
			{
				return bundlesWithVariant[bestFitIndex];
			}
			else
			{
				return assetBundleName;
			}
		}


		void UpdateLoading()
		{
			// Collect all the finished WWWs.
			var keysToRemove = new List<string>();
			foreach (var keyValue in m_DownloadingWWWs)
			{
				WWW download = keyValue.Value;

				// If downloading fails.
				if (download.error != null)
				{
					m_DownloadingErrors.Add(keyValue.Key, string.Format("Failed downloading bundle {0} from {1}: {2}", keyValue.Key, download.url, download.error));
					keysToRemove.Add(keyValue.Key);
					continue;
				}

				// If downloading succeeds.
				if (download.isDone)
				{
					AssetBundle bundle = download.assetBundle;
					if (bundle == null)
					{
						m_DownloadingErrors.Add(keyValue.Key, string.Format("{0} is not a valid asset bundle.", keyValue.Key));
						keysToRemove.Add(keyValue.Key);
						continue;
					}

					//Log.Info("Downloading " + keyValue.Key + " is done at frame " + Time.frameCount);
					m_LoadedAssetBundles.Add(keyValue.Key, new LoadedAssetBundle(download.assetBundle));
					keysToRemove.Add(keyValue.Key);
				}
			}

			// Remove the finished WWWs.
			foreach (var key in keysToRemove)
			{
				WWW download = m_DownloadingWWWs[key];
				m_DownloadingWWWs.Remove(key);
				download.Dispose();
			}

			// Update all in progress operations
			for (int i = 0; i < m_InProgressOperations.Count; )
			{
				if (!m_InProgressOperations[i].Update())
				{
					m_InProgressOperations.RemoveAt(i);
				}
				else
					i++;
			}
		}


		public T LoadAsset<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
		{
			string path = m_BaseDownloadingURL + assetBundleName;
			var ab = AssetBundle.LoadFromFile(path);
			if (ab == null)
			{
				Log.Error("ResourceManager.LoadAsset: file not exist: " + path);
				return null;
			}
			var obj = ab.LoadAsset<T>(assetName);
			if (obj == null)
			{
				Log.Error("ResourceManager.LoadAsset: asset " + assetName + " not exist in " + path);
			}
			return obj;
		}


		public AssetBundleLoadAssetOperation LoadAssetAsync<T>(string assetBundleName, string assetName)
		{
			return LoadAssetAsync(assetBundleName, assetName, typeof(T));
		}


		public AssetBundleLoadAssetOperation LoadAssetAsync(string assetBundleName, string assetName, System.Type type)
		{
			//Log.Info("Loading " + assetName + " from " + assetBundleName + " bundle");

			AssetBundleLoadAssetOperation operation = null;
#if UNITY_EDITOR
			if (SimulateAssetBundleInEditor)
			{
				string[] assetPths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);

				int index1 = assetName.LastIndexOf("/");
				int len = assetName.LastIndexOf(".") - index1 - 1;
				assetName = assetName.Substring(index1 + 1, len);
				string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
				if (assetPaths.Length == 0)
				{
					Log.Error("There is no asset with name \"" + assetName + "\" in " + assetBundleName);
					return null;
				}

				// @TODO: Now we only get the main object from the first asset. Should consider type also.
				UnityEngine.Object target = UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);
				operation = new AssetBundleLoadAssetOperationSimulation(target);
			}
			else
#endif
			{
				assetBundleName = RemapVariantName(assetBundleName);
				LoadAssetBundle(assetBundleName, false);
				operation = new AssetBundleLoadAssetOperationFull(assetBundleName, assetName, type);

				m_InProgressOperations.Add(operation);
			}

			return operation;
		}

		
		public AssetBundleLoadOperation LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive)
		{
			//Log.Info("Loading " + levelName + " from " + assetBundleName + " bundle");

			AssetBundleLoadOperation operation = null;
#if UNITY_EDITOR
			if (SimulateAssetBundleInEditor)
			{
				operation = new AssetBundleLoadLevelSimulationOperation(assetBundleName, levelName, isAdditive);
			}
			else
#endif
			{
				assetBundleName = RemapVariantName(assetBundleName);
				LoadAssetBundle(assetBundleName, false);
				operation = new AssetBundleLoadLevelOperation(assetBundleName, levelName, isAdditive);

				m_InProgressOperations.Add(operation);
			}

			return operation;
		}


		private void LoadAssetBundle(string assetBundleName, bool isLoadingAssetBundleManifest)
		{
			//Log.Info("Loading Asset Bundle " + (isLoadingAssetBundleManifest ? "Manifest: " : ": ") + assetBundleName);

#if UNITY_EDITOR
			if (SimulateAssetBundleInEditor)
				return;
#endif

			if (!isLoadingAssetBundleManifest)
			{
				if (AssetBundleManifestObject == null)
				{
					Log.Error("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
					return;
				}
			}

			// Check if the assetBundle has already been processed.
			bool isAlreadyProcessed = LoadAssetBundleInternal(assetBundleName, isLoadingAssetBundleManifest);

			// Load dependencies.
			if (!isAlreadyProcessed && !isLoadingAssetBundleManifest)
				LoadDependencies(assetBundleName);
		}


		public AssetBundleLoadOperationPure LoadAssetBundleAsync(string assetBundleName)
		{
			if (AssetBundleManifestObject == null)
			{
				Log.Error("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
				return null;
			}

			// Check if the assetBundle has already been processed.
			bool isAlreadyProcessed = LoadAssetBundleInternal(assetBundleName, false);
			if (!isAlreadyProcessed)
				LoadDependencies(assetBundleName);

			AssetBundleLoadOperationPure operation = null;
			operation = new AssetBundleLoadOperationPure(assetBundleName);
			m_InProgressOperations.Add(operation);
			return operation;
		}


		protected bool LoadAssetBundleInternal(string assetBundleName, bool isLoadingAssetBundleManifest)
		{
			// Already loaded.
			LoadedAssetBundle bundle = null;
			m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
			if (bundle != null)
			{
				bundle.referencedCount++;
				return true;
			}

			// @TODO: Do we need to consider the referenced count of WWWs?
			// In the demo, we never have duplicate WWWs as we wait LoadAssetAsync()/LoadLevelAsync() to be finished before calling another LoadAssetAsync()/LoadLevelAsync().
			// But in the real case, users can call LoadAssetAsync()/LoadLevelAsync() several times then wait them to be finished which might have duplicate WWWs.
			if (m_DownloadingWWWs.ContainsKey(assetBundleName))
				return true;

			WWW download = null;
			string url = m_BaseDownloadingURL + assetBundleName;

			// For manifest assetbundle, always download it as we don't have hash for it.
			if (isLoadingAssetBundleManifest)
				download = new WWW(url);
			else
				download = WWW.LoadFromCacheOrDownload(url, AssetBundleManifestObject.GetAssetBundleHash(assetBundleName), 0);

			m_DownloadingWWWs.Add(assetBundleName, download);

			return false;
		}

		
		protected void LoadDependencies(string assetBundleName)
		{
			if (AssetBundleManifestObject == null)
			{
				Log.Error("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
				return;
			}

			// Get dependecies from the AssetBundleManifest object..
			string[] dependencies = AssetBundleManifestObject.GetAllDependencies(assetBundleName);
			if (dependencies.Length == 0)
				return;

			for (int i = 0; i < dependencies.Length; i++)
				dependencies[i] = RemapVariantName(dependencies[i]);

			// Record and load all dependencies.
			m_Dependencies.Add(assetBundleName, dependencies);
			for (int i = 0; i < dependencies.Length; i++)
				LoadAssetBundleInternal(dependencies[i], false);
		}

		
		public void UnloadAssetBundle(string assetBundleName)
		{
#if UNITY_EDITOR
			if (SimulateAssetBundleInEditor)
				return;
#endif
			UnloadAssetBundleInternal(assetBundleName);
			UnloadDependencies(assetBundleName);

			//Log.Info(m_LoadedAssetBundles.Count + " assetbundle(s) in memory after unloading " + assetBundleName);
		}

		protected void UnloadDependencies(string assetBundleName)
		{
			string[] dependencies = null;
			if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
				return;

			foreach (var dependency in dependencies)
			{
				UnloadAssetBundleInternal(dependency);
			}

			m_Dependencies.Remove(assetBundleName);
		}

		protected void UnloadAssetBundleInternal(string assetBundleName)
		{
			string error;
			LoadedAssetBundle bundle = GetLoadedAssetBundle(assetBundleName, out error);
			if (bundle == null)
				return;

			if (--bundle.referencedCount == 0)
			{
				bundle.assetBundle.Unload(false);
				m_LoadedAssetBundles.Remove(assetBundleName);

				//Log.Info(assetBundleName + " has been unloaded successfully");
			}
		}



	} // End of AssetBundleManager.


}
