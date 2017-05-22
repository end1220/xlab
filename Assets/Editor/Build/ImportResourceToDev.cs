using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using TwFramework;



public class ImportResourceToDev : EditorWindow
{

	static readonly string sourcePathKey = "ImportResourceToDev_sourcePath";

	private string sourcePath = "";
	private string targetPath = "";

	private readonly string[] sourceFolders = new string[]
			{
				"Base",
				"BehaviourTree",
				"Characters",
				"Effects",
				"Environment",
				"Maps",
				"Prefabs",
				"Resources",
				"Scenes",
				"ShaderForge",
				"Sound",
				"Templates",
				//"TwLua",
				"UI",
				"Weapons",
			};


	[MenuItem("Tools/AssetBundle/Import Resource to Dev")]
	public static void ShowWindow()
	{
		var sourcePath = UnityEditor.EditorPrefs.GetString(sourcePathKey, "D:\\tw\\jj-scheme-dov\\TWEditor\\Assets");
		var targetPath = Path.Combine(System.Environment.CurrentDirectory, "Assets");

		var wnd = EditorWindow.GetWindow(typeof(ImportResourceToDev)) as ImportResourceToDev;
		wnd.SetPaths(sourcePath, targetPath);
	}


	public void SetPaths(string source, string target)
	{
		sourcePath = source;
		targetPath = target;
	}


	void OnGUI()
	{
		float spaceSize = 3f;

		GUILayout.Label("This tool copies assets to dev project.", EditorStyles.largeLabel);
		GUILayout.Space(spaceSize);

		GUILayout.Label("Only used by programmers.", EditorStyles.boldLabel);
		GUILayout.Space(spaceSize);

		GUILayout.Label("source path：", EditorStyles.boldLabel);
		sourcePath = GUILayout.TextField(sourcePath);
		GUILayout.Space(spaceSize);

		if (GUILayout.Button("Go!"))
		{
			UnityEditor.EditorPrefs.SetString(sourcePathKey, sourcePath);
			CopyWtf();
		}
		GUILayout.Space(spaceSize);

	}


	
	private void CopyWtf()
	{
		try
		{
			sourcePath.Replace("/", "\\");
			if (!sourcePath.EndsWith("\\"))
				sourcePath += "\\";

			targetPath.Replace("/", "\\");
			if (!targetPath.EndsWith("\\"))
				targetPath += "\\";

			if (!Directory.Exists(sourcePath))
			{
				Debug.LogError("Path not exist: " + sourcePath);
				return;
			}

			if (!Directory.Exists(targetPath))
			{
				Debug.LogError("Path not exist: " + targetPath);
				return;
			}

			UpdateProgress(0, sourceFolders.Length, "Copy WFT");

			for (int i = 0; i < sourceFolders.Length; ++i)
			{
				CopyFiles(sourcePath + sourceFolders[i], targetPath + sourceFolders[i]);
				UpdateProgress(i+1, sourceFolders.Length, "Copy WFT");
			}

			Tool.GenMeta.FixAllPrefabInDev();

			FixBevTrees(targetPath + "BehaviourTree\\");

			EditorUtility.ClearProgressBar();
		}
		catch (System.Exception e)
		{
			Debug.LogError(e.ToString());
			EditorUtility.ClearProgressBar();
			AssetDatabase.Refresh();
		}
	}


	void UpdateProgress(int progress, int progressMax, string desc)
	{
		string title = "[" + progress + " / " + progressMax + "]";
		float value = (float)progress / (float)progressMax;
		EditorUtility.DisplayProgressBar(title, desc, value);
	}


	void CopyFiles(string fromDirectory, string toDirectory)
	{
		if (!Directory.Exists(fromDirectory))
		{
			Log.Error("Directory Not Exists. " + fromDirectory);
			return;
		}
		if (!Directory.Exists(toDirectory))
			Directory.CreateDirectory(toDirectory);
		
		string[] directories = Directory.GetDirectories(fromDirectory);
		foreach (string d in directories)
		{
			CopyFiles(d, toDirectory + d.Substring(d.LastIndexOf("\\")));
		}

		string[] files = Directory.GetFiles(fromDirectory);
		foreach (string s in files)
		{
			File.Copy(s, toDirectory + s.Substring(s.LastIndexOf("\\")), true);
		}
		
	}


	void FixBevTrees(string treePath)
	{
		if (!Directory.Exists(treePath))
		{
			Log.Error("BevTree Path Not Exists: " + treePath);
			return;
		}
		string[] directories = Directory.GetDirectories(treePath);
		foreach (string d in directories)
		{
			FixBevTrees(d);
		}
		string[] files = Directory.GetFiles(treePath);
		foreach (string path in files)
		{
			FileStream readFs = File.Open(path, FileMode.Open);
			StreamReader sr = new StreamReader(readFs);
			StringWriter stringWriter = new StringWriter();
			string line;
			bool isChange = false;
			while (!sr.EndOfStream)
			{
				line = sr.ReadLine();
				if (line.IndexOf("TWScript", StringComparison.Ordinal) != -1)
				//if (line.IndexOf("m_serializedData", StringComparison.Ordinal) != -1)
				{
					isChange = true;
					line = line.Replace("TWScript", "Assembly-CSharp");
				}
				stringWriter.WriteLine(line);
			}
			sr.Close();
			readFs.Close();
			if (isChange)
			{
				FileStream writeFs = File.Open(path, FileMode.Create);
				StreamWriter sw = new StreamWriter(writeFs);
				sw.Write(stringWriter.ToString());
				sw.Flush();
				sw.Close();
				writeFs.Close();
			}
			stringWriter.Close();
		}

	}


}
