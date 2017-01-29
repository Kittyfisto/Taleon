namespace Assets.Scripts.AI.FCS
{
	public class FlakTurretComponent
		: AbstractGunPlatform
	{
		// Update is called once per frame
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