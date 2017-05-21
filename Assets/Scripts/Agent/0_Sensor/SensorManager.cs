
using System.Collections;
using System.Collections.Generic;
using Lite.Strategy;

namespace Lite.Knowledge
{

	public class SensorManager
	{
		private Dictionary<int, ISensor> sensorMap;
		private int idCounter = 0;

		public SensorManager()
		{
			sensorMap = new Dictionary<int, ISensor>();
		}

		public void Init()
		{

		}

		public T AddSensor<T>(Agent agent) where T : ISensor, new()
		{
			T sensor = new T();
			sensor.id = ++idCounter;
			sensor.owner = agent;
			sensor.enabled = true;
			sensorMap.Add(sensor.id, sensor);
			return sensor;
		}

		public void RemoveSensor(int id)
		{
			if (!sensorMap.ContainsKey(id))
			{
				Log.Error("SensorManager.RemoveSensor : not exist !");
				return;
			}
			sensorMap.Remove(id);
		}

		public void EnableSensor(int id)
		{
			if (!sensorMap.ContainsKey(id))
			{
				Log.Error("SensorManager.EnableSensor : not exist !");
				return;
			}
			sensorMap[id].enabled = true;
		}

		public void Update()
		{
			IDictionaryEnumerator iter = sensorMap.GetEnumerator();
			while (iter.MoveNext())
			{
				ISensor sensor = iter.Entry.Value as ISensor;
				if (sensor.enabled)
					sensor.Update();
			}
		}

	}

}