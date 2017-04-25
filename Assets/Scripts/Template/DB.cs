

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Lite
{
	using Data = Dictionary<int, IData>;
	using StrData = Dictionary<string, IData>;

	public class DB : Singleton<DB>
	{
		private const int mJsonStartIndex = 5;
		private const int mColumnStartIndex = 1;

		private Dictionary<Type, Data> mDataPoolDic = new Dictionary<Type, Data>();
		private Dictionary<Type, StrData> mStrDataPoolDic = new Dictionary<Type, StrData>();

		public bool HasLoaded = false;

		private Dictionary<Type, string> tableList = new Dictionary<Type, string>
			{
				{typeof(Npc0_Data), "Npc0"},
				/*{typeof(Npc1_Data), "Npc1"},
				{typeof(Npc2_Data), "Npc2"},
				{typeof(Npc3_Data), "Npc3"},
				{typeof(Npc4_Data), "Npc4"},
				{typeof(Npc5_Data), "Npc5"},
				{typeof(Npc6_Data), "Npc6"},
				{typeof(Npc7_Data), "Npc7"},
				{typeof(Npc8_Data), "Npc8"},
				{typeof(Npc9_Data), "Npc9"},

				{typeof(Npc10_Data), "Npc10"},
				{typeof(Npc11_Data), "Npc11"},
				{typeof(Npc12_Data), "Npc12"},
				{typeof(Npc13_Data), "Npc13"},
				{typeof(Npc14_Data), "Npc14"},
				{typeof(Npc15_Data), "Npc15"},
				{typeof(Npc16_Data), "Npc16"},
				{typeof(Npc17_Data), "Npc17"},
				{typeof(Npc18_Data), "Npc18"},
				{typeof(Npc19_Data), "Npc19"},

				{typeof(Npc20_Data), "Npc20"},
				{typeof(Npc21_Data), "Npc21"},
				{typeof(Npc22_Data), "Npc22"},
				{typeof(Npc23_Data), "Npc23"},
				{typeof(Npc24_Data), "Npc24"},
				{typeof(Npc25_Data), "Npc25"},
				{typeof(Npc26_Data), "Npc26"},
				{typeof(Npc27_Data), "Npc27"},
				{typeof(Npc28_Data), "Npc28"},
				{typeof(Npc29_Data), "Npc29"},

				{typeof(Npc30_Data), "Npc30"},
				{typeof(Npc31_Data), "Npc31"},
				{typeof(Npc32_Data), "Npc32"},
				{typeof(Npc33_Data), "Npc33"},
				{typeof(Npc34_Data), "Npc34"},
				{typeof(Npc35_Data), "Npc35"},
				{typeof(Npc36_Data), "Npc36"},
				{typeof(Npc37_Data), "Npc37"},
				{typeof(Npc38_Data), "Npc38"},
				{typeof(Npc39_Data), "Npc39"},

				{typeof(Npc40_Data), "Npc40"},
				{typeof(Npc41_Data), "Npc41"},
				{typeof(Npc42_Data), "Npc42"},
				{typeof(Npc43_Data), "Npc43"},
				{typeof(Npc44_Data), "Npc44"},
				{typeof(Npc45_Data), "Npc45"},
				{typeof(Npc46_Data), "Npc46"},
				{typeof(Npc47_Data), "Npc47"},
				{typeof(Npc48_Data), "Npc48"},
				{typeof(Npc49_Data), "Npc49"},

				{typeof(Npc50_Data), "Npc50"},
				{typeof(Npc51_Data), "Npc51"},
				{typeof(Npc52_Data), "Npc52"},
				{typeof(Npc53_Data), "Npc53"},
				{typeof(Npc54_Data), "Npc54"},
				{typeof(Npc55_Data), "Npc55"},
				{typeof(Npc56_Data), "Npc56"},
				{typeof(Npc57_Data), "Npc57"},
				{typeof(Npc58_Data), "Npc58"},
				{typeof(Npc59_Data), "Npc59"},

				{typeof(Npc60_Data), "Npc60"},
				{typeof(Npc61_Data), "Npc61"},
				{typeof(Npc62_Data), "Npc62"},
				{typeof(Npc63_Data), "Npc63"},
				{typeof(Npc64_Data), "Npc64"},
				{typeof(Npc65_Data), "Npc65"},
				{typeof(Npc66_Data), "Npc66"},
				{typeof(Npc67_Data), "Npc67"},
				{typeof(Npc68_Data), "Npc68"},
				{typeof(Npc69_Data), "Npc69"},

				{typeof(Npc70_Data), "Npc70"},
				{typeof(Npc71_Data), "Npc71"},
				{typeof(Npc72_Data), "Npc72"},
				{typeof(Npc73_Data), "Npc73"},
				{typeof(Npc74_Data), "Npc74"},
				{typeof(Npc75_Data), "Npc75"},
				{typeof(Npc76_Data), "Npc76"},
				{typeof(Npc77_Data), "Npc77"},
				{typeof(Npc78_Data), "Npc78"},
				{typeof(Npc79_Data), "Npc79"},

				{typeof(Npc80_Data), "Npc80"},
				{typeof(Npc81_Data), "Npc81"},
				{typeof(Npc82_Data), "Npc82"},
				{typeof(Npc83_Data), "Npc83"},
				{typeof(Npc84_Data), "Npc84"},
				{typeof(Npc85_Data), "Npc85"},
				{typeof(Npc86_Data), "Npc86"},
				{typeof(Npc87_Data), "Npc87"},
				{typeof(Npc88_Data), "Npc88"},
				{typeof(Npc89_Data), "Npc89"},

				{typeof(Npc90_Data), "Npc90"},
				{typeof(Npc91_Data), "Npc91"},
				{typeof(Npc92_Data), "Npc92"},
				{typeof(Npc93_Data), "Npc93"},
				{typeof(Npc94_Data), "Npc94"},
				{typeof(Npc95_Data), "Npc95"},
				{typeof(Npc96_Data), "Npc96"},
				{typeof(Npc97_Data), "Npc97"},
				{typeof(Npc98_Data), "Npc98"},
				{typeof(Npc99_Data), "Npc99"},
				*/


			};

		public Dictionary<Type, string> TableList
		{
			get { return tableList; }
		}

		public IEnumerator Load()
		{
			if (HasLoaded)
			{
				Log.Warning("Skip DB.Load() , _isBuilt == true");
				yield return null;
			}

			IDictionaryEnumerator dicEtor = tableList.GetEnumerator();
			while (dicEtor.MoveNext())
			{
				LoadRes(dicEtor.Key as Type, "Template/" + dicEtor.Value as string);
				yield return new WaitForEndOfFrame();
			}

			HasLoaded = true;

			Log.Info("db done");

			yield return null;
		}

		public void Release()
		{
			foreach (var node in mDataPoolDic)
				node.Value.Clear();
			mDataPoolDic.Clear();

			foreach (var node in mStrDataPoolDic)
				node.Value.Clear();
			mStrDataPoolDic.Clear();
		}

		public void LoadRes(Type type, string path)
		{
			if (string.IsNullOrEmpty(path))
				return;

			if (mDataPoolDic.ContainsKey(type) || mStrDataPoolDic.ContainsKey(type))
				return;

			try
			{
				string rawText = null;
#if UNITY_EDITOR
				var ta = Resources.Load(path) as TextAsset;
				rawText = ta.text;
#else
                    var ta = Resources.Load(path) as TextAsset;
                    rawText = ta.text;
#endif

				Dictionary<int, IData> dic = new Dictionary<int, IData>();
				Dictionary<string, IData> strDic = new Dictionary<string, IData>();
				mDataPoolDic.Add(type, dic);
				mStrDataPoolDic.Add(type, strDic);

				TabReader tabReader = new TabReader();
				tabReader.Load(rawText);
				int JsonNodeCount = tabReader.RowCount;
				for (int i = mJsonStartIndex; i < JsonNodeCount; i++)
				{
					try
					{
						System.Reflection.ConstructorInfo conInfo = type.GetConstructor(Type.EmptyTypes);
						IData t = conInfo.Invoke(null) as IData;

						t.init(tabReader, i, mColumnStartIndex);
						if (0 != t.id)
						{
							if (!dic.ContainsKey(t.id))
								dic.Add(t.id, t);
							else
								Log.Error("Config Data Ready Exist, TableName: " + t.GetType().Name + " ID:" + t.id);
						}
						else
						{
							if (!strDic.ContainsKey(t.strId))
								strDic.Add(t.strId, t);
							else
								Log.Error("Config Data Ready Exist, TableName: " + t.GetType().Name + " ID:" + t.strId);
						}
					}
					catch (Exception e)
					{
						Log.Error(type.ToString() + " ERROR!!! line " + (i + 2).ToString() + ", " + e.ToString());
					}
				}

			}
			catch (UnityException uex)
			{
				Log.Error(uex.ToString());
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
		}

		public IData GetDataByKey(Type type, int key)
		{
			if (mDataPoolDic.ContainsKey(type) && GetDataPool(type).ContainsKey(key))
			{
				return GetDataPool(type)[key];
			}
			if (key > 0)
			{
				Log.Error("Config Data Is Null : " + type.Name + " key: " + key);
			}
			return null;
		}

		public IData GetDataByKey(Type type, string strKey)
		{
			if (mStrDataPoolDic.ContainsKey(type) && GetStrDataPool(type).ContainsKey(strKey))
			{
				return GetStrDataPool(type)[strKey];
			}

			Log.Error("Config Data Is Null : " + type.Name + " key: " + strKey);
			return null;
		}

		public T GetDataByKey<T>(int key) where T : IData
		{
			return GetDataByKey(typeof(T), key) as T;
		}

		public T GetDataByKey<T>(string strKey) where T : IData
		{
			return GetDataByKey(typeof(T), strKey) as T;
		}

		public Dictionary<int, IData> GetDataPool<T>()
		{
			return GetDataPool(typeof(T));
		}

		public Dictionary<string, IData> GetStrDataPool<T>()
		{
			return GetStrDataPool(typeof(T));
		}

		public Dictionary<int, IData> GetDataPool(Type type)
		{
			if (mDataPoolDic.ContainsKey(type))
			{
				return mDataPoolDic[type];
			}

			return null;
		}

		public Dictionary<string, IData> GetStrDataPool(Type type)
		{
			if (mStrDataPoolDic.ContainsKey(type))
			{
				return mStrDataPoolDic[type];
			}

			return null;
		}

		public int GetPoolSize(Type type)
		{
			if (mDataPoolDic.ContainsKey(type))
			{
				return mDataPoolDic[type].Count;
			}

			return 0;
		}

		public IEnumerator GetElement(Type type)
		{
			return null;
		}
	}
}
