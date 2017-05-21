
namespace Lite.Strategy
{
	public enum WorldStateType
	{
		HasTool,
		HasFirewood,
		HasOre,
		HasLogs,
		HasNewTools,
		CollectTools,
		CollectOre,
		CollectLogs,
		CollectFirewood,

		Count
	}

	public enum ActionType
	{
		Default,
		ChopFirewood,
		ChopTree,
		ForgeTool,
		DropOffFirewood,
		DropOffLogs,
		DropOffOre,
		DropOffTools,
		MineOre,
		PickupLogs,
		PickupOre,
		PickupTools,
	}

	public class GoapDefines
	{
		public static readonly int STATE_COUNT = (int)WorldStateType.Count;
	}

	public enum GoalType
	{
		Default,
		MakeFirewood,
		MakeLogs,
		MakeOre,
		MakeTools,
	}

}