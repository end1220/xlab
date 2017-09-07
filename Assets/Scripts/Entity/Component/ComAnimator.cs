
using System.Collections.Generic;
using UnityEngine;


namespace Lite
{
	/// <summary>
	/// 动画控制组件。抽象基类。
	/// </summary>
	public abstract class ComAnimator : IComponent
	{
		protected enum ResetStyle
		{
			Manually,
			AtBeginning,
			AtEnd
		}

		protected class StateData
		{
			public bool activeCmd = false;
			public bool deactiveCmd = false;
			public string stateName = "none";
			public string stateTag = "none";
			public string[] activeParamNames;
			public bool[] activeParamValues;
			public string[] deactiveParamNames;
			public bool[] deactiveParamValues;
			public ResetStyle resetStyle = ResetStyle.Manually;
			public float speed = 1.0f;
		}

		protected Dictionary<MotionType, StateData> status = new Dictionary<MotionType, StateData>();

		protected Animator animator;

		protected AnimatorStateInfo stateInfo;


		/// <summary>
		/// 子类在这里设置状态控制参数与逻辑
		/// </summary>
		protected abstract void SetupStatusData();


		protected override void OnInit()
		{
			SetupStatusData();
		}


		protected override void OnRenderTick()
		{
			BaseLayer();
		}


		protected void StartPlay(MotionType type, float speed = 1.0f)
		{
			StateData info = null;
			status.TryGetValue(type, out info);
			if (info != null)
			{
				info.activeCmd = true;
				info.speed = speed;
				info.deactiveCmd = false;
			}
		}


		protected void StopPlay(MotionType type)
		{
			StateData info = null;
			status.TryGetValue(type, out info);
			if (info != null)
			{
				info.deactiveCmd = true;
				info.activeCmd = false;
			}
		}


		protected void RefreshSpeed(MotionType motion)
		{
			var mtn = owner.GetComponent<ComMotion>();
			if (mtn.IsInMotion(motion))
			{
				float speed = (float)owner.blackboard.moveSpeed;
				StartPlay(motion, speed);
			}
		}


		private void BaseLayer()
		{
			if (animator == null)
				return;
			
			stateInfo = animator.GetCurrentAnimatorStateInfo(0);

			var itor = status.GetEnumerator();
			while (itor.MoveNext())
			{
				StateData cfg = itor.Current.Value as StateData;
				bool isCurrentState = stateInfo.IsName(cfg.stateName);
				bool isCurrentTag = stateInfo.IsTag(cfg.stateTag);
				if (cfg.activeCmd)
				{
					cfg.activeCmd = false;
					//if (!isCurrentState && !isCurrentTag)
					ApplyActiveParamValues(cfg.activeParamNames, cfg.activeParamValues);
				}
				if (cfg.deactiveCmd)
				{
					cfg.deactiveCmd = false;
					ApplyDeactiveParamValues(cfg.deactiveParamNames, cfg.deactiveParamValues);
				}
				// modify animator speed if necessary.
				if ((isCurrentState || isCurrentTag) && !Mathf.Approximately(cfg.speed, animator.speed))
					ModifyAnimatorSpeed((MotionType)itor.Current.Key, cfg, stateInfo);

				// check reset to avoid looping
				if (!animator.IsInTransition(0))
				{
					switch (cfg.resetStyle)
					{
						case ResetStyle.Manually:
							break;
						case ResetStyle.AtBeginning:
							if (isCurrentState || isCurrentTag)
								ApplyDeactiveParamValues(cfg.deactiveParamNames, cfg.deactiveParamValues);
							break;
						case ResetStyle.AtEnd:
							if ((isCurrentState || isCurrentTag) && stateInfo.normalizedTime > 0.7f && !cfg.activeCmd)
								ApplyDeactiveParamValues(cfg.deactiveParamNames, cfg.deactiveParamValues);
							break;
					}
				}
			}

		}


		private void ApplyActiveParamValues(string[] names, bool[] values)
		{
			if (names == null || values == null)
				return;
			for (int i = 0; i < names.Length; ++i)
			{
				animator.SetBool(names[i], (bool)values[i]);
			}
		}


		private void ApplyDeactiveParamValues(string[] names, bool[] values)
		{
			if (names == null || values == null)
				return;
			for (int i = 0; i < names.Length; ++i)
			{
				/*Type tp = values[i].GetType();
				if (tp == typeof(bool))*/
				animator.SetBool(names[i], (bool)values[i]);
			}
		}


		private void ModifyAnimatorSpeed(MotionType action, StateData cfg, AnimatorStateInfo stateInfo)
		{
			animator.speed = cfg.speed;
		}
		

	}

}