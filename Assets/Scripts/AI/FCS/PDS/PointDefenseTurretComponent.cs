﻿using UnityEngine;

namespace Assets.Scripts.AI.FCS.PDS
{
	/// <summary>
	/// This component is responsible for controlling a single point defense turret.
	/// This includes aiming the turret at the assigned target and firing projectiles
	/// whenever ready.
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class PointDefenseTurretComponent
		: AbstractGunPlatform
	{
		private AudioSource _audioSource;
		private bool _isShooting;
		
		public PointDefenseTurretComponent()
		{
			RoundsPerMinute = 60;
		}

		public bool IsShooting
		{
			get { return _isShooting; }
			set
			{
				if (value == _isShooting)
					return;

				_isShooting = value;
				if (value)
					_audioSource.Play();
				else
					_audioSource.Stop();
			}
		}

		// Use this for initialization
		protected override void Start()
		{
			base.Start();

			_audioSource = GetComponent<AudioSource>();
		}

		// Update is called once per frame
		protected override void Update()
		{
			base.Update();

			if (Target != null)
			{
				FiringSolution? solution = FindSolution(Target);
				if (solution != null)
				{
					Debug.DrawRay(transform.position, solution.Value.TargetPosition - transform.position);

					IsShooting = true;
					if (CanShoot)
					{
						ShootProjectile(solution.Value);
					}
				}
				else
				{
					IsShooting = false;
					Target = null;
				}

			}
			else
			{
				IsShooting = false;
			}
		}
	}
}