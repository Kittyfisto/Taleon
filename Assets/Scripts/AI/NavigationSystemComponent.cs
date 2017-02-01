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

		public float CurrentVelocity
		{
			get { return _engine.CurrentVelocity; }
		}

		public void SetVelocity(float velocity)
		{
			_targetVelocity = velocity;
		}

		private void Start()
		{
			_engine = GetComponentInChildren<EngineSystem>();
		}

		private void Update()
		{
			var velocity = _engine.CurrentVelocity;
			var error = _targetVelocity - velocity;
			if (Mathf.Abs(error) > 0.01)
			{
				var a = _engine.MaximumAcceleration;
				_engine.Burn(Mathf.Sign(error) * a);
			}
			else
			{
				_engine.Stop();
			}
		}
	}
}