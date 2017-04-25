

using System.Collections.Generic;


namespace Lite.Common
{

	public class Blackboard<T>
	{
		private Dictionary<T, AtomValue> valueDic = new Dictionary<T, AtomValue>();

		private AtomValue Find(T key)
		{
			AtomValue atom;
			valueDic.TryGetValue(key, out atom);
			return atom;
		}

		public int GetInt(T key, int defaultValue)
		{
			IntValue atom = (IntValue)this.Find(key);
			return atom != null ? atom.ToInt() : defaultValue;
		}

		public void SetInt(T key, int value)
		{
			IntValue atom = (IntValue)this.Find(key);
			if (atom == null)
			{
				atom = new IntValue(value);
				valueDic.Add(key, atom);
			}
			atom.SetInt(value);
		}

		public float GetFloat(T key, float defaultValue)
		{
			FloatValue atom = (FloatValue)this.Find(key);
			return atom != null ? atom.ToFloat() : defaultValue;
		}

		public void SetFloat(T key, float value)
		{
			FloatValue atom = (FloatValue)this.Find(key);
			if (atom == null)
			{
				atom = new FloatValue(value);
				valueDic.Add(key, atom);
			}
			atom.SetFloat(value);
		}

		public bool GetBool(T key, bool defaultValue)
		{
			BoolValue atom = (BoolValue)this.Find(key);
			return atom != null ? atom.ToBool() : defaultValue;
		}

		public void SetBool(T key, bool value)
		{
			BoolValue atom = (BoolValue)this.Find(key);
			if (atom == null)
			{
				atom = new BoolValue(value);
				valueDic.Add(key, atom);
			}
			atom.SetBool(value);
		}

		public string GetString(T key, string defaultValue)
		{
			StringValue atom = (StringValue)this.Find(key);
			return atom != null ? atom.ToString() : defaultValue;
		}

		public void SetString(T key, string value)
		{
			StringValue atom = (StringValue)this.Find(key);
			if (atom == null)
			{
				atom = new StringValue(value);
				valueDic.Add(key, atom);
			}
			atom.SetString(value);
		}

	}

}