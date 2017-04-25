
using System.Collections;
using System.Collections.Generic;


namespace Lite.Knowledge
{

	// 存储收集到的世界信息，可以作为全局知识，或者个人知识
	// 全局知识比如：天气、昼夜、人口、资源等等
	// 个人知识比如：距离最近的敌人、最虚弱的队友距离、队友数量
	
	public enum KnowledgeType
	{
		Default,

		Population,

		NearestAgent,
		NearestEnemy,
		NearestTeamate,
	}

	
}