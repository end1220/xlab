

namespace Lite
{

	/// <summary>
	/// 杂项
	/// 黑板组件，存放actor组件间共享数据。
	/// 不知道放在哪里，那就放在这里吧。
	/// </summary>
	public class ComBlackboard : IComponent
	{
		public int level;

		public int attack;
		public int defence;
		public int health;
		public int health_Max;
		public int moveVelocity;
		public int moveSpeed;
		public int attackSpeed;


		public int AttackTargetId = 0;
		// 保护目标id
		public int ProtectTargetId = 0;
		// 上次拾取神符时间
		public int lastPickCardTime = 0;

		public bool isMoving = false;
		// 复活中
		public bool isReviving = false;
		// 沉默
		public bool isSilent = false;

		public int dieCount = 0;
		public int DieCount { get { return dieCount; } }

		public int killCount = 0;

		public bool CanDoAction = true;

		public bool IsLocalPlayer = false;



		protected override void OnInit()
		{
			ListenEvents(
				MsgConst.Actor_Die
				);
		}


		protected override void OnEnable()
		{
			isMoving = false;
			isReviving = false;
		
		}


		protected override void OnDestroy()
		{
			
		}


		protected override void OnEvent(int eventId, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6)
		{
			switch (eventId)
			{
				case MsgConst.Actor_Die:
					break;
			}
		}


	}

}