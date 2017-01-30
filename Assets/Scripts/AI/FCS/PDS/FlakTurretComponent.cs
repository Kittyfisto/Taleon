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

		private TurretBase _turretBase;

		protected override void Start()
		{
			base.Start();

			_turretBase = GetComponentInChildren<TurretBase>();
		}

		protected override void Update()
		{
			base.Update();

			if (Target != null)
			{
				var solution = FindSolution(Target);
				if (solution != null)
				{
					_turretBase.TargetDirection = solution.Value.FiringDirection;
					if (CanShoot)
					{
						ShootProjectile(solution.Value);
					}
				}
				else
				{
					_turretBase.TargetDirection = null;
				}
			}
			else
			{
				_turretBase.TargetDirection = null;
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