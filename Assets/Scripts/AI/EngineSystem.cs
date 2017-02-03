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

		private bool _fireThrustersForLateralMovement;
		private Vector3 _lateralThrusterDirection;

		private bool _fireThrustersForRotation;
		private bool _fireThrustersForOrientation;

		/// <summary>
		/// The axis we're rotating around, if any.
		/// </summary>
		private Vector3 _rotationAxis;

		private RotationHint _currentRotation;

		enum RotationHint
		{
			None,
			SpeedingUp,
			SlowingDown,
		}

		private float _angularVelocity;
		private Vector3 _velocity;
		private ThrusterComponent[] _thrusters;

		// Use this for initialization
		private void Start()
		{
			_engine = transform.FindChild("Engine").gameObject;
			_body = GetComponent<Rigidbody>();
			_thrusters = GetComponentsInChildren<ThrusterComponent>();
		}

		// Update is called once per frame
		private void Update()
		{
			_velocity = _body.velocity;
			_engine.SetActive(_isFiringMainEngine);

			foreach (var thruster in _thrusters)
			{
				thruster.Fire(CalculateFireStrength(thruster));
			}

			_fireThrustersForOrientation = false;
			_fireThrustersForRotation = false;
			_currentRotation = RotationHint.None;
		}

		private float CalculateFireStrength(ThrusterComponent thruster)
		{
			if (_fireThrustersForOrientation)
			{
				// TODO: Implement
				return 0;
			}

			if (_fireThrustersForLateralMovement)
			{
				var dot = Vector3.Dot(-_lateralThrusterDirection, thruster.transform.forward);
				return dot;
			}

			return 0;
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
		public float MaximumAngularAcceleration = 10;


		public void Burn(EngineType engineType, Vector3 worldDirection, float deltaVelocity)
		{
			var requiredAcceleration = deltaVelocity / Time.deltaTime;
			float maximumAcceleration;

			switch (engineType)
			{
				case EngineType.Main:
					maximumAcceleration = MaximumAcceleration;
					_isFiringMainEngine = true;
					break;

				case EngineType.Thrusters:
					maximumAcceleration = 2;
					_lateralThrusterDirection = worldDirection;
					// We're constantly applying some lateral thrust to stay in play so
					// we only show that thrusters are firing when the thrust is big enough...
					if (deltaVelocity > 1)
					{
						_fireThrustersForLateralMovement = true;
					}
					break;

				default:
					Debug.LogWarningFormat("Unknown engine type: {0}", engineType);
					return;
			}

			float currentAcceleration = Mathf.Clamp(requiredAcceleration, 0, maximumAcceleration);
			var force = worldDirection * currentAcceleration;
			_body.AddForce(force);
		}

		public void Stop(EngineType type)
		{
			switch (type)
			{
				case EngineType.Main:
					_isFiringMainEngine = false;
					break;
				case EngineType.Thrusters:
					_fireThrustersForLateralMovement = false;
					break;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="targetDirection"></param>
		/// <param name="angle"></param>
		public void OrientShipTowards(Vector3 targetDirection, float angle)
		{
			var current = transform.forward;
			//var delta = targetDirection - current;
			_rotationAxis = Vector3.Cross(current, targetDirection);
			if (angle > 170)
			{
				// Let's provide some initial kick...
				_rotationAxis = transform.right*2;
			}

			var currentVelocity = Mathf.Rad2Deg * Vector3.Project(_body.angularVelocity, _rotationAxis).magnitude;

			var change = Mathf.Clamp(MaximumAngularAcceleration, 0, angle);
			var torque = _rotationAxis * change;

			// r = vt + at²

			var timeToTarget = angle / currentVelocity;
			var timeToZero = currentVelocity / MaximumAngularAcceleration;

			if (timeToZero > timeToTarget)
			{
				torque *= -1.1f;
				_currentRotation = RotationHint.SlowingDown;
			}
			else
			{
				_currentRotation = RotationHint.SpeedingUp;
			}

			Debug.DrawRay(transform.position, torque, Color.cyan);

			_body.AddTorque(torque);

			// We're constantly orienting ourselves, so in order to not show the thrusters
			// as always on, we do so until the angular error is small enough
			if (angle > 3)
			{
				_fireThrustersForOrientation = true;
			}
		}

		public void RotateShip(RotationDirection direction)
		{
			if (direction == RotationDirection.None)
				return;

			var sign = direction == RotationDirection.Left ? 1 : -1;
			var angularChange = sign * MaximumAngularAcceleration * Time.deltaTime;
			transform.Rotate(Vector3.forward, angularChange, Space.Self);

			_fireThrustersForRotation = true;
		}
	}
}