using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	[RequireComponent(typeof(Collider), typeof(Rigidbody))]
	public class ProjectileComponent : MonoBehaviour
	{
		private float _currentLifetime;
		private float _lifetime;

		public float Range;
		public float Velocity;

		protected FiringSolution Solution { get; private set; }

		public Vector3 StartPosition { get; private set; }

		private void Start()
		{
			_lifetime = Range / Velocity;
		}

		public void Shoot(Vector3 position, FiringSolution solution)
		{
			var body = GetComponent<Rigidbody>();
			transform.position = StartPosition = position;
			transform.forward = solution.FiringDirection;
			body.MovePosition(position);
			body.velocity = Velocity * solution.FiringDirection;
			Solution = solution;
		}

		// Update is called once per frame
		protected virtual void Update()
		{
			_currentLifetime += Time.deltaTime;

			if (_currentLifetime >= _lifetime)
				Destroy(gameObject);
		}

		private void OnTriggerEnter(Collider other)
		{
			Explode(other.gameObject);
		}

		protected virtual void Explode(GameObject otherGameObject)
		{
			Destroy(gameObject);
		}
	}
}