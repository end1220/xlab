
using UnityEngine;



namespace Lite
{
	/// <summary>
	/// 匀速直线移动到目标位置
	/// </summary>
	public class ComMoving : IComponent
	{
		private int speed;
		private Int3 direction = Int3.zero;
		private int directionLength = 0;
		private int targetDistance = 0;
		private Int3 startPos = Int3.zero;
		private Int3 targetPos = Int3.zero;
		private int frameCount = 0;
		private bool isDirectionalMoving = false;

		private int flySpeed = 0;
		private long flyCenterCount = 0;
		private long flyResultCount = 0;
		public bool isJumping = false;
		private int flyHeight = 0;
		public int originalHeght = 0;


		protected override void OnInit()
		{
			ListenEvents(MsgConst.Actor_MoveStop);
		}


		protected override void OnFrameTick()
		{
			UpdateDirectionalMoving();
		}


		protected override void OnEvent(int eventId, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6)
		{
			switch (eventId)
			{
				case MsgConst.Actor_MoveStop:
					Stop();
					break;
			}
		}


		public void MoveTo(Int3 position, int speed)
		{
			if (position.x == owner.transform.position.x && position.z == owner.transform.position.z)
				return;

			isDirectionalMoving = true;
			this.speed = speed;
			targetPos = position;
			direction = targetPos - owner.transform.position;
			targetDistance = direction.magnitude;
			direction = direction * 10000 / targetDistance;
			directionLength = direction.magnitude;
			startPos = owner.transform.position;
			frameCount = 0;
		}


		public void Stop()
		{
			isDirectionalMoving = false;
		}


		private void UpdateDirectionalMoving()
		{
			if (isDirectionalMoving)
			{
				frameCount++;

				/*var deltaDist = (Fix64)(speed * AppConst.FrameSyncInterval * frameCount) / (Fix64)1000;
				Fix64 scale = deltaDist / (Fix64)directionLength;
				int x = (int)((Fix64)(direction.x) * scale);
				int y = (int)((Fix64)(direction.y) * scale);
				int z = (int)((Fix64)(direction.z) * scale);
				Int3 deltaPos = new Int3(x, y, z);

				Int3 nextFramePos = startPos + deltaPos;
				if (deltaPos.magnitude >= targetDistance)
					nextFramePos = targetPos;

				owner.SetPosition(nextFramePos);
				CheckStepOnGround();

				if (nextFramePos.x == targetPos.x && nextFramePos.z == targetPos.z)
					Stop();*/
			}
		}


		

	}

}