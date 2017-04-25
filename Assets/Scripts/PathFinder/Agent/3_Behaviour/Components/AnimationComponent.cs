using UnityEngine;
using System.Collections;


namespace Lite.Bev
{

	public class AnimationComponent : IComponent
	{
		private Animator animator;

		private string currentAnimation = "";

		public Agent agent;

		public void Init(Agent agent)
		{
			this.agent = agent;
		}

		public override void OnStart()
		{
			animator = GetComponentInChildren<Animator>();
		}

		public override void OnUpdate()
		{
		}

		public float moveSpeed
		{
			set { animator.SetFloat("moveSpeed", value); }
			get { return animator.GetFloat("moveSpeed"); }
		}

		public bool attack1
		{
			set { animator.SetBool("attack1", value); }
			get { return animator.GetBool("attack1"); }
		}

	}

}
