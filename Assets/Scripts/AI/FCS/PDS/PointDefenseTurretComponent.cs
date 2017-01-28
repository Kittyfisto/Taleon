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

		/// <summary>
		/// The target of this turret.
		/// When set to a non-null object,
		/// </summary>
		public GameObject target;

		public GameObject ProjectilePrefab;

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
			_audioSource = GetComponent<AudioSource>();
			_spawn = GetComponentInChildren<ProjectileSpawnComponent>();
		}

		// Update is called once per frame
		protected override void Update()
		{
			base.Update();

			if (target != null)
			{
				FiringSolution? solution = FindSolution(target);
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
					target = null;
				}

			}
			else
			{
				IsShooting = false;
			}
		}

		/// <summary>
		/// Tries to find a solution that allows us to shoot a projectile that hits the target
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		private FiringSolution? FindSolution(GameObject target)
		{
			var rocket = target.GetComponent<RocketComponent>();
			var body = target.GetComponent<Rigidbody>();

			var targetPosition = target.transform.position;
			var targetVelocity = body.velocity;
			var targetAcceleration = rocket.MaximumAcceleration;

			var position = _spawn.transform.position;
			var velocity = ProjectilePrefab.GetComponent<ProjectileComponent>().velocity;

			return TrySolve(position, velocity, targetPosition, targetVelocity, targetAcceleration);
		}

		private static FiringSolution? TrySolve(Vector3 projectilePosition, float projectileVelocity, Vector3 targetPosition,
			Vector3 targetVelocity, float targetAcceleration)
		{
			// We need to solve "v1*t = a2*t + v2*t"
			// Where v1 is our projectile's speed, v2 is the rocket's speed and a2 is the rocket's acceleration.
			// The following solution is a simple iterative approach that starts with a wrong solution, calculates
			// the error and then tries to find the solution where the error is minimal.

			var distance = Vector3.Distance(projectilePosition, targetPosition);
			var flyingTime = distance / projectileVelocity;
			FiringSolution? solution = null;

			const int maxIterations = 10;

			for(int i = 0; i < maxIterations; ++i)
			{
				// x = at²+vt
				var futurePosition = targetPosition +
				                     targetVelocity * flyingTime +
				                     targetAcceleration * Mathf.Pow(flyingTime, 2) * targetVelocity.normalized;
				var firingDirection = futurePosition - projectilePosition;
				var finalDistance = firingDirection.magnitude;
				var finalFlyingTime = finalDistance / projectileVelocity;
				var distanceError = finalDistance - distance;
				var timeError = finalFlyingTime - flyingTime;

				if (IsProperSolution(finalDistance))
				{
					solution = new FiringSolution
					{
						TargetPosition = futurePosition,
						FiringDirection = firingDirection / finalDistance,
						DistanceError = distanceError,
						TimeError = timeError,
					};
				}

				flyingTime = finalFlyingTime;
			}

			return solution;
		}

		private static bool IsProperSolution(float finalDistance)
		{
			return finalDistance > 0 &&
			       !float.IsNaN(finalDistance) &&
			       !float.IsInfinity(finalDistance);
		}

		private void ShootProjectile(FiringSolution solution)
		{
			_spawn.Spawn(ProjectilePrefab, solution.FiringDirection);
			OnShot();
		}
	}
}