using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Lite
{
	public class AppDefine
	{

		public const int TimerInterval = 1;
		public const int GameFrameRate = 30;

		public const string AppName = "uLab";
		public const string AppPrefix = AppName + "_";

		public static string UserId = string.Empty;                 //用户ID
		public static int SocketPort = 8866;                           //Socket服务器端口
		public static string SocketAddress = "127.0.0.1";          //Socket服务器地址

		// layer
		public static string LayerDefault = "Default";
		public static string LayerTerrain = "Terrain";
		public static string LayerBot = "Bot";

	}
}