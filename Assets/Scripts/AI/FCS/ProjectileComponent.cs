using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	[RequireComponent(typeof(Collider), typeof(Rigidbody))]
	public class ProjectileComponent : MonoBehaviour
	{
		public float Range;
		public float Velocity;
		private Vector3 _startPosition;
		private float _distanceTravelled;

		public float CurrentLifetime { get; private set; }

		public float MaximumLifetime
		{
			get { return Range / Velocity; }
		}

		public float DistanceTravelled
		{
			get { return _distanceTravelled; }
		}

		protected FiringSolution Solution { get; private set; }

		public Vector3 StartPosition
		{
			get { return _startPosition; }
		}

		public void Shoot(Vector3 position, FiringSolution solution)
		{
			var body = GetComponent<Rigidbody>();
			transform.position = _startPosition = position;
			transform.forward = solution.FiringDirection;
			body.MovePosition(position);
			body.velocity = Velocity * solution.FiringDirection;
			Solution = solution;
		}

		// Update is called once per frame
		protected virtual void Update()
		{
			CurrentLifetime += Time.deltaTime;

			_distanceTravelled = Vector3.Distance(transform.position, _startPosition);

			if (CurrentLifetime >= MaximumLifetime)
				Destroy(gameObject);
		}

		private void OnTriggerEnter(Collider other)
		{
			OnHit(other.gameObject);
		}

		protected virtual void OnHit(GameObject otherGameObject)
		{
			if (otherGameObject != null)
			{
				var rocket = otherGameObject.GetComponent<RocketComponent>();
				if (rocket != null)
					rocket.TakeDamage(this);
			}

			Destroy(gameObject);
		}
	}
}