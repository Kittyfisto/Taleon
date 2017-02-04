using UnityEngine;

namespace Assets.Scripts.AI
{
	/// <summary>
	///     Responsible for translating concrete commands such as:
	///     - Change velocity by X into direction y
	///     - Orient ship towards direction z
	///     - Rotate ship around forward axis by w degrees
	///     Into forces that are applied to the rigid body of the ship.
	///     Enables / disables all relevant engines and thrusters to make this look convincing.
	/// </summary>
	[RequireComponent(typeof(Rigidbody))]
	public class EngineSystem
		: MonoBehaviour
	{
		private float _angularVelocity;

		private Rigidbody _body;

		private RotationHint _currentRotation;
		private EngineComponent _engine;

		private bool _fireThrustersForLateralMovement;
		private bool _fireThrustersForOrientation;

		private bool _fireThrustersForRotation;
		private Vector3 _lateralThrusterDirection;

		/// <summary>
		///     The axis we're rotating around, if any.
		/// </summary>
		private Vector3 _rotationAxis;

		private ThrusterComponent[] _thrusters;

		/// <summary>
		///     10m/s²
		/// </summary>
		public float MaximumAcceleration = 10;

		/// <summary>
		///     10 deg/s
		/// </summary>
		public float MaximumAngularAcceleration = 10;

		/// <summary>
		///     2m/s²
		/// </summary>
		public float MaximumThrusterAcceleration = 2;

		public bool IsFiringMainEngine { get; private set; }

		public Vector3 CurrentVelocity { get; private set; }

		public Vector3 MovementDirection
		{
			get
			{
				var velocity = _body.velocity;
				var length = velocity.magnitude;
				if (length <= 0.001)
					return Vector3.zero;

				return velocity / length;
			}
		}

		// Use this for initialization
		private void Start()
		{
			_body = GetComponent<Rigidbody>();
			_engine = GetComponentInChildren<EngineComponent>();
			_thrusters = GetComponentsInChildren<ThrusterComponent>();
		}

		// Update is called once per frame
		private void Update()
		{
			CurrentVelocity = _body.velocity;
			_engine.Fire(IsFiringMainEngine);

			foreach (var thruster in _thrusters)
				thruster.Fire(CalculateFireStrength(thruster));

			_fireThrustersForOrientation = false;
			_fireThrustersForRotation = false;
			_currentRotation = RotationHint.None;
		}

		private float CalculateFireStrength(ThrusterComponent thruster)
		{
			if (_fireThrustersForOrientation)
				return 0;

			if (_fireThrustersForLateralMovement)
			{
				var dot = Vector3.Dot(-_lateralThrusterDirection, thruster.transform.forward);
				return dot;
			}

			return 0;
		}


		public void Burn(EngineType engineType, Vector3 worldDirection, float deltaVelocity)
		{
			var requiredAcceleration = deltaVelocity / Time.deltaTime;
			float maximumAcceleration;

			switch (engineType)
			{
				case EngineType.Main:
					maximumAcceleration = MaximumAcceleration;
					IsFiringMainEngine = true;
					break;

				case EngineType.Thrusters:
					maximumAcceleration = 2;
					_lateralThrusterDirection = worldDirection;
					// We're constantly applying some lateral thrust to stay in play so
					// we only show that thrusters are firing when the thrust is big enough...
					if (deltaVelocity > 1)
						_fireThrustersForLateralMovement = true;
					break;

				default:
					Debug.LogWarningFormat("Unknown engine type: {0}", engineType);
					return;
			}

			var currentAcceleration = Mathf.Clamp(requiredAcceleration, 0, maximumAcceleration);
			var force = worldDirection * currentAcceleration;
			_body.AddForce(force);
		}

		public void Stop(EngineType type)
		{
			switch (type)
			{
				case EngineType.Main:
					IsFiringMainEngine = false;
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
				_rotationAxis = transform.right * 2;

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
				_fireThrustersForOrientation = true;
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

		private enum RotationHint
		{
			None,
			SpeedingUp,
			SlowingDown
		}
	}
}