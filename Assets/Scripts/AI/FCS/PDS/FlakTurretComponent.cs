using UnityEngine;

namespace Assets.Scripts.AI.FCS.PDS
{
	public sealed class FlakTurretComponent
		: AbstractGunPlatform
	{
		/// <summary>
		///     The distance from the ship at which the flak screen should be spawned.
		/// </summary>
		public float Distance;

		protected override void Update()
		{
			base.Update();

			if (Target != null)
			{
				if (CanShoot)
				{
					var solution = FindSolution(Target);
					if (solution != null)
					{
						ShootProjectile(solution.Value);
					}
				}
			}
		}

		protected override FiringSolution? FindSolution(GameObject target)
		{
			var projectile = ProjectilePrefab.GetComponent<FlakProjectile>();
			var minimumRange = projectile.MinimumFuseRange;

			var targetPosition = target.transform.position;
			var delta = targetPosition - transform.position;
			var distance = delta.magnitude;
			var direction = delta / distance;

			if (distance < minimumRange)
				return null;

			return new FiringSolution
			{
				TargetPosition = targetPosition,
				FiringDirection = direction,
				InterceptionDistance = distance
			};
		}
	}
}