using UnityEngine;

namespace Assets.Scripts.AI
{
	[RequireComponent(typeof(Rigidbody))]
	public class EngineSystem
		: MonoBehaviour
	{
		private GameObject _engine;

		private Rigidbody _body;
		private bool _isFiring;

		// Use this for initialization
		private void Start()
		{
			_engine = transform.FindChild("Engine").gameObject;
			_body = GetComponent<Rigidbody>();
		}

		// Update is called once per frame
		private void Update()
		{
			_velocity = _body.velocity.magnitude;
		}

		public bool IsFiring
		{
			get { return _isFiring; }
		}

		public float CurrentVelocity
		{
			get { return _velocity; }
		}

		/// <summary>
		/// 10m/s²
		/// </summary>
		public float MaximumAcceleration = 10;

		private float _velocity;

		public void Burn(float acceleration)
		{
			var force = transform.forward * acceleration;
			_body.AddForce(force);

			_isFiring = true;
			UpdateEngine();
		}

		public void Stop()
		{
			_isFiring = false;
			UpdateEngine();
		}

		private void UpdateEngine()
		{
			_engine.SetActive(_isFiring);
		}
	}
}