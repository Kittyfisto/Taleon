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
			var directionError = Vector3.Angle(_targetWorldForward, forward);

			if (Mathf.Abs(directionError) > 1f)
			{
				_engine.OrientShipTowards(_targetWorldForward, directionError);
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
			_movingInTargetDirection = Mathf.Abs(directionError) < 1f;

			var velocity = _engine.CurrentVelocity.magnitude;
			var deltaVelocity = _targetVelocity - velocity;
			var velocityError = Mathf.Abs(deltaVelocity);

			if (_facingInTargetDirection && velocityError > 0.01f)
			{
				if (deltaVelocity > 0 || !_movingInTargetDirection)
				{
					// Are we slower than intended? => burn the main engines
					_engine.Burn(EngineType.Main, velocityError);
				}
				else
				{
					// We're slightly faster than we intend to
					_engine.Burn(EngineType.BackwardsThrusters, velocityError);
				}
			}
			else
			{
				_engine.Stop();
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