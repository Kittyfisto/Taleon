using UnityEngine;

namespace Assets.Scripts.AI.FCS.PDS
{
	public struct FiringSolution
	{
		/// <summary>
		///     The position at which the two projectiles are assumed to meet.
		/// </summary>
		public Vector3 TargetPosition;

		/// <summary>
		///     The direction at which the projectile must be fired to hit the other one.
		/// </summary>
		public Vector3 FiringDirection;

		/// <summary>
		///     The total distance the projectile has to cover to intercept
		///     the other one.
		/// </summary>
		public float InterceptionDistance;
	}
}