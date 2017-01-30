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
		public float RotationVelocity;

		/// <summary>
		///     The direction at which this turret shall point at in order to shoot the projectiles.
		/// </summary>
		public Vector3 TargetDirection;

		private float _currentRotation;
		private float _rotationOffset;

		// Use this for initialization
		private void Start()
		{
			_hinge = GetComponentInChildren<BarrelHinge>();
			_rotationOffset = transform.rotation.eulerAngles.y;
		}

		// Update is called once per frame
		private void Update()
		{
		}
	}
}