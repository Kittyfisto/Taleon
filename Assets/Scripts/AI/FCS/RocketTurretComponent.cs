﻿using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	public class RocketTurretComponent
		: AbstractWeaponPlatform
	{
		private ShipSystemComponent _ship;

		public RocketTurretComponent()
		{
			RoundsPerMinute = 30;
		}

		public bool IsShooting { get; set; }

		private void Start()
		{
			_ship = transform.parent.GetComponent<ShipSystemComponent>();
		}

		// Update is called once per frame
		protected override void Update()
		{
			base.Update();

			if (Target != null)
			{
				IsShooting = true;
				if (CanShoot)
					ShootProjectile(Target);
			}
			else
			{
				IsShooting = false;
			}
		}

		private void ShootProjectile(GameObject target)
		{
			var go = Instantiate(ProjectilePrefab);
			go.transform.position = transform.position;
			go.transform.forward = transform.forward;
			var rocket = go.GetComponent<RocketComponent>();
			rocket.Target = target;

			if (_ship != null)
				rocket.AdditionalVelocity = _ship.CurrentVelocity;

			OnShot();
		}
	}
}