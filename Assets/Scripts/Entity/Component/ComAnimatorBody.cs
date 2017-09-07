

using UnityEngine;


namespace Lite
{
	/// <summary>
	/// 角色身体的动画控制组件。用来控制角色身体动作。
	/// </summary>
	public class ComAnimatorBody : ComAnimator
	{
		
		private int attackIndex = 0;
		private int attackRandom = 0;
		private int skillIndex = 0;

		protected int AttackIndex
		{
			set
			{
				if (attackIndex != value)
				{
					attackIndex = value;
					if (animator != null)
						animator.SetInteger("attack_index", value);
				}
			}
		}

		protected int AttackRandom
		{
			set
			{
				if (attackRandom != value)
				{
					attackRandom = value;
					if (animator != null)
						animator.SetInteger("attack_random", value);
				}
			}
			get
			{
				return attackRandom;
			}
		}

		protected int SkillIndex
		{
			set
			{
				if (skillIndex != value)
				{
					skillIndex = value;
					if (animator != null)
						animator.SetInteger("skill_index", value);
				}
			}
		}

		protected override void OnInit()
		{
			base.OnInit();

			ListenEvents(
				MsgConst.Actor_Die,
				MsgConst.Actor_Revive,
				MsgConst.Actor_MoveBegin,
				MsgConst.Actor_MoveStop,
				MsgConst.Actor_DizzyBegin,
				MsgConst.Actor_DizzyEnd,
				MsgConst.Actor_LevitateBegin,
				MsgConst.Actor_LevitateEnd,
				MsgConst.Actor_SpeedChanged,
				MsgConst.Actor_AttackBegin,
				MsgConst.Actor_AttackEnd,
				MsgConst.Actor_SingBegin,
				MsgConst.Actor_SingEnd,
				MsgConst.Actor_PreBegin,
				MsgConst.Actor_PreEnd,
				MsgConst.Actor_ReleaseBegin,
				MsgConst.Actor_ReleaseEnd,
				MsgConst.Actor_PostBegin,
				MsgConst.Actor_PostEnd,
				MsgConst.Actor_SkillEnd
				);

		}


		protected override void OnEnable()
		{
			if (animator == null)
			{
				/*var ae = owner.actor.GetComponentInChildren<BodyFrameEventReceiver>();
				if (ae != null)
					animator = ae.GetComponent<Animator>();
				else
					Log.Error("ComAnimatorBody.OnInit: cannot find AnimationEventReceiver! Actor id " + owner.actorTemplate.Id);*/
			}
			StartPlay(MotionType.Idle);
		}


		protected override void OnDisable()
		{
			StartPlay(MotionType.Idle);
		}


		protected override void SetupStatusData()
		{
			StateData cfg = null;

			cfg = new StateData();
			status.Add(MotionType.Idle, cfg);
			cfg.stateName = "idle";
			cfg.activeParamNames = new string[] { "move", "dead", "dizzy", "attack",
				"skill", "skill_sing", "skill_release", "skill_post" };
			cfg.activeParamValues = new bool[]{ false, false, false, false,
				false, false, false, false };
			cfg.deactiveParamNames = new string[] { };
			cfg.deactiveParamValues = new bool[] { };
			cfg.resetStyle = ResetStyle.Manually;

			cfg = new StateData();
			status.Add(MotionType.Move, cfg);
			cfg.stateName = "move";
			cfg.activeParamNames = new string[] { "move", "attack", "skill" };
			cfg.activeParamValues = new bool[] { true, false, false };
			cfg.deactiveParamNames = new string[] { "move" };
			cfg.deactiveParamValues = new bool[] { false };
			cfg.resetStyle = ResetStyle.Manually;

			cfg = new StateData();
			status.Add(MotionType.Die, cfg);
			cfg.stateName = "die";
			cfg.activeParamNames = new string[] { "dead", "move", "attack", "skill" };
			cfg.activeParamValues = new bool[] { true, false, false, false };
			cfg.deactiveParamNames = new string[] { "dead" };
			cfg.deactiveParamValues = new bool[] { false };
			cfg.resetStyle = ResetStyle.Manually;

			cfg = new StateData();
			status.Add(MotionType.Dizzy, cfg);
			cfg.stateName = "dizzy";
			cfg.activeParamNames = new string[] { "dizzy", "attack", "skill" };
			cfg.activeParamValues = new bool[] { true, false, false };
			cfg.deactiveParamNames = new string[] { "dizzy" };
			cfg.deactiveParamValues = new bool[] { false };
			cfg.resetStyle = ResetStyle.Manually;

			cfg = new StateData();
			status.Add(MotionType.Levitate, cfg);
			cfg.stateName = "levitate";
			cfg.activeParamNames = new string[] { "levitate", "attack", "skill" };
			cfg.activeParamValues = new bool[] { true, false, false };
			cfg.deactiveParamNames = new string[] { "levitate" };
			cfg.deactiveParamValues = new bool[] { false };
			cfg.resetStyle = ResetStyle.Manually;

			cfg = new StateData();
			status.Add(MotionType.Attack, cfg);
			cfg.stateTag = "attack";
			cfg.activeParamNames = new string[] { "attack" };
			cfg.activeParamValues = new bool[] { true };
			cfg.deactiveParamNames = new string[] { "attack" };
			cfg.deactiveParamValues = new bool[] { false };
			cfg.resetStyle = ResetStyle.AtEnd;

			cfg = new StateData();
			status.Add(MotionType.Skill_Sing, cfg);
			cfg.stateTag = "skill_sing";
			cfg.activeParamNames = new string[] { "skill", "skill_sing" };
			cfg.activeParamValues = new bool[] { true, true };
			cfg.deactiveParamNames = new string[] { "skill_sing" };
			cfg.deactiveParamValues = new bool[] { false };
			cfg.resetStyle = ResetStyle.Manually;

			cfg = new StateData();
			status.Add(MotionType.Skill_Pre, cfg);
			cfg.stateTag = "skill_pre";
			cfg.activeParamNames = new string[] { "skill", "skill_pre" };
			cfg.activeParamValues = new bool[] { true, true };
			cfg.deactiveParamNames = new string[] { "skill_pre" };
			cfg.deactiveParamValues = new bool[] { false };
			cfg.resetStyle = ResetStyle.Manually;

			cfg = new StateData();
			status.Add(MotionType.Skill_Release, cfg);
			cfg.stateTag = "skill_release";
			cfg.activeParamNames = new string[] { "skill", "skill_release" };
			cfg.activeParamValues = new bool[] { true, true };
			cfg.deactiveParamNames = new string[] { "skill_release" };
			cfg.deactiveParamValues = new bool[] { false };
			cfg.resetStyle = ResetStyle.Manually;

			cfg = new StateData();
			status.Add(MotionType.Skill_Post, cfg);
			cfg.stateTag = "skill_post";
			cfg.activeParamNames = new string[] { "skill", "skill_post" };
			cfg.activeParamValues = new bool[] { true, true };
			cfg.deactiveParamNames = new string[] { "skill_post" };
			cfg.deactiveParamValues = new bool[] { false };
			cfg.resetStyle = ResetStyle.Manually;

			cfg = new StateData();
			status.Add(MotionType.Skill_End, cfg);
			cfg.stateTag = "skill_post";
			cfg.activeParamNames = new string[] { "skill" };
			cfg.activeParamValues = new bool[] { false };
			cfg.deactiveParamNames = new string[] { };
			cfg.deactiveParamValues = new bool[] { };
			cfg.resetStyle = ResetStyle.Manually;

		}


		protected override void OnEvent(int eventId, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6)
		{
			switch (eventId)
			{
				case MsgConst.Actor_Die:
					StartPlay(MotionType.Die);
					break;
				case MsgConst.Actor_Revive:
					StopPlay(MotionType.Die);
					break;
				case MsgConst.Actor_MoveBegin:
					StartPlay(MotionType.Move, arg1 * 0.001f);
					break;
				case MsgConst.Actor_MoveStop:
					StopPlay(MotionType.Move);
					break;
				case MsgConst.Actor_DizzyBegin:
					StartPlay(MotionType.Dizzy);
					break;
				case MsgConst.Actor_DizzyEnd:
					StopPlay(MotionType.Dizzy);
					break;
				case MsgConst.Actor_LevitateBegin:
					StartPlay(MotionType.Dizzy);
					break;
				case MsgConst.Actor_LevitateEnd:
					StopPlay(MotionType.Dizzy);
					break;
				case MsgConst.Actor_SpeedChanged:
					RefreshSpeed((MotionType)arg1);
					break;
				case MsgConst.Actor_AttackBegin:
					AttackIndex = arg1;
					AttackRandom = AttackRandom % arg2 + 1;
					StartPlay(MotionType.Attack, arg3 * 0.001f);
					break;
				case MsgConst.Actor_AttackEnd:
					StopPlay(MotionType.Attack);
					break;
				case MsgConst.Actor_SingBegin:
					SkillIndex = arg1;
					StartPlay(MotionType.Skill_Sing);
					break;
				case MsgConst.Actor_SingEnd:
					StopPlay(MotionType.Skill_Sing);
					break;
				case MsgConst.Actor_PreBegin:
					SkillIndex = arg1;
					StartPlay(MotionType.Skill_Pre);
					break;
				case MsgConst.Actor_PreEnd:
					StopPlay(MotionType.Skill_Pre);
					break;
				case MsgConst.Actor_ReleaseBegin:
					SkillIndex = arg1;
					StartPlay(MotionType.Skill_Release);
					break;
				case MsgConst.Actor_ReleaseEnd:
					StopPlay(MotionType.Skill_Release);
					break;
				case MsgConst.Actor_PostBegin:
					SkillIndex = arg1;
					StartPlay(MotionType.Skill_Post);
					break;
				case MsgConst.Actor_PostEnd:
					StopPlay(MotionType.Skill_Post);
					break;
				case MsgConst.Actor_SkillEnd:
					StartPlay(MotionType.Skill_End);
					break;
			}
		}


	}
}