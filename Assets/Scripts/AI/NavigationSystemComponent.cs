using UnityEngine;

namespace Assets.Scripts.AI
{
	/// <summary>
	///     Responsible for actually controlling the engine(s) of the ship.
	/// </summary>
	public class NavigationSystemComponent : MonoBehaviour
	{
		private float _targetVelocity;
		private float _targetRotation;
		private EngineSystem _engine;
		private Vector3 _targetWorldDirection;
		private bool _facingInTargetDirection;
		private bool _movingInTargetDirection;

		public Vector3 TargetWorldDirection
		{
			get { return _targetWorldDirection; }
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
				_targetWorldDirection = -MovementDirection;
			}
		}

		public Vector3 MovementDirection
		{
			get { return _engine.MovementDirection; }
		}

		public void SetRotation(float rotation)
		{
			_targetRotation = rotation;
		}

		private void Start()
		{
			_engine = GetComponentInChildren<EngineSystem>();
			_targetWorldDirection = transform.forward;
		}

		private void Update()
		{
			ChangeDirection();
			ChangeVelocity();

			Debug.DrawRay(transform.position, _targetWorldDirection*10);
		}

		private void ChangeDirection()
		{
			var forward = transform.forward;
			var directionError = Vector3.Angle(_targetWorldDirection, forward);

			if (Mathf.Abs(directionError) > 0.01f)
			{
				_engine.OrientShipTowards(_targetWorldDirection, directionError);
				_facingInTargetDirection = false;
			}
			else
			{
				_facingInTargetDirection = true;
			}
		}

		private void ChangeVelocity()
		{
			var directionError = Vector3.Angle(_targetWorldDirection, MovementDirection);
			_movingInTargetDirection = Mathf.Abs(directionError) < 0.01f;

			var velocity = _engine.CurrentVelocity.magnitude;
			var deltaVelocity = _targetVelocity - velocity;
			var fuck = Mathf.Abs(deltaVelocity);

			if (_facingInTargetDirection && fuck > 0.01f)
			{
				if (deltaVelocity > 0 || !_movingInTargetDirection)
				{
					// Are we slower than intended? => burn the main engines
					_engine.Burn(EngineType.Main, fuck);
				}
				else
				{
					// We're slightly faster than we intend to
					_engine.Burn(EngineType.BackwardsThrusters, fuck);
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

			_targetWorldDirection = worldTargetDirection/length;
		}
	}
}