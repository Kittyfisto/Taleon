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
				var solution = FindFiringSolution(Target);
				if (solution != null)
				{
					_turretBase.TargetDirection = solution.Value.FiringDirection;
					if (CanShoot && _turretBase.IsTargetInSight)
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

		protected override FiringSolution? FindFiringSolution(GameObject target)
		{
			var projectile = ProjectilePrefab.GetComponent<FlakProjectile>();
			var minimumRange = projectile.MinimumFuseRange;

			var targetPosition = target.transform.position;
			var delta = targetPosition - transform.position;
			var distance = delta.magnitude;
			var direction = delta / distance;

			if (distance < minimumRange)
				return null;

			var targetSolution = _turretBase.FindSolution(direction);
			if (targetSolution == null)
				return null; //< even if we wanted to, we're not able to point our gun at this solution...

			return new FiringSolution
			{
				TargetPosition = targetPosition,
				FiringDirection = direction,
				InterceptionDistance = distance
			};
		}
	}
}