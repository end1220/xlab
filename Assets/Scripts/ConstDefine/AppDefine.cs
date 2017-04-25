using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Lite
{
	public class AppDefine
	{
		public static bool LuaBundleMode = false;                    //True:��bundle�м���lua, false:ֱ�Ӷ�lua�ļ�

		public static bool UpdateMode = false;                       //����ģʽ-Ĭ�Ϲر� 

		public static bool LuaByteMode = false;                       //Lua�ֽ���ģʽ-Ĭ�Ϲر� 

		public const int TimerInterval = 1;
		public const int GameFrameRate = 30;                        //��Ϸ֡Ƶ

		public const string AppName = "uLab";               //Ӧ�ó�������
		public const string AppPrefix = AppName + "_";              //Ӧ�ó���ǰ׺
		public const string ExtName = ".unity3d";                   //�ز���չ��
		public const string StreamingAssetDir = "StreamingAssets";           //�ز�Ŀ¼ 
		public const string WebUrl = "http://192.168.1.28:6688/";      //���Ը��µ�ַ

		public static string UserId = string.Empty;                 //�û�ID
		public static int SocketPort = 8866;                           //Socket�������˿�
		public static string SocketAddress = "127.0.0.1";          //Socket��������ַ

		public const string ManagerGOName = "Managers";				// �����ֹ�����ʹ�õ�go


		// layer
		public static string LayerDefault = "Default";
		public static string LayerTerrain = "Terrain";
		public static string LayerBot = "Bot";

	}
}