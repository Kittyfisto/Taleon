using UnityEngine;

namespace Assets.Scripts.AI.FCS.PDS
{
	/// <summary>
	///     This component is responsible for controlling a single point defense turret.
	///     This includes aiming the turret at the assigned target and firing projectiles
	///     whenever ready.
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class PointDefenseTurretComponent
		: AbstractGunPlatform
	{
		private AudioSource _audioSource;
		private bool _isShooting;
		private float _playing;

		public PointDefenseTurretComponent()
		{
			RoundsPerMinute = 60;
		}

		protected override FiringSolution? FindFiringSolution(GameObject target)
		{
			return FiringSolution.FindSolution(gameObject, ProjectilePrefab.GetComponent<ProjectileComponent>(), target);
		}

		public bool IsShooting
		{
			get { return _isShooting; }
			set
			{
				_isShooting = value;
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
				IsShooting = true;
				if (CanShoot)
				{
					var solution = FindFiringSolution(Target);
					if (solution != null)
						ShootProjectile(solution.Value);
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


			if (IsShooting)
			{
				if (!_audioSource.isPlaying)
				{
					_audioSource.Play();
					_playing = 0;
				}
				else
				{
					_playing += Time.deltaTime;
				}
			}
			else
			{
				if (_playing > 0.5f)
					_audioSource.Stop();
			}
		}
	}
}