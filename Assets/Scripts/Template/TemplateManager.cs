
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using Lite;


namespace TwGame
{
	using TemplateDic = Dictionary<int, ScriptableObject>;
	public class TemplateManager : Singleton<TemplateManager>
	{

		private Dictionary<Type, TemplateDic> templatePool = new Dictionary<Type, TemplateDic>();

		#region file path

		private static Dictionary<Type, string> files = new Dictionary<Type, string>()
		{
			/*{typeof(Skill_Template), "Skill"},
			{typeof(Buff_Template), "Buff"},
			{typeof(Npc_Template), "Npc"},
			{typeof(Item_Template), "Item"},*/
		};

		#endregion


		void Awake()
		{
			Init();
		}


		public void Init()
		{
			string scriptObjPath = "Assets/Locke/obj/";

			var iter = files.GetEnumerator();
			while (iter.MoveNext())
			{
				string filePath = iter.Current.Value as string;

				Dictionary<int, ScriptableObject> soDic = new Dictionary<int, ScriptableObject>();

				string[] filePaths = Directory.GetFiles(scriptObjPath + filePath, "*.asset", SearchOption.AllDirectories);

				for (int i = 0; i < filePaths.Length; ++i)
				{
					string strTempPath = filePaths[i].Replace(@"\", "/");
					strTempPath = strTempPath.Substring(strTempPath.IndexOf("Assets"));

					//TwTemplate tp = UnityEditor.AssetDatabase.LoadAssetAtPath(strTempPath, typeof(ScriptableObject)) as TwTemplate;

					//soDic.Add(tp.id, tp);
				}
				templatePool.Add(iter.Current.Key as Type, soDic);
			}
		}


		public T Get<T>(int id) where T : TwTemplate
		{
			Dictionary<int, ScriptableObject> soDic = null;
			templatePool.TryGetValue(typeof(T), out soDic);
			if (soDic != null)
			{
				ScriptableObject so = null;
				soDic.TryGetValue(id, out so);
				return so as T;
			}
			return null;
		}


	}
}
