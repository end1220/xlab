
using System.Collections.Generic;


namespace Lite.Bev
{

	public class BevBlackboard
	{
		public Agent agent;

		private Dictionary<string, int> intDic;
		private Dictionary<string, float> floatDic;
		private Dictionary<string, bool> boolDic;
		private Dictionary<string, string> stringDic;

		private Dictionary<long, Dictionary<string, int>> intScopeDic;
		private Dictionary<long, Dictionary<string, float>> floatScopeDic;
		private Dictionary<long, Dictionary<string, bool>> boolScopeDic;
		private Dictionary<long, Dictionary<string, string>> stringScopeDic;

		public BevBlackboard(Agent agent)
		{
			this.agent = agent;
			intDic = new Dictionary<string, int>();
			floatDic = new Dictionary<string, float>();
			boolDic = new Dictionary<string, bool>();
			stringDic = new Dictionary<string, string>();

			intScopeDic = new Dictionary<long, Dictionary<string, int>>();
			floatScopeDic = new Dictionary<long, Dictionary<string, float>>();
			boolScopeDic = new Dictionary<long, Dictionary<string, bool>>();
			stringScopeDic = new Dictionary<long, Dictionary<string, string>>();
		}

		public int GetInt(string key)
		{
			int i = 0;
			intDic.TryGetValue(key, out i);
			return i;
		}

		public void SetInt(string key, int value)
		{
			if (intDic.ContainsKey(key))
				intDic[key] = value;
			else
				intDic.Add(key, value);
		}

		public float GetFloat(string key)
		{
			float i = 0;
			floatDic.TryGetValue(key, out i);
			return i;
		}

		public void SetFloat(string key, float value)
		{
			if (floatDic.ContainsKey(key))
				floatDic[key] = value;
			else
				floatDic.Add(key, value);
		}

		public bool GetBool(string key)
		{
			bool i = false;
			boolDic.TryGetValue(key, out i);
			return i;
		}

		public void SetBool(string key, bool value)
		{
			if (boolDic.ContainsKey(key))
				boolDic[key] = value;
			else
				boolDic.Add(key, value);
		}

		public string GetString(string key)
		{
			string i = "";
			stringDic.TryGetValue(key, out i);
			return i;
		}

		public void SetString(string key, string value)
		{
			if (stringDic.ContainsKey(key))
				stringDic[key] = value;
			else
				stringDic.Add(key, value);
		}

		public int GetInt(long scope, string key)
		{
			int i = 0;
			Dictionary<string, int> dic;
			intScopeDic.TryGetValue(scope, out dic);
			if (dic == null)
				return i;
			else
				dic.TryGetValue(key, out i);
			return i;
		}

		public void SetInt(long scope, string key, int value)
		{
			Dictionary<string, int> dic;
			intScopeDic.TryGetValue(scope, out dic);
			if (dic == null)
			{
				dic = new Dictionary<string, int>();
				intScopeDic.Add(scope, dic);
			}

			if (dic.ContainsKey(key))
				dic[key] = value;
			else
				dic.Add(key, value);
		}

		public float GetFloat(long scope, string key)
		{
			float i = 0;
			Dictionary<string, float> dic;
			floatScopeDic.TryGetValue(scope, out dic);
			if (dic == null)
				return i;
			else
				dic.TryGetValue(key, out i);
			return i;
		}

		public void SetFloat(long scope, string key, float value)
		{
			Dictionary<string, float> dic;
			floatScopeDic.TryGetValue(scope, out dic);
			if (dic == null)
			{
				dic = new Dictionary<string, float>();
				floatScopeDic.Add(scope, dic);
			}

			if (dic.ContainsKey(key))
				dic[key] = value;
			else
				dic.Add(key, value);
		}

		public bool GetBool(long scope, string key)
		{
			bool i = false;
			Dictionary<string, bool> dic;
			boolScopeDic.TryGetValue(scope, out dic);
			if (dic == null)
				return i;
			else
				dic.TryGetValue(key, out i);
			return i;
		}

		public void SetBool(long scope, string key, bool value)
		{
			Dictionary<string, bool> dic;
			boolScopeDic.TryGetValue(scope, out dic);
			if (dic == null)
			{
				dic = new Dictionary<string, bool>();
				boolScopeDic.Add(scope, dic);
			}

			if (dic.ContainsKey(key))
				dic[key] = value;
			else
				dic.Add(key, value);
		}

		public string GetString(long scope, string key)
		{
			string i = "";
			Dictionary<string, string> dic;
			stringScopeDic.TryGetValue(scope, out dic);
			if (dic == null)
				return i;
			else
				dic.TryGetValue(key, out i);
			return i;
		}

		public void SetString(long scope, string key, string value)
		{
			Dictionary<string, string> dic;
			stringScopeDic.TryGetValue(scope, out dic);
			if (dic == null)
			{
				dic = new Dictionary<string, string>();
				stringScopeDic.Add(scope, dic);
			}

			if (dic.ContainsKey(key))
				dic[key] = value;
			else
				dic.Add(key, value);
		}

	}

}