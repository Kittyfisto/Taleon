using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	[RequireComponent(typeof(Rigidbody), typeof(Collider))]
	public class RocketComponent : MonoBehaviour
	{
		private Rigidbody _body;
		private float _lifeTime;

		/// <summary>
		/// 
		/// </summary>
		public GameObject ExplosionPrefab;

		/// <summary>
		/// The maximum angle the nozzle can be rotated at.
		/// </summary>
		public float MaximumNozzleGimbalAngle;

		/// <summary>
		///     The acceleration of this rocket in units per s².
		/// </summary>
		public float MaximumAcceleration;

		/// <summary>
		///     The amount of time it takes after launch until the rocket is activated.
		/// </summary>
		public float ActivationTime;

		/// <summary>
		///     The amount of time this rockets can burn before all fuel is gone.
		/// </summary>
		public float Burntime;

		/// <summary>
		///     The amount of velocity this rocket starts with.
		/// </summary>
		public float InitialVelocity;
		
		/// <summary>
		///     The amount of time this rocket lifes until its destroyed.
		/// </summary>
		/// <remarks>
		///     Should be greater or equal to <see cref="Burntime" />.
		/// </remarks>
		public float Lifetime;

		public float CurrentAcceleration
		{
			get { return _currentAcceleration; }
		}

		public bool IsActivated
		{
			get { return _isActivated; }
		}

		public GameObject target;
		private bool _isActivated;
		private Collider _collider;
		private GameObject _engine;
		private float _lastDistance;
		private float _currentAcceleration;

		// Use this for initialization
		private void Start()
		{
			_body = GetComponent<Rigidbody>();
			_collider = GetComponent<Collider>();
			_collider.enabled = false;
			_engine = transform.FindChild("Engine").gameObject;
			
			_body.velocity = transform.forward * InitialVelocity;
			_lastDistance = Vector3.Distance(transform.position, target.transform.position);
		}

		// Update is called once per frame
		private void Update()
		{
			_lifeTime += Time.deltaTime;

			if (_lifeTime >= ActivationTime && !_isActivated)
			{
				ActivateEngine();
			}

			Vector3 targetDirection;
			float acceleration;
			var direction = CalculateEngineSolution(out targetDirection, out acceleration);
			if (_isActivated)
			{
				if (_lifeTime < Burntime && target != null)
				{
					BurnEngine(direction);
					
					Debug.DrawRay(transform.position, targetDirection, Color.green);
					Debug.DrawRay(transform.position, direction, Color.red);
					Debug.DrawRay(transform.position, _body.velocity.normalized, Color.blue);
				}
				else
				{
					StopEngine();
				}
			}

			transform.LookAt(transform.position + _body.velocity.normalized);

			if (_lifeTime >= Lifetime)
				Destroy(gameObject);
		}

		private void StopEngine()
		{
			_engine.SetActive(false);
			_currentAcceleration = 0;
		}

		private void ActivateEngine()
		{
			_isActivated = true;
			_engine.SetActive(true);
			_collider.enabled = true;
		}

		private void BurnEngine(Vector3 direction)
		{
			_currentAcceleration = MaximumAcceleration;
			var force = _body.mass * _currentAcceleration;
			_body.AddForce(direction * force);
		}

		private Vector3 CalculateEngineSolution(out Vector3 targetDirection, out float acceleration)
		{
			if (target == null)
			{
				targetDirection = Vector3.zero;
				acceleration = 0;
				return Vector3.zero;
			}

			// TODO: Find projected position of target based on its current acceleration
			var delta = target.transform.position - transform.position;
			targetDirection = delta.normalized;

			// When the movement direction of this rocket does not align with the direction to the target,
			// then our rocket would naturally overshoot. Therefore we deliberately change the direction we point
			// our engine at.
			var movementDirection = _body.velocity.normalized;

			var error = targetDirection - movementDirection;

			Vector3 accelerationDirection;
			if (error != Vector3.zero)
			{
				var idealAccelerationDirection = targetDirection + error;
				idealAccelerationDirection = idealAccelerationDirection.normalized;

				// Ideally, we would burn exactly into this direction, however we're constrained
				// by our engine: Its nozzle can only gimbal so much.
				// Therefore we clamp the direction to the maximum gimbal angle of the rocket.
				// (The greater this angle, the more sharp turns the rocket can perform).
				var forward = transform.forward;
				var angle = Vector3.Angle(idealAccelerationDirection, forward);

				if (angle > MaximumNozzleGimbalAngle)
				{
					var maximumAngle = Mathf.Clamp(angle, 0, MaximumNozzleGimbalAngle);
					accelerationDirection = Vector3.RotateTowards(forward, idealAccelerationDirection, maximumAngle * Mathf.Deg2Rad, 1);

					// we cannot accelerate with full thrust when we're pointed at the wrong direction
					// and therefore we turn with a very limited amount f acceleration until
					// we're roughly pointed in the right direction...
					acceleration = 0.1f * MaximumAcceleration;
				}
				else
				{
					accelerationDirection = idealAccelerationDirection;
					acceleration = MaximumAcceleration;
				}
			}
			else
			{
				acceleration = MaximumAcceleration;
				accelerationDirection = movementDirection;
			}

			return accelerationDirection;
		}

		private void OnTriggerEnter(Collider other)
		{
			var otherGo = other.gameObject;
			var rocket = otherGo.GetComponent<RocketComponent>();
			var projectile = otherGo.GetComponent<ProjectileComponent>();
			if (rocket == null && projectile == null)
			{
				Explode();
			}
		}

		public void TakeDamage(ProjectileComponent other)
		{
			Explode();
		}

		/// <summary>
		/// Is called when the rocket successfully intercepts the target .
		/// </summary>
		private void Explode()
		{
			var explosion = Instantiate(ExplosionPrefab);
			explosion.transform.position = transform.position;

			Destroy(gameObject);
		}
	}
}