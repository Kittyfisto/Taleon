using UnityEngine;

namespace Assets.Scripts.AI.FCS.PDS
{
	/// <summary>
	/// The third kind of point defense system: A laser that literally moves at light speed, but cannot be fired very often.
	/// </summary>
	public class LaserTurretComponent
		: AbstractGunPlatform
	{
		public float MinimumDistance;
		public float MaximumDistance;

		protected override FiringSolution? FindFiringSolution(GameObject target)
		{
			// The laser moves instantly (and distances in this game are very much less than
			// a light second).
			var targetPosition = target.transform.position;
			var delta = targetPosition - transform.position;
			var distance = delta.magnitude;
			if (distance < MinimumDistance || distance > MaximumDistance)
				return null;

			var direction = delta/distance;

			return new FiringSolution
			{
				FiringDirection = direction,
				InterceptionDistance = float.MaxValue,
				TargetPosition = targetPosition
			};
		}
	}
}