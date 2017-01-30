﻿using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	/// <summary>
	///     Responsible for moving the entire turret into any given direction.
	/// </summary>
	/// <remarks>
	///     A turret base can on itself only rotate around the y-axis.
	///     It depends on a child <see cref="BarrelHinge" /> to allow for vertical rotation of the barrels.
	/// </remarks>
	public class TurretBase : MonoBehaviour
	{
		private BarrelHinge _hinge;

		/// <summary>
		///     The minimum rotation angle of this turret base in degrees.
		/// </summary>
		/// <remarks>
		///     0° is forward, 180° is backwards.
		/// </remarks>
		public float MinimumHorizontalRotation;

		/// <summary>
		///     The maximum rotation angle of this turret base in degrees.
		/// </summary>
		/// <remarks>
		///     0° is forward, 180° is backwards.
		/// </remarks>
		public float MaximumHorizontalRotation;

		/// <summary>
		///     The rotation velocity in degrees per second.
		/// </summary>
		public float MaximumHorizontalRotationPerSecond;

		/// <summary>
		///     The minimum rotation angle of this turret base in degrees.
		/// </summary>
		/// <remarks>
		///     0° is level, 90° is upwards.
		/// </remarks>
		public float MinimumVerticalRotation;

		/// <summary>
		///     The maximum rotation angle of this turret base in degrees.
		/// </summary>
		/// <remarks>
		///     0° is level, 90° is upwards.
		/// </remarks>
		public float MaximumVerticalRotation;

		/// <summary>
		///     The rotation velocity in degrees per second.
		/// </summary>
		public float MaximumVerticalRotationPerSecond;

		/// <summary>
		///     The direction at which this turret shall point at in order to shoot the projectiles.
		/// </summary>
		/// <remarks>
		///     This direction MUST be given in world space.
		/// </remarks>
		public Vector3? TargetDirection;

		public bool CanRotateFullCircle
		{
			get
			{
				return MinimumHorizontalRotation <= -180 &&
				       MaximumHorizontalRotation >= 180;
			}
		}

		// Use this for initialization
		private void Start()
		{
			_hinge = GetComponentInChildren<BarrelHinge>();
		}

		// Update is called once per frame
		private void Update()
		{
			if (TargetDirection != null)
			{
				var solution = FindSolution(TargetDirection.Value);
				if (solution != null)
				{
					RotateTurretBase(solution.Value.HorizontalAngle);
					RotateBarrels(solution.Value.VerticalAngle);
				}
			}
		}

		private void RotateTurretBase(float targetAngle)
		{
			var maximumRotationThisFrame = MaximumHorizontalRotationPerSecond * Time.deltaTime;
			var currentAngle = HorizontalRotation;
			var counterClockwiseDelta = targetAngle - currentAngle;
			float delta;

			if (CanRotateFullCircle && Mathf.Abs(counterClockwiseDelta) > 180)
			{
				delta = counterClockwiseDelta < 0
					? counterClockwiseDelta + 360
					: counterClockwiseDelta - 360;
			}
			else
			{
				delta = counterClockwiseDelta;
			}

			var roationThisFrame = Mathf.Min(Mathf.Abs(delta), maximumRotationThisFrame);

			var actualRoationThisFrame = roationThisFrame * Mathf.Sign(delta);
			var rotationDelta = Quaternion.AngleAxis(actualRoationThisFrame, Vector3.up);
			var rotation = transform.rotation * rotationDelta;
			transform.rotation = rotation;
		}

		public float HorizontalRotation
		{
			get
			{
				var angle = transform.eulerAngles.y;
				if (angle > 180)
					angle -= 360;
				return angle;
			}
		}

		private void RotateBarrels(float targetAngle)
		{
			_hinge.RotateBarrels(targetAngle);
		}

		/// <summary>
		///     Finds a targeting solution (if one exists) and returns it.
		/// </summary>
		/// <param name="direction"></param>
		/// <returns></returns>
		public TargetSolution? FindSolution(Vector3 direction)
		{
			var horizontalSign = Mathf.Sign(Vector3.Cross(Vector3.forward, direction).y);
			var horizontalDirection = Vector3.ProjectOnPlane(direction, transform.up);
			var horizontalAngle = Vector3.Angle(Vector3.forward, horizontalDirection) * horizontalSign;

			var verticalSign = Mathf.Sign(Vector3.Cross(direction, horizontalDirection).y);
			var verticalAngle = Vector3.Angle(horizontalDirection, direction) * verticalSign;

			if (horizontalAngle < MinimumHorizontalRotation || horizontalAngle > MaximumHorizontalRotation)
				return null;

			if (verticalAngle < MinimumVerticalRotation || verticalAngle > MaximumVerticalRotation)
				return null;

			return new TargetSolution
			{
				HorizontalAngle = horizontalAngle,
				VerticalAngle = verticalAngle
			};
		}
	}
}