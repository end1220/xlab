



namespace Lite.Common
{
	
	public abstract class AtomValue
	{
		public abstract bool Equals(AtomValue other);
		public abstract AtomValue Clone();
		public abstract void Copy(AtomValue from);
		public virtual int ToInt() { return 0; }
		public virtual void SetInt(int value) { }
		public virtual long ToLong() { return 0; }
		public virtual void SetLong(long value) { }
		public virtual float ToFloat() { return 0; }
		public virtual void SetFloat(float value) { }
		public virtual bool ToBool() { return false; }
		public virtual void SetBool(bool value) { }
		public virtual string ToString() { return string.Empty; }
		public virtual void SetString(string value) { }
	}

	public class IntValue : AtomValue
	{
		int value;
		public IntValue(int value)
		{
			this.value = value;
		}
		public override int ToInt()
		{
			return value;
		}
		public override void SetInt(int value)
		{
			this.value = value;
		}
		public override bool Equals(AtomValue other)
		{
			IntValue o = (IntValue)other;
			if (o != null)
				return this.value == o.value;
			return false;
		}
		public override AtomValue Clone()
		{
			return new IntValue(value);
		}
		public override void Copy(AtomValue from)
		{
			this.value = (from as IntValue).value;
		}
	}
	public class LongValue : AtomValue
	{
		long value;
		public LongValue(long value)
		{
			this.value = value;
		}
		public override long ToLong()
		{
			return value;
		}
		public override void SetLong(long value)
		{
			this.value = value;
		}
		public override bool Equals(AtomValue other)
		{
			LongValue o = (LongValue)other;
			if (o != null)
				return this.value == o.value;
			return false;
		}
		public override AtomValue Clone()
		{
			return new LongValue(value);
		}
		public override void Copy(AtomValue from)
		{
			this.value = (from as LongValue).value;
		}
	}
	public class FloatValue : AtomValue
	{
		float value;
		public FloatValue(float value)
		{
			this.value = value;
		}
		public override float ToFloat()
		{
			return value;
		}
		public override void SetFloat(float value)
		{
			this.value = value;
		}
		public override bool Equals(AtomValue other)
		{
			FloatValue o = (FloatValue)other;
			if (o != null)
				return this.value == o.value;
			return false;
		}
		public override AtomValue Clone()
		{
			return new FloatValue(value);
		}
		public override void Copy(AtomValue from)
		{
			this.value = (from as FloatValue).value;
		}
	}
	public class BoolValue : AtomValue
	{
		bool value;
		public BoolValue(bool value)
		{
			this.value = value;
		}
		public override bool ToBool()
		{
			return value;
		}
		public override void SetBool(bool value)
		{
			this.value = value;
		}
		public override bool Equals(AtomValue other)
		{
			BoolValue o = (BoolValue)other;
			if (o != null)
				return this.value == o.value;
			return false;
		}
		public override AtomValue Clone()
		{
			return new BoolValue(value);
		}
		public override void Copy(AtomValue from)
		{
			this.value = (from as BoolValue).value;
		}
	}
	public class StringValue : AtomValue
	{
		string value;
		public StringValue(string value)
		{
			this.value = value;
		}
		public override string ToString()
		{
			return value;
		}
		public override void SetString(string value)
		{
			this.value = value;
		}
		public override bool Equals(AtomValue other)
		{
			StringValue o = (StringValue)other;
			if (o != null)
				return this.value == o.value;
			return false;
		}
		public override AtomValue Clone()
		{
			return new StringValue(value);
		}
		public override void Copy(AtomValue from)
		{
			this.value = (from as StringValue).value;
		}
	}

}