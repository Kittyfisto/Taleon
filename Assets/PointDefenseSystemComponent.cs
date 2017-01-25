using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDefenseSystemComponent : MonoBehaviour
{
	public PointDefenseSystemComponent()
	{
		roundsPerMinute = 60;
	}

	public float roundsPerMinute;

	private float _lastShot;
	private GameObject _prefab;

	private float RoundWaitInterval
	{
		get
		{
			return 60/roundsPerMinute;
		}
	}

	private bool CanShoot
	{
		get
		{
			var delta = Time.time - _lastShot;
			if (delta >= RoundWaitInterval)
			{
				return true;
			}

			return false;
		}
	}

	// Use this for initialization
	void Start ()
	{
		_prefab = Resources.Load("pds_projectile") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (CanShoot)
		{
			Shoot();
		}
	}

	private void Shoot()
	{
		GameObject body = Instantiate(_prefab) as GameObject;
		var projectile = body.GetComponent<ProjectileComponent>();
		projectile.Shoot(transform.position, transform.forward);

		_lastShot = Time.time;
	}
}
