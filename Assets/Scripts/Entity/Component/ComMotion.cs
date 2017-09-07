


namespace Lite
{
	public enum MotionType
	{
		Idle = 1 << 0,
		Move = 1 << 1,
		Die = 1 << 2,
		Dizzy = 1 << 3,
		Attack = 1 << 4,

		Skill_Sing = 1 << 5,
		Skill_Pre = 1 << 6,
		Skill_Release = 1 << 7,
		Skill_Post = 1 << 8,
		Skill_End = 1 << 9,

		Taunt = 1 << 10,
		Levitate = 1 << 11,
		Revive = 1 << 12,
	}


	public class ComMotion : IComponent
	{
		protected uint currentMotion = (uint)MotionType.Idle;


		protected override void OnInit()
		{
			ListenEvents(
				MsgConst.Actor_Die,
				MsgConst.Actor_Revive,
				MsgConst.Actor_MoveBegin,
				MsgConst.Actor_MoveStop,
				MsgConst.Actor_DizzyBegin,
				MsgConst.Actor_DizzyEnd,
				MsgConst.Actor_LevitateBegin,
				MsgConst.Actor_LevitateEnd,
				MsgConst.Actor_AttackBegin,
				MsgConst.Actor_AttackEnd,
				MsgConst.Actor_SingBegin,
				MsgConst.Actor_SingEnd,
				MsgConst.Actor_PreBegin,
				MsgConst.Actor_PreEnd,
				MsgConst.Actor_ReleaseBegin,
				MsgConst.Actor_ReleaseEnd,
				MsgConst.Actor_PostBegin,
				MsgConst.Actor_PostEnd
				);
		}


		public void ApplyMotion(MotionType motion)
		{
			uint m = (uint)motion;
			currentMotion |= m;
		}


		public void CancelMotion(MotionType motion)
		{
			if (IsInMotion(motion))
			{
				uint m = (uint)motion;
				currentMotion ^= m;
			}
		}


		public bool IsInMotion(MotionType motion)
		{
			uint m = (uint)motion;
			return (currentMotion & m) > 0;
		}


		protected override void OnEvent(int eventId, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6)
		{
			switch (eventId)
			{
				case MsgConst.Actor_Die:
					ApplyMotion(MotionType.Die);
					break;
				case MsgConst.Actor_Revive:
					CancelMotion(MotionType.Die);
					break;
				case MsgConst.Actor_MoveBegin:
					ApplyMotion(MotionType.Move);
					break;
				case MsgConst.Actor_MoveStop:
					CancelMotion(MotionType.Move);
					break;
				case MsgConst.Actor_DizzyBegin:
					ApplyMotion(MotionType.Dizzy);
					break;
				case MsgConst.Actor_DizzyEnd:
					CancelMotion(MotionType.Dizzy);
					break;
				case MsgConst.Actor_LevitateBegin:
					ApplyMotion(MotionType.Levitate);
					break;
				case MsgConst.Actor_LevitateEnd:
					CancelMotion(MotionType.Levitate);
					break;
				case MsgConst.Actor_TauntBegin:
					ApplyMotion(MotionType.Taunt);
					break;
				case MsgConst.Actor_TauntEnd:
					CancelMotion(MotionType.Taunt);
					break;
				case MsgConst.Actor_AttackBegin:
					ApplyMotion(MotionType.Attack);
					break;
				case MsgConst.Actor_AttackEnd:
					CancelMotion(MotionType.Attack);
					break;
				case MsgConst.Actor_SingBegin:
					ApplyMotion(MotionType.Skill_Sing);
					break;
				case MsgConst.Actor_SingEnd:
					CancelMotion(MotionType.Skill_Sing);
					break;
				case MsgConst.Actor_PreBegin:
					ApplyMotion(MotionType.Skill_Pre);
					break;
				case MsgConst.Actor_PreEnd:
					CancelMotion(MotionType.Skill_Pre);
					break;
				case MsgConst.Actor_ReleaseBegin:
					ApplyMotion(MotionType.Skill_Release);
					break;
				case MsgConst.Actor_ReleaseEnd:
					CancelMotion(MotionType.Skill_Release);
					break;
				case MsgConst.Actor_PostBegin:
					ApplyMotion(MotionType.Skill_Post);
					break;
				case MsgConst.Actor_PostEnd:
					CancelMotion(MotionType.Skill_Post);
					break;
				case MsgConst.Actor_SkillEnd:
					ApplyMotion(MotionType.Skill_End);
					break;
			}
		}


	}

}