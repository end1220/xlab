using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TwFramework;


namespace TwFramework
{

	/// <summary>
	/// 打包模板数据
	/// </summary>
	public class AB_Template
	{
		static string assetBundlesPath = "AssetBundles";

		static List<AssetBundleBuild> maps = new List<AssetBundleBuild>();

		public static void Build()
		{
			string rawFilePath = "Assets/Dev/Templates/";
			string outputPath = Path.Combine(assetBundlesPath, Utils.GetPlatformName());
			outputPath = Path.Combine(outputPath, "Template");

			if (Directory.Exists(outputPath))
				Directory.Delete(outputPath, true);
			Directory.CreateDirectory(outputPath);
			
			maps.Clear();

			string[] dirs = Directory.GetDirectories(rawFilePath);
			for (int i = 0; i < dirs.Length; ++i)
			{
				var folderName = dirs[i].Substring(dirs[i].LastIndexOf("/") + 1);
				AddBuildMap(folderName + ".unity3d", "*.asset", dirs[i]);
			}

			BuildPipeline.BuildAssetBundles(outputPath, maps.ToArray(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

			Debug.Log("Bulid Templates done.");

			string copyTargetPath = "Assets/StreamingAssets/Template";
			CopyAssetBundles(outputPath, copyTargetPath);

			Debug.Log("Copy files done.");
		}


		static void AddBuildMap(string bundleName, string pattern, string path)
		{
			string[] buildFiles = Directory.GetFiles(path, pattern);
			if (buildFiles.Length == 0) return;

			for (int i = 0; i < buildFiles.Length; i++)
			{
				buildFiles[i] = buildFiles[i].Replace('\\', '/');
			}
			AssetBundleBuild build = new AssetBundleBuild();
			build.assetBundleName = bundleName;
			build.assetNames = buildFiles;
			maps.Add(build);
		}


		static void CopyAssetBundles(string sourcePath, string outputPath)
		{
			Directory.CreateDirectory(outputPath);

			var source = Path.Combine(System.Environment.CurrentDirectory, sourcePath);

			var destination = System.IO.Path.Combine(System.Environment.CurrentDirectory, outputPath);
			if (System.IO.Directory.Exists(destination))
				FileUtil.DeleteFileOrDirectory(destination);

			FileUtil.CopyFileOrDirectory(source, destination);
		}

	}

}