using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	public abstract class AbstractGunPlatform
		: AbstractWeaponPlatform
	{
		private ProjectileSpawnComponent[] _barrels;

		/// <summary>
		///     Tries to assign the given gameObject as a <see cref="AbstractWeaponPlatform.Target" /> to this weapon.
		///     Returns true if this weapon is confident that it can intercept the target in time, false otherwise.
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public bool TryAssignTarget(GameObject target)
		{
			var solution = FindFiringSolution(target);
			if (solution == null)
				return false;

			Target = target;
			return true;
		}

		protected abstract FiringSolution? FindFiringSolution(GameObject target);

		protected void ShootProjectile(FiringSolution solution)
		{
			foreach (var barrel in _barrels)
				barrel.Spawn(ProjectilePrefab, solution);
			OnShot();
		}

		protected virtual void Start()
		{
			_barrels = GetComponentsInChildren<ProjectileSpawnComponent>();
		}
	}
}