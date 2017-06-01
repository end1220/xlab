
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Lite
{

	public class BuildAssetBundles
	{

		static string tmpOutputPath = "AssetBundles";
		static string copyTargetPath = "Assets/StreamingAssets";

		static string LuaSrcPath = Application.dataPath + "/TwLua";
		static string luaTempDir = Application.dataPath + "/LuaTemp";


		enum AssignType
		{
			Whole,
			ChildFolder,
			Mannully
		}

		struct BuildConfig
		{
			public AssignType assignType;
			public string categoryName;
			public string path;
			public BuildConfig(AssignType ast, string catName, string pa)
			{
				assignType = ast;
				categoryName = catName;
				path = pa;
			}
		}

		static BuildConfig[] configs = new BuildConfig[]
		{
			new BuildConfig(AssignType.Mannully, "base", "Assets/Base"),
			new BuildConfig(AssignType.Whole, "behaviour", "Assets/BehaviourTree"),
			new BuildConfig(AssignType.ChildFolder, "character", "Assets/Characters"),
			new BuildConfig(AssignType.Whole, "config", "Assets/Config"),
			new BuildConfig(AssignType.ChildFolder, "effect", "Assets/Effects"),
			new BuildConfig(AssignType.Mannully, "environment", "Assets/Environment"),
			new BuildConfig(AssignType.Mannully, "map", "Assets/Maps"),
			new BuildConfig(AssignType.Mannully, "prefab", "Assets/Prefabs"),
			new BuildConfig(AssignType.ChildFolder, "missile", "Assets/Prefabs/Missile"),
			new BuildConfig(AssignType.Mannully, "scene", "Assets/Scenes"),
			new BuildConfig(AssignType.ChildFolder, "sound", "Assets/Sound"),
			new BuildConfig(AssignType.Whole, "template", "Assets/Templates"),
			new BuildConfig(AssignType.ChildFolder, "lua", "Assets/LuaTemp/Lua"),
			new BuildConfig(AssignType.Mannully, "ui", "Assets/UI"),
			new BuildConfig(AssignType.ChildFolder, "weapon", "Assets/Weapons"),
		};



		public static void Build()
		{
			try
			{
				string outputPath = Path.Combine(tmpOutputPath, GetPlatformName());
				outputPath = Path.Combine(outputPath, AppDefine.AppName);

				if (Directory.Exists(outputPath))
					Directory.Delete(outputPath, true);
				Directory.CreateDirectory(outputPath);

				MakeLuaTempDir();

				RefreshAssetBundleNames();

				BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

				BuildFileIndex(outputPath);

				Debug.Log("Bulid done.");

				DeleteLuaTempDir();

				CopyAssetBundles(outputPath, copyTargetPath);

				//CopyFiles(Path.Combine(System.Environment.CurrentDirectory, outputPath), AppConst.PersistentDataPath);

				Debug.Log("Copy done.");

				AssetDatabase.Refresh();
			}
			catch (Exception e)
			{
				Debug.LogError(e.ToString());
				EditorUtility.ClearProgressBar();
			}
		}


		public static void RefreshAssetBundleNames()
		{
			try
			{
				//ClearAssetBundleNames();

				for (int i = 0; i < configs.Length; ++i)
				{
					var cfg = configs[i];
					UpdateProgress(i, configs.Length, "Asign Names");
					if (Directory.Exists(cfg.path))
						AssignAssetBundleNames(cfg.path, cfg);
				}

				EditorUtility.ClearProgressBar();
			}
			catch (Exception e)
			{
				Debug.LogError(e.ToString());
				EditorUtility.ClearProgressBar();
			}
		}


		private static void ClearAssetBundleNames()
		{
			string[] oldABNames = AssetDatabase.GetAllAssetBundleNames();

			for (int i = 0; i < oldABNames.Length; i++)
			{
				AssetDatabase.RemoveAssetBundleName(oldABNames[i], true);
				UpdateProgress(i + 1, oldABNames.Length, "Clear Names");
			}

			EditorUtility.ClearProgressBar();
			

			Debug.Log(string.Format("Clear Asset Bundle Names done. Before {0}, Now {1}", oldABNames.Length, AssetDatabase.GetAllAssetBundleNames().Length));
		}


		private static void AssignAssetBundleNames(string source, BuildConfig cfg)
		{
			source = source.Replace("\\", "/");

			if (cfg.assignType == AssignType.Whole)
			{
				AssignNameRecur(source, cfg.categoryName + "/" + cfg.categoryName);
			}
			else if (cfg.assignType == AssignType.ChildFolder)
			{
				string[] folders = Directory.GetDirectories(source);
				for (int i = 0; i < folders.Length; ++i)
				{
					folders[i] = folders[i].Replace("\\", "/");
					string ABName = folders[i].Substring(folders[i].LastIndexOf("/")+1);
					AssignNameRecur(folders[i], cfg.categoryName + "/" + ABName);
				}
				string[] files = Directory.GetFiles(source);
				for (int i = 0; i < files.Length; ++i)
				{
					if (!files[i].EndsWith(".meta") && !files[i].EndsWith(".cs"))
					{
						AssignName(files[i], cfg.categoryName + "/" + cfg.categoryName);
					}
				}
			}
			else
			{
				// do nothing.
			}

		}


		private static void AssignNameRecur(string source, string ABName)
		{
			string[] files = Directory.GetFiles(source);
			for (int i = 0; i < files.Length; ++i)
			{
				if (!files[i].EndsWith(".meta") && !files[i].EndsWith(".cs"))
				{
					AssignName(files[i], ABName);
				}
			}

			string[] folders = Directory.GetDirectories(source);
			for (int i = 0; i < folders.Length; ++i)
			{
				AssignNameRecur(folders[i], ABName);
			}
		}


		private static void AssignName(string assetPath, string ABName)
		{
			AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
			if (assetImporter != null)
				assetImporter.assetBundleName = ABName;
		}


		private static string GetFileName(string assetPath)
		{
			assetPath = assetPath.Replace("\\", "/");
			int startIndex = assetPath.LastIndexOf("/") + 1;
			int len = assetPath.LastIndexOf(".") - startIndex + 1;
			string assetName = assetPath.Substring(startIndex, len);
			return assetName;
		}


		private static void CopyAssetBundles(string sourcePath, string outputPath)
		{
			Directory.CreateDirectory(outputPath);

			var source = Path.Combine(System.Environment.CurrentDirectory, sourcePath);

			var destination = System.IO.Path.Combine(System.Environment.CurrentDirectory, outputPath);
			if (System.IO.Directory.Exists(destination))
				FileUtil.DeleteFileOrDirectory(destination);

			FileUtil.CopyFileOrDirectory(source, destination);
		}


		static void CopyFiles(string fromDirectory, string toDirectory)
		{
			if (!Directory.Exists(fromDirectory))
			{
				Log.Error("Directory Not Exists. " + fromDirectory);
				return;
			}
			if (Directory.Exists(toDirectory))
				Directory.Delete(toDirectory, true);
			
			Directory.CreateDirectory(toDirectory);

			string[] directories = Directory.GetDirectories(fromDirectory);
			if (directories.Length > 0)
			{
				foreach (string d in directories)
				{
					CopyFiles(d, toDirectory + d.Substring(d.LastIndexOf("\\")));
				}
			}

			string[] files = Directory.GetFiles(fromDirectory);
			if (files.Length > 0)
			{
				foreach (string s in files)
				{
					File.Copy(s, toDirectory + s.Substring(s.LastIndexOf("\\")), true);
				}
			}
		}


		static void UpdateProgress(int progress, int progressMax, string desc)
		{
			string title = "[" + progress + " / " + progressMax + "]";
			float value = (float)progress / (float)progressMax;
			EditorUtility.DisplayProgressBar(title, desc, value);
		}


		static string GetPlatformName()
		{
			switch(EditorUserBuildSettings.activeBuildTarget)
			{
				case BuildTarget.Android:
					return "Android";
				case BuildTarget.iOS:
					return "iOS";
				case BuildTarget.StandaloneWindows64:
					return "Win64";
			}
			return "NoTarget";
		}


		#region 文件列表

		static List<string> paths = new List<string>();
		static List<string> files = new List<string>();
		static void BuildFileIndex(string resPath)
		{
			resPath = resPath.Replace("\\", "/");
			if (!resPath.EndsWith("/"))
				resPath = resPath + "/";
			string newFilePath = resPath + "/files.txt";
			if (File.Exists(newFilePath))
			{
				File.Delete(newFilePath);
			}

			paths.Clear();
			files.Clear();
			Recursive(resPath);

			FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
			StreamWriter sw = new StreamWriter(fs);
			foreach (string file in files)
			{
				if (file.EndsWith(".meta") || file.Contains(".DS_Store"))
					continue;

				string md5 = Util.md5file(file);
				string value = file.Replace(resPath, "");
				sw.WriteLine(value + "|" + md5);
			}
			sw.Close();
			fs.Close();
		}

		static void Recursive(string path)
		{
			string[] names = Directory.GetFiles(path);
			string[] dirs = Directory.GetDirectories(path);
			foreach (string filename in names)
			{
				string ext = Path.GetExtension(filename);
				if (ext != null && ext.Equals(".meta"))
				{
					continue;
				}
				files.Add(filename.Replace('\\', '/'));
			}
			foreach (string dir in dirs)
			{
				paths.Add(dir.Replace('\\', '/'));
				Recursive(dir);
			}
		}

		#endregion


		#region Lua特殊处理

		/// <summary>
		/// 把lua源文件复制一份并加上.bytes后缀，作为被打包的文件
		/// </summary>
		static void MakeLuaTempDir()
		{
			string sourceDir = LuaSrcPath;
			string destDir = luaTempDir;

			if (Directory.Exists(destDir))
				Directory.Delete(destDir, true);

			Directory.CreateDirectory(destDir);

			string[] files = Directory.GetFiles(sourceDir, "*.lua", SearchOption.AllDirectories);
			int len = sourceDir.Length;
			if (sourceDir[len - 1] == '/' || sourceDir[len - 1] == '\\')
				--len;

			foreach (string file in files)
			{
				string str = file.Remove(0, len);
				string dest = destDir + "/" + str;
				dest += ".bytes";
				string dir = Path.GetDirectoryName(dest);
				Directory.CreateDirectory(dir);
				File.Copy(file, dest, true);
			}

			AssetDatabase.Refresh();
		}


		static void DeleteLuaTempDir()
		{
			string destDir = luaTempDir;

			if (Directory.Exists(destDir))
				Directory.Delete(destDir, true);

			AssetDatabase.Refresh();
		}

		#endregion

	}


}