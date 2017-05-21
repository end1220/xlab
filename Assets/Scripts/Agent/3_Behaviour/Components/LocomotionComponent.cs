
using UnityEngine;


namespace Lite
{
	public class LocomotionComponent : IComponent
	{
		// configed members
		public float mass;
		public float maxWalkSpeed;
		public float maxRunSpeed;
		public float maxSprintSpeed;

		// dynamic members
		public Vector3 position { get { return transform.position; } }
		public Vector3 forward { get { return transform.forward; } }
		[System.NonSerialized]
		public Vector3 velocity = Vector3.zero;
		[System.NonSerialized]
		public float speed;
		[System.NonSerialized]
		public Vector3 targetPosition = Vector3.zero;
		private float updateVelocityTime;


		// constants
		public const bool isPlanar = true;
		public const float damping = 5f;
		
		// unity components
		private CharacterController controller;
		private Rigidbody theRigidbody;

		
		public override void OnAwake()
		{
			mass = 1;
			maxWalkSpeed = 1;
			maxRunSpeed = 2;
			maxSprintSpeed = 4;
		}

		public void OnStart()
		{
			controller = GetComponent<CharacterController>();
			theRigidbody = GetComponent<Rigidbody>();
		}

		public override void OnUpdate()
		{
			UpdateMovement();
		}

		public void SetPosition(Vector3 pos)
		{
			transform.position = pos;
		}

		public void SetForward(Vector3 dir)
		{
			transform.forward = dir;
		}

		public void StartMove(Vector3 target, float speed = -1)
		{
			if (speed > 0)
				this.speed = speed;
			targetPosition = target;
			UpdateVelocity();
			updateVelocityTime = GameTimer.timeSinceLevelLoad + 0.2f;
		}

		public void StopMove()
		{
			speed = 0;
			velocity = Vector3.zero;
		}

		private void UpdateMovement()
		{
			if (this.speed > 0.00001f)
			{
				float deltaTime = GameTimer.deltaTime;

				float distanceSqr = MathUtil.DistanceSqr2D(targetPosition, transform.position);
				if (distanceSqr < speed * speed * deltaTime)
				{
					SetPosition(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
					StopMove();
				}
				else
				{
					if (GameTimer.timeSinceLevelLoad > updateVelocityTime)
					{
						UpdateVelocity();
						updateVelocityTime = Time.timeSinceLevelLoad + 0.5f;
					}

					Vector3 moveDistance = this.velocity * deltaTime;
					if (controller != null)
					{
						controller.SimpleMove(this.velocity);
					}
					else if (theRigidbody == null || theRigidbody.isKinematic)
					{
						transform.position += moveDistance;
					}
					else
					{
						theRigidbody.MovePosition(theRigidbody.position + moveDistance);
					}
				}
			
				// rotating
				Vector3 newForward = Vector3.Slerp(transform.forward, this.velocity, damping * GameTimer.deltaTime * (speed / maxWalkSpeed));
				this.SetForward(newForward);
			}

		}

		private void UpdateVelocity()
		{
			velocity = Vector3.Normalize(targetPosition - position) * speed;
			if (isPlanar)
				velocity.Set(velocity.x, 0, velocity.z);
		}


	}

}