using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	public abstract class AbstractGunPlatform
		: AbstractWeaponPlatform
	{
		private ProjectileSpawnComponent[] _barrels;

		protected void ShootProjectile(FiringSolution solution)
		{
			foreach (var barrel in _barrels)
			{
				barrel.Spawn(ProjectilePrefab, solution);
			}
			OnShot();
		}

		protected virtual void Start()
		{
			_barrels = GetComponentsInChildren<ProjectileSpawnComponent>();
		}

		/// <summary>
		/// Tries to find a solution that allows us to shoot a projectile that hits the target
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		protected FiringSolution? FindSolution(GameObject target)
		{
			var projectile = ProjectilePrefab.GetComponent<ProjectileComponent>();
			var rocket = target.GetComponent<RocketComponent>();
			var body = target.GetComponent<Rigidbody>();

			var targetPosition = target.transform.position;
			var targetVelocity = body.velocity;
			var targetAcceleration = rocket.CurrentAcceleration;

			var position = transform.position;
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

			for (int i = 0; i < maxIterations; ++i)
			{
				// x = at²+vt
				var futurePosition = targetPosition +
									 targetVelocity * flyingTime +
									 targetAcceleration * Mathf.Pow(flyingTime, 2) * targetVelocity.normalized;
				var firingDirection = futurePosition - projectilePosition;
				var finalDistance = firingDirection.magnitude;
				var finalFlyingTime = finalDistance / projectileVelocity;

				if (IsProperSolution(finalDistance, projectileRange))
				{
					solution = new FiringSolution
					{
						TargetPosition = futurePosition,
						FiringDirection = firingDirection / finalDistance,
						InterceptionDistance = finalDistance
					};
				}

				flyingTime = finalFlyingTime;
			}

			return solution;
		}

		private static bool IsProperSolution(float finalDistance, float maximumRange)
		{
			if (finalDistance > maximumRange)
				return false;

			if (finalDistance <= 0)
				return false;

			if (float.IsNaN(finalDistance))
				return false;

			if (float.IsInfinity(finalDistance))
				return false;

			return true;
		}

	}
}