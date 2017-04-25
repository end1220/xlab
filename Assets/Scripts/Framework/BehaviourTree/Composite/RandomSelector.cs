
namespace Lite.BevTree
{
	[AddNodeMenu("Composite/RandomSelector")]
	public class RandomSelector : Composite
	{

		public RandomSelector()
		{

		}


		public RandomSelector(params BehaviourNode[] nodes) :
			base(nodes)
		{

		}


		protected override void OnOpen(Context context)
		{
			int randomIndex = RandomGen.RandInt(0, m_children.Count - 1);
			context.blackboard.SetInt(context.tree.guid, this.guid, "randomIndex", randomIndex);
		}


		protected override RunningStatus OnTick(Context context)
		{
			int randomIndex = context.blackboard.GetInt(context.tree.guid, this.guid, "randomIndex");
			return m_children[randomIndex]._tick(context);
		}

	}

}

