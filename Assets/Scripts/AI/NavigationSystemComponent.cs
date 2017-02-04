using UnityEngine;

namespace Assets.Scripts.AI
{
	/// <summary>
	///     Responsible for actually controlling the engine(s) of the ship.
	/// </summary>
	[RequireComponent(typeof(EngineSystem))]
	public class NavigationSystemComponent : MonoBehaviour
	{
		private float _targetVelocity;
		private EngineSystem _engine;
		private bool _facingInTargetDirection;
		private bool _movingInTargetDirection;
		private bool _movingInOppositeTargetDirection;

		private Vector3 _targetWorldForward;
		private float _orientationError;

		public Vector3 TargetWorldForward
		{
			get { return _targetWorldForward; }
		}

		public Vector3 CurrentVelocity
		{
			get { return _engine.CurrentVelocity; }
		}

		public void SetVelocity(float velocity)
		{
			_targetVelocity = Mathf.Clamp(velocity, 0, float.MaxValue);
			if (_targetVelocity <= 0.001)
			{
				// We must ensure that we're pointing in the right direction
				_targetWorldForward = -MovementDirection;
			}
		}

		public Vector3 MovementDirection
		{
			get { return _engine.MovementDirection; }
		}

		public void Rotate(RotationDirection direction)
		{
			_engine.RotateShip(direction);
		}

		private void Start()
		{
			_engine = GetComponentInChildren<EngineSystem>();
			_targetWorldForward = transform.forward;
		}

		private void Update()
		{
			ChangeDirection();
			ChangeVelocity();

			Debug.DrawRay(transform.position, _targetWorldForward*10, Color.green);

			Debug.DrawRay(transform.position, MovementDirection*10, Color.blue);
		}

		private void ChangeDirection()
		{
			var forward = transform.forward;
			_orientationError = Vector3.Angle(_targetWorldForward, forward);

			if (Mathf.Abs(_orientationError) > 0.1f)
			{
				_engine.OrientShipTowards(_targetWorldForward, _orientationError);
				_facingInTargetDirection = false;
			}
			else
			{
				_facingInTargetDirection = true;
			}
		}

		private void ChangeVelocity()
		{
			var directionError = Vector3.Angle(_targetWorldForward, MovementDirection);
			_movingInTargetDirection = Mathf.Abs(directionError) < 5f;
			_movingInOppositeTargetDirection = directionError > 170;

			var currentVelocity = _engine.CurrentVelocity;
			var currentVelocityMagnitude = currentVelocity.magnitude;
			var signedVelocityErrorMagnitude = _targetVelocity - currentVelocityMagnitude;

			var requiredVelocity = _targetWorldForward * _targetVelocity;
			var velocityError = requiredVelocity - currentVelocity;
			var velocityErrorMagnitude = velocityError.magnitude;
			var velocityErrorDirection = velocityError / velocityErrorMagnitude;
			var absoluteVelocityError = Mathf.Abs(velocityErrorMagnitude);
			bool pointingInCorrectDirection = Mathf.Abs(_orientationError) < 5;

			if (velocityErrorMagnitude > 0.01f)
			{
				_engine.Burn(EngineType.Thrusters, velocityErrorDirection, absoluteVelocityError);
			}
			else
			{
				_engine.Stop(EngineType.Thrusters);
			}

			if (pointingInCorrectDirection && absoluteVelocityError > 0.01f)
			{
				if (signedVelocityErrorMagnitude > 0)
				{
					_engine.Burn(EngineType.Main, _targetWorldForward, absoluteVelocityError);
				}
				else if (_movingInOppositeTargetDirection)
				{
					_engine.Burn(EngineType.Main, _targetWorldForward, absoluteVelocityError);
				}
				else
				{
					_engine.Stop(EngineType.Main);
				}
			}
			else
			{
				_engine.Stop(EngineType.Main);
			}
		}

		public void SetDirection(Vector3 worldTargetDirection)
		{
			var length = worldTargetDirection.magnitude;
			if (length < 0.01)
				return;

			var newForward = worldTargetDirection/length;
			_targetWorldForward = newForward;
		}
	}
}