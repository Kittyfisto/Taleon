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
		private Vector3 _targetDirection;

		public Vector3 CurrentVelocity
		{
			get { return _engine.CurrentVelocity; }
		}

		public void SetVelocity(Vector3 worldDirection, float velocity)
		{
			_targetDirection = worldDirection;
			_targetVelocity = Mathf.Clamp(velocity, 0, float.MaxValue);
		}

		public void SetRotation(float rotation)
		{
			_targetRotation = rotation;
		}

		private void Start()
		{
			_engine = GetComponentInChildren<EngineSystem>();
			_targetDirection = transform.forward;
		}

		private void Update()
		{
			ChangeDirection();
			ChangeVelocity();
		}

		private void ChangeDirection()
		{
			var forward = transform.forward;
			var directionError = Vector3.Angle(_targetDirection, forward);
			var directionSign = Vector3.Cross(_targetDirection, forward);

			if (Mathf.Abs(directionError) > 0.01f)
			{
				var relativeSpeed = Mathf.Clamp(Mathf.InverseLerp(0, 10, directionError), 0, 1);
				var angularSpeed = relativeSpeed * 10;
				_engine.RotateAround(transform.right, directionError, angularSpeed);
			}
		}

		private void ChangeVelocity()
		{
			var velocity = _engine.CurrentVelocity.magnitude;
			var error = _targetVelocity - velocity;
			if (Mathf.Abs(error) > 0.01f)
			{
				//var a = _engine.MaximumAcceleration;
				//_engine.Burn(Mathf.Sign(error) * a);
			}
			else
			{
				_engine.Stop();
			}
		}
	}
}