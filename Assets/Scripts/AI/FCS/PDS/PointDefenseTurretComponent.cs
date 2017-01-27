using UnityEngine;

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
		private GameObject _projectile;

		/// <summary>
		/// The target of this turret.
		/// When set to a non-null object,
		/// </summary>
		public GameObject target;

		private ProjectileSpawnComponent _spawn;

		public PointDefenseTurretComponent()
		{
			roundsPerMinute = 60;
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
		private void Start()
		{
			_projectile = Resources.Load("pds_projectile") as GameObject;
			_audioSource = GetComponent<AudioSource>();
			_spawn = GetComponentInChildren<ProjectileSpawnComponent>();
		}

		// Update is called once per frame
		protected override void Update()
		{
			base.Update();

			if (target != null)
			{
				IsShooting = true;
				if (CanShoot)
					ShootProjectile(target.transform.position);
			}
			else
			{
				IsShooting = false;
			}
		}

		private void ShootProjectile(Vector3 targetPosition)
		{
			var direction = (targetPosition - transform.position).normalized;

			_spawn.Spawn(_projectile, direction);
			OnShot();
		}
	}
}