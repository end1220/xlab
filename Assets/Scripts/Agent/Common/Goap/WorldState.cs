
using System.Collections;
using System.Collections.Generic;

using Lite.Common;


namespace Lite.Goap
{
	
	public class WorldState
	{
		private BitArray bits;
		private AtomValue[] values;

		public WorldState(int maxStateCount)
		{
			bits = new BitArray(maxStateCount);
			values = new AtomValue[maxStateCount];
		}

		public int MaxStateCount { get { return bits.Count; } }

		public void Reset()
		{
			int count = MaxStateCount;
			for (int i = 0; i < count; ++i)
			{
				bits[i] = false;
				values[i] = null;
			}
		}

		public void Copy(WorldState from)
		{
			int count = MaxStateCount;
			for (int i = 0; i < count; ++i)
			{
				if (from.bits[i])
				{
					if (this.bits[i])
					{
						this.values[i].Copy(from.values[i]);
					}
					else
					{
						this.values[i] = from.values[i].Clone();
						this.bits[i] = true;
					}
				}
				else
				{
					this.bits[i] = false;
					values[i] = null;
				}
			}
		}

		public void Merge(WorldState from)
		{
			int count = MaxStateCount;
			for (int i = 0; i < count; ++i)
			{
				if (from.bits[i])
				{
					if (this.bits[i])
					{
						this.values[i].Copy(from.values[i]);
					}
					else
					{
						this.values[i] = from.values[i].Clone();
						this.bits[i] = true;
					}
				}
			}
		}

		public bool Contains(WorldState subset)
		{
			int count = MaxStateCount;
			for (int i = 0; i < count; ++i)
			{
				if (subset.bits[i])
				{
					if (this.bits[i])
					{
						if (!this.values[i].Equals(subset.values[i]))
							return false;
					}
					else
					{
						return false;
					}
				}
			}
			return true;
		}

		public bool HasState(int index)
		{
			return bits[index];
		}

		public AtomValue Get(int index)
		{
			return values[index];
		}

		public int CountDifference(WorldState other)
		{
			int count = bits.Count;
			int diff = 0;
			for (int i = 0; i < count; ++i)
			{
				diff += bits[i] == other.bits[i] ? 0 : 1;
			}
			return diff;
		}

		public void Set(int index, int value)
		{
			if (bits[index])
			{
				values[index].SetInt(value);
			}
			else
			{
				values[index] = new IntValue(value);
				bits[index] = true;
			}
		}

		public void Set(int index, bool value)
		{
			if (bits[index])
			{
				values[index].SetBool(value);
			}
			else
			{
				values[index] = new BoolValue(value);
				bits[index] = true;
			}
		}



	}


}