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
					var direction = Target.transform.position - transform.position;
					var solution = new FiringSolution
					{
						TargetPosition = Target.transform.position,
						FiringDirection = direction,
					};
					ShootProjectile(solution);
				}
			}
		}
	}
}