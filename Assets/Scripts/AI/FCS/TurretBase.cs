using UnityEngine;

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
		///     The maximum rotation of this turret base in degrees.
		/// </summary>
		/// <remarks>
		///     0° is forward, 180° is backwards.
		/// </remarks>
		public float MaximumRotation;

		/// <summary>
		///     The minimum rotation of this turret base in degrees.
		/// </summary>
		/// <remarks>
		///     0° is forward, 180° is backwards.
		/// </remarks>
		public float MinimumRotation;

		/// <summary>
		///     The rotation velocity in degrees per second.
		/// </summary>
		public float TurretRotationInDegPerSecond;

		/// <summary>
		///     The direction at which this turret shall point at in order to shoot the projectiles.
		/// </summary>
		/// <remarks>
		///     This direction MUST be given in world space.
		/// </remarks>
		public Vector3? TargetDirection;

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
				var targetRotation = Vector3.ProjectOnPlane(TargetDirection.Value, transform.up);
				var angle = Vector3.Angle(transform.forward, targetRotation);
				var maximumRotationThisFrame = TurretRotationInDegPerSecond * Time.deltaTime;
				var roationThisFrame = Mathf.Min(angle, maximumRotationThisFrame);

				var sign = Mathf.Sign(Vector3.Cross(transform.forward, targetRotation).y);
				var actualRoationThisFrame = roationThisFrame * sign;
				var rotationDelta = Quaternion.AngleAxis(actualRoationThisFrame, Vector3.up);
				var rotation = transform.rotation * rotationDelta;
				transform.rotation = rotation;
			}
		}
	}
}