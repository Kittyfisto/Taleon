using UnityEngine;

namespace Assets.Scripts.AI.FCS
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

		/// <summary>
		///     Tries to find a solution that allows us to shoot a projectile that hits the target
		/// </summary>
		/// <param name="turret"></param>
		/// <param name="projectile"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static FiringSolution? FindSolution(GameObject turret,
			ProjectileComponent projectile,
			GameObject target)
		{
			var rocket = target.GetComponent<RocketComponent>();
			var body = target.GetComponent<Rigidbody>();

			var targetPosition = target.transform.position;
			var targetVelocity = body.velocity;
			var targetAcceleration = rocket.CurrentAcceleration;

			var position = turret.transform.position;
			var velocity = projectile.Velocity;
			var range = projectile.Range;

			return TrySolve(position, velocity, range, targetPosition, targetVelocity, targetAcceleration);
		}

		private static FiringSolution? TrySolve(Vector3 projectilePosition,
			float projectileVelocity,
			float projectileRange,
			Vector3 targetPosition,
			Vector3 targetVelocity, float targetAcceleration)
		{
			// We need to solve "v1*t = a2*t + v2*t"
			// Where v1 is our projectile's speed, v2 is the rocket's speed and a2 is the rocket's acceleration.
			// The following solution is a simple iterative approach that starts with a wrong solution, calculates
			// the error and then tries to find the solution where the error is minimal.

			var distance = Vector3.Distance(projectilePosition, targetPosition);
			var flyingTime = distance / projectileVelocity;
			FiringSolution? solution = null;

			const int maxIterations = 10;

			for (var i = 0; i < maxIterations; ++i)
			{
				// x = at²+vt
				var futurePosition = targetPosition +
									 targetVelocity * flyingTime +
									 targetAcceleration * Mathf.Pow(flyingTime, 2) * targetVelocity.normalized;
				var firingDirection = futurePosition - projectilePosition;
				var finalDistance = firingDirection.magnitude;
				var finalFlyingTime = finalDistance / projectileVelocity;

				if (IsProperSolution(projectilePosition, targetPosition, futurePosition, finalDistance, projectileRange))
					solution = new FiringSolution
					{
						TargetPosition = futurePosition,
						FiringDirection = firingDirection / finalDistance,
						InterceptionDistance = finalDistance
					};

				flyingTime = finalFlyingTime;
			}

			return solution;
		}

		private static bool IsProperSolution(Vector3 projectilePosition,
			Vector3 targetPosition,
			Vector3 interceptPosition,
			float finalDistance, float maximumRange)
		{
			if (finalDistance > maximumRange)
				return false;

			if (finalDistance <= 0)
				return false;

			if (float.IsNaN(finalDistance))
				return false;

			if (float.IsInfinity(finalDistance))
				return false;

			// We need to find out where the interception point is supposed to be.
			// It MUST be in between us and the target, not on the other side (because
			// its too late, then).
			var toTarget = (targetPosition - projectilePosition).normalized;
			var toIntercept = (interceptPosition - projectilePosition).normalized;
			var angle = Vector3.Angle(toTarget, toIntercept);
			if (angle > 90)
				return false;

			return true;
		}
	}
}