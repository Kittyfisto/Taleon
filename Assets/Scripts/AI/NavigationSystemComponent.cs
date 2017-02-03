using UnityEngine;

namespace Assets.Scripts.AI
{
	/// <summary>
	///     Responsible for actually controlling the engine(s) of the ship.
	/// </summary>
	public class NavigationSystemComponent : MonoBehaviour
	{
		private float _targetVelocity;
		private EngineSystem _engine;
		private bool _facingInTargetDirection;
		private bool _movingInTargetDirection;

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

			var velocity = _engine.CurrentVelocity.magnitude;
			var deltaVelocity = _targetVelocity - velocity;
			var velocityError = Mathf.Abs(deltaVelocity);
			bool pointingInCorrectDirection = Mathf.Abs(_orientationError) < 5;

			Vector3 movementError;
			if ((_targetWorldForward - MovementDirection).TryGetNormalized(0.1f, out movementError))
			{
				// We're not moving in the right direction and need to perform a lateral burn
				_engine.Burn(EngineType.Thrusters, movementError, velocityError);
			}
			else
			{
				if (pointingInCorrectDirection && velocityError > 0.01f)
				{
					if (deltaVelocity > 0 || !_movingInTargetDirection)
					{
						// Are we slower than intended? => burn the main engines
						_engine.Burn(EngineType.Main, _targetWorldForward, velocityError);
					}
					else
					{// We are definately moving in the right direction, but just a little bit too fast.
						_engine.Burn(EngineType.Thrusters, -_targetWorldForward, velocityError);
					}
				}
				else
				{
					_engine.Stop();
				}
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