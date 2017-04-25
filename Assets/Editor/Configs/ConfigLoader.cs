
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Lite;

class ConfigLoader
{
	public static Dictionary<int, IData> LoadConfig(string path, Type type)
	{
		int jsonStartIndex = 4;
		int columnStartIndex = 1;

		var ta = Resources.Load(path) as TextAsset;
		string RawJson = ta.text;

		Dictionary<int, IData> dic = new Dictionary<int, IData>();
		TabReader tabReader = new TabReader();
		tabReader.Load(RawJson);
		int JsonNodeCount = tabReader.RowCount;
		for (int i = jsonStartIndex; i < JsonNodeCount; i++)
		{
			try
			{
				System.Reflection.ConstructorInfo conInfo = type.GetConstructor(Type.EmptyTypes);
				IData t = conInfo.Invoke(null) as IData;

				t.init(tabReader, i, columnStartIndex);
				if (0 != t.id)
				{
					if (!dic.ContainsKey(t.id))
						dic.Add(t.id, t);
					else
						Log.Error("Config Data Ready Exist, TableName: " + t.GetType().Name + " ID:" + t.id);
				}
			}
			catch (Exception)
			{
				Log.Error(type.ToString() + " ERROR!!! line " + (i + 2).ToString());
			}
		}
		return dic;
	}
}