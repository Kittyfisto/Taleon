using UnityEngine;

/// <summary>
/// This component is responsible for controlling a single point defense turret.
/// This includes aiming the turret at the assigned target and firing projectiles
/// whenever ready.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PointDefenseTurretComponent : MonoBehaviour
{
	private AudioSource _audioSource;
	private bool _isShooting;
	private float _lastShot;
	private GameObject _prefab;

	/// <summary>
	/// The target of this turret.
	/// When set to a non-null object,
	/// </summary>
	public GameObject target;

	public float roundsPerMinute;
	private ProjectileSpawnComponent _spawn;

	public PointDefenseTurretComponent()
	{
		roundsPerMinute = 60;
	}

	private float RoundWaitInterval
	{
		get { return 60 / roundsPerMinute; }
	}

	private bool CanShoot
	{
		get
		{
			var delta = Time.time - _lastShot;
			if (delta >= RoundWaitInterval)
				return true;

			return false;
		}
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
		_prefab = Resources.Load("pds_projectile") as GameObject;
		_audioSource = GetComponent<AudioSource>();
		_spawn = GetComponentInChildren<ProjectileSpawnComponent>();
	}

	// Update is called once per frame
	private void Update()
	{
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

		_spawn.Spawn(_prefab, direction);

		_lastShot = Time.time;
	}
}