using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	/// <summary>
	///     Responsible for instantiating and shooting a projectile at its current location.
	///     Should be located at the end of a gun barrel.
	///     Includings a light source.
	/// </summary>
	public class ProjectileSpawnComponent : MonoBehaviour
	{
		public void Spawn(GameObject projectilePrefab, Vector3 direction)
		{
			var body = Instantiate(projectilePrefab);
			var projectile = body.GetComponent<ProjectileComponent>();
			var spawnPosition = transform.position;
			projectile.Shoot(spawnPosition, direction.normalized);
		}
	}
}