


namespace Lite
{
	public class Log
	{
		public static void Info(string text)
		{
			UnityEngine.Debug.Log(text);
		}

		public static void Warning(string text)
		{
			UnityEngine.Debug.LogWarning(text);
		}

		public static void Error(string text)
		{
			UnityEngine.Debug.LogError(text);
		}
	}

}
