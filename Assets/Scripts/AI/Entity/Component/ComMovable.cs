
using UnityEngine;



namespace Lite
{
	
	public class ComMovable : IComponent
	{
		private Steering[] steerings;
		public float maxSpeed = 10;
		public float maxForce = 100;
		protected float sqrMaxSpeed;
		public float mass = 1;
		public Vector3 velocity;
		public float damping = 0.9f;
		public float computeInterval = 0.2f;
		public bool isPlanar = true;

		private Vector3 steeringForce;
		protected Vector3 acceleration;
		private float timer;


		protected override void OnInit()
		{
			steeringForce = new Vector3(0, 0, 0);
			sqrMaxSpeed = maxSpeed * maxSpeed;
			//moveDistance = new Vector3(0,0,0);
			timer = 0;

			steerings = null;
		}


		protected override void OnTick()
		{
			timer += Time.deltaTime;
			steeringForce = new Vector3(0, 0, 0);

			//ticked part, we will not compute force every frame
			if (timer > computeInterval)
			{
				foreach (Steering s in steerings)
				{
					if (s.enabled)
						steeringForce += s.Force() * s.weight;
				}

				steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);
				acceleration = steeringForce / mass;

				timer = 0;
			}
		}


	}

}