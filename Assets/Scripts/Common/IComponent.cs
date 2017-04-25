
using UnityEngine;

namespace Lite
{
	public abstract class IComponent : MonoBehaviour
	{
		private void Awake()
		{
			OnAwake();
		}

		private void Start()
		{
			OnStart();
		}

		private void Update()
		{
			OnUpdate();
		}

		public virtual void OnAwake()
		{

		}

		public virtual void OnStart()
		{

		}

		public virtual void OnUpdate()
		{

		}


	}

}