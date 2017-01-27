using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	public class RocketTurretComponent
		: AbstractGunPlatform
	{
		public RocketTurretComponent()
		{
			roundsPerMinute = 30;
		}

		/// <summary>
		/// The prefab that is used to spawn new rockets.
		/// </summary>
		public GameObject rocketPrefab;

		/// <summary>
		///     The target of this turret.
		///     When set to a non-null object,
		/// </summary>
		public GameObject target;

		public bool IsShooting { get; set; }
		
		// Update is called once per frame
		protected override void Update()
		{
			base.Update();

			if (target != null)
			{
				IsShooting = true;
				if (CanShoot)
					ShootProjectile(target);
			}
			else
			{
				IsShooting = false;
			}
		}

		private void ShootProjectile(GameObject target)
		{
			var go = Instantiate(rocketPrefab);
			go.transform.position = transform.position;
			var rocket = go.GetComponent<RocketComponent>();
			rocket.target = target;
			OnShot();
		}
	}
}