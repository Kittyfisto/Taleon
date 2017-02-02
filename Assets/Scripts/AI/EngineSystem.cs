﻿using UnityEngine;

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
			_velocity = _body.velocity;
		}

		public bool IsFiring
		{
			get { return _isFiring; }
		}

		public Vector3 CurrentVelocity
		{
			get { return _velocity; }
		}

		/// <summary>
		/// 10m/s²
		/// </summary>
		public float MaximumAcceleration = 10;

		/// <summary>
		/// 10 deg/s
		/// </summary>
		public float MaximumAngularVelocity = 90;

		private Vector3 _velocity;

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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rotationAxis"></param>
		/// <param name="angle"></param>
		/// <param name="angularVelocity">The angular velocity in deg per second</param>
		public void RotateAround(Vector3 rotationAxis, float angle, float angularVelocity)
		{
			//_body.AddTorque(rotationAxis*angularVelocity*Time.deltaTime);
			var deltaAngle = MaximumAngularVelocity * Time.deltaTime;
			deltaAngle = Mathf.Clamp(deltaAngle, 0, angle);

			transform.RotateAround(transform.position,
				rotationAxis,
				deltaAngle);
		}
	}
}