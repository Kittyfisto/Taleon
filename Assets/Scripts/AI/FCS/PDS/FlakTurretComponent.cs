namespace Assets.Scripts.AI.FCS.PDS
{
	public class FlakTurretComponent
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
					var targetPosition = Target.transform.position;
					var delta = targetPosition - transform.position;
					var distance = delta.magnitude;
					var direction = delta / distance;

					var solution = new FiringSolution
					{
						TargetPosition = targetPosition,
						FiringDirection = direction,
						InterceptionDistance = distance
					};
					ShootProjectile(solution);
				}
			}
		}
	}
}