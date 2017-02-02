using UnityEngine;

namespace Assets.Scripts.AI
{
	[RequireComponent(typeof(Rigidbody))]
	public class EngineSystem
		: MonoBehaviour
	{
		private GameObject _engine;

		private Rigidbody _body;
		private bool _isFiringMainEngine;
		private float _angularVelocity;

		// Use this for initialization
		private void Start()
		{
			_engine = transform.FindChild("Engine").gameObject;
			_body = GetComponent<Rigidbody>();
		}

		// Update is called once per frame
		private void Update()
		{
			_velocity = _body.velocity;
		}

		public bool IsFiringMainEngine
		{
			get { return _isFiringMainEngine; }
		}

		public Vector3 CurrentVelocity
		{
			get { return _velocity; }
		}

		public Vector3 MovementDirection
		{
			get
			{
				var velocity = _body.velocity;
				var length = velocity.magnitude;
				if (length <= 0.001)
					return Vector3.zero;

				return velocity/length;
			}
		}

		/// <summary>
		/// 10m/s²
		/// </summary>
		public float MaximumAcceleration = 10;

		/// <summary>
		/// 2m/s²
		/// </summary>
		public float MaximumThrusterAcceleration = 2;

		/// <summary>
		/// 10 deg/s
		/// </summary>
		public float MaximumAngularVelocity = 10;

		private Vector3 _velocity;

		public void Burn(EngineType engineType, float deltaVelocity)
		{
			var requiredAcceleration = deltaVelocity / Time.deltaTime;
			float maximumAcceleration;
			Vector3 direction;

			switch (engineType)
			{
				case EngineType.Main:
					maximumAcceleration = MaximumAcceleration;
					direction = transform.forward;
					_isFiringMainEngine = true;
					break;

				case EngineType.BackwardsThrusters:
					maximumAcceleration = 2;
					direction = -transform.forward;
					break;

				default:
					Debug.LogWarningFormat("Unknown engine type: {0}", engineType);
					return;
			}

			float currentAcceleration = Mathf.Clamp(requiredAcceleration, 0, maximumAcceleration);
			var force = direction * currentAcceleration;
			_body.AddForce(force);

			UpdateEngine();
		}

		public void Stop()
		{
			_isFiringMainEngine = false;
			UpdateEngine();
		}

		private void UpdateEngine()
		{
			_engine.SetActive(_isFiringMainEngine);
		}

		/// <summary>
		/// </summary>
		/// <param name="targetDirection"></param>
		/// <param name="angle"></param>
		public void OrientShipTowards(Vector3 targetDirection, float angle)
		{
			var current = transform.forward;
			//var delta = targetDirection - current;
			var axis = Vector3.Cross(current, targetDirection);
			var currentVelocity = Mathf.Rad2Deg * Vector3.Project(_body.angularVelocity, axis).magnitude;

			var change = Mathf.Clamp(MaximumAngularVelocity * Time.deltaTime, 0, angle);
			var torque = axis * change;

			var timeToTarget = angle / currentVelocity;
			var timeToZero = currentVelocity / MaximumAngularVelocity;

			if (timeToZero > timeToTarget)
			{
				// Slow down
				torque *= -1;
			}
			else
			{
				// Slow up
			}

			Debug.DrawRay(transform.position, torque, Color.cyan);

			_body.AddTorque(torque);
		}

		public void RotateShip(RotationDirection direction)
		{
			if (direction == RotationDirection.None)
				return;

			var sign = direction == RotationDirection.Left ? 1 : -1;
			var angularChange = sign * MaximumAngularVelocity * Time.deltaTime;
			transform.Rotate(Vector3.forward, angularChange, Space.Self);
		}
	}
}