﻿using UnityEngine;

namespace Assets.Scripts
{
	public class RtsController
		: MonoBehaviour
	{
		const float MinimumYAngle = Mathf.Deg2Rad * 20;
		const float MaximumYAngle = Mathf.Deg2Rad * 160;

		private Transform _target;
		private float _distance;
		private float _verticalAngle;
		private float _horizontalAngle;
		private Vector3 _lastPosition;

		// Use this for initialization
		void Start ()
		{
			SetTarget("Farragut");
			_distance = 100;
			_verticalAngle = (MaximumYAngle - MinimumYAngle) / 2 + MinimumYAngle;
		}

		public void SetTarget(string name)
		{
			var go = GameObject.Find(name);
			var component = go.GetComponent<Transform>();
			_target = component;
		}
	
		// Update is called once per frame
		void Update ()
		{
			ChangeDistance();
			ChangeOrientation();
			UpdatePositions();
			ChangeDistantObjects();
		}

		private void ChangeDistantObjects()
		{
			var cameraPosition = Camera.main.transform.position;
			var objects = FindObjectsOfType<DistantObjectComponent>();
			foreach (var component in objects)
			{
				var offset = component.Offset;
				var position = cameraPosition + offset;
				component.transform.position = position;
			}
		}

		private void ChangeDistance()
		{
			const float minimumDistance = 3;
			const float maximumDistance = 100;
			const float minimumSpeed = 10;
			const float maximumSpeed = 100;

			var t = Mathf.InverseLerp(minimumDistance, maximumDistance, _distance);
			var speed = Mathf.Lerp(minimumSpeed, maximumSpeed, t);
			var dx = Input.mouseScrollDelta.y *10* speed * Time.deltaTime;
			_distance -= dx;
			_distance = Mathf.Clamp(_distance, minimumDistance, maximumDistance);
		}

		private void ChangeOrientation()
		{
			const float speed = Mathf.Deg2Rad*0.3f;

			var currentPosition = Input.mousePosition;
			if (Input.GetMouseButton(MouseButtons.Right))
			{
				var mouseDelta = currentPosition - _lastPosition;
				var deltaH = mouseDelta.x * speed;
				_horizontalAngle -= deltaH;

				var deltaV = mouseDelta.y * speed;
				_verticalAngle += deltaV;
				_verticalAngle = Mathf.Clamp(_verticalAngle, MinimumYAngle, MaximumYAngle);

				//Debug.LogFormat("Moving camera: x={0}, y={1}", _horizontalAngle, _verticalAngle);
			}
			_lastPosition = currentPosition;
		}

		private void UpdatePositions()
		{
			var direction = new Vector3(
				Mathf.Sin(_verticalAngle) * Mathf.Cos(_horizontalAngle),
				Mathf.Cos(_verticalAngle),
				Mathf.Sin(_verticalAngle) * Mathf.Sin(_horizontalAngle)
			);

			//Debug.LogFormat("Camera: direction={0}, radius={1}", direction, _distance);
			
			transform.position = _target.position + _distance*direction;
			transform.LookAt(_target.position);
		}
	}
}
