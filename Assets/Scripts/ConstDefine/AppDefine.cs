using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Lite
{
	public class AppDefine
	{
		public static bool LuaBundleMode = false;                    //True:从bundle中加载lua, false:直接读lua文件

		public static bool UpdateMode = false;                       //更新模式-默认关闭 

		public static bool LuaByteMode = false;                       //Lua字节码模式-默认关闭 

		public const int TimerInterval = 1;
		public const int GameFrameRate = 30;                        //游戏帧频

		public const string AppName = "uLab";               //应用程序名称
		public const string AppPrefix = AppName + "_";              //应用程序前缀
		public const string ExtName = ".unity3d";                   //素材扩展名
		public const string StreamingAssetDir = "StreamingAssets";           //素材目录 
		public const string WebUrl = "http://192.168.1.28:6688/";      //测试更新地址

		public static string UserId = string.Empty;                 //用户ID
		public static int SocketPort = 8866;                           //Socket服务器端口
		public static string SocketAddress = "127.0.0.1";          //Socket服务器地址

		public const string ManagerGOName = "Managers";				// 给各种管理器使用的go


		// layer
		public static string LayerDefault = "Default";
		public static string LayerTerrain = "Terrain";
		public static string LayerBot = "Bot";

	}
}