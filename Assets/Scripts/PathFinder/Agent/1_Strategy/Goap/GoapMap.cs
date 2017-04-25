

using Lite.Goap;


namespace Lite.Strategy
{
	public class GoapMap : GoapAStarMap
	{
		public GoapMap(int stateCount):
			base(stateCount)
		{

		}

		public override void BuildActionTable(IAgent agnt)
		{
			Agent agent = agnt as Agent;
			if (agent.career == Strategy.Career.Miner)
			{
				AddAction(new PickUpTool(agent));
				AddAction(new MineOre(agent));
				AddAction(new DropOffOre(agent));
			}
			else if (agent.career == Strategy.Career.Logger)
			{
				AddAction(new ChopTree(agent));
				AddAction(new DropOffLogs(agent));
				AddAction(new PickUpTool(agent));
			}
			else if (agent.career == Strategy.Career.WoodCutter)
			{
				AddAction(new ChopFirewood(agent));
				AddAction(new DropOffFirewood(agent));
				AddAction(new PickUpTool(agent));
			}
			else if (agent.career == Strategy.Career.Blacksmith)
			{
				AddAction(new ForgeTool(agent));
				AddAction(new DropOffTools(agent));
				AddAction(new PickUpOre(agent));
			}
		}

	}
}

