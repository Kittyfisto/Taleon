using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	public sealed class RailgunTurretComponent
		: AbstractGunPlatform
	{
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
			var targetPosition = target.transform.position;
			var delta = targetPosition - transform.position;
			if (delta == Vector3.zero)
				return null;

			var direction = delta.normalized;

			return new FiringSolution
			{
				FiringDirection = direction,
				InterceptionDistance = float.MaxValue,
				TargetPosition = targetPosition
			};
		}
	}
}