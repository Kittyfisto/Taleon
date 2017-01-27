using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	/// <summary>
	///     Responsible for instantiating and shooting a projectile at its current location.
	///     Should be located at the end of a gun barrel.
	///     Includings a light source.
	/// </summary>
	[RequireComponent(typeof(Light))]
	public class ProjectileSpawnComponent : MonoBehaviour
	{
		private Light _light;

		private float _muzzleTimer;
		private float _maximumInstensity;

		/// <summary>
		///     The amount of time the muzzle flash illuminates the surroundings.
		/// </summary>
		public float MuzzleFlashTime = 0.5f;

		// Use this for initialization
		private void Start()
		{
			_light = GetComponent<Light>();
			_maximumInstensity = _light.intensity;
		}

		// Update is called once per frame
		private void Update()
		{
			_muzzleTimer += Time.deltaTime;
			var d = 1 - Mathf.InverseLerp(0, MuzzleFlashTime, _muzzleTimer);
			_light.intensity = Mathf.Clamp(d, 0, _maximumInstensity);
		}

		public void Spawn(GameObject projectilePrefab, Vector3 direction)
		{
			_muzzleTimer = 0;

			var body = Instantiate(projectilePrefab);
			var projectile = body.GetComponent<ProjectileComponent>();
			var spawnPosition = transform.position;
			projectile.Shoot(spawnPosition, direction);
		}
	}
}