using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	public abstract class AbstractWeaponPlatform
		: MonoBehaviour
	{
		private bool _reloading;
		private float _reloadTimer;

		#region Public Variables

		/// <summary>
		///     The projectile prefab that is spawned for every shot.
		/// </summary>
		public GameObject ProjectilePrefab;

		/// <summary>
		///     The number of projectiles this gun can shoot per minute.
		/// </summary>
		public float RoundsPerMinute;

		/// <summary>
		///     The target of this turret.
		///     When set to a non-null object,
		/// </summary>
		public GameObject Target;

		#endregion

		public float TimeToNextShot
		{
			get
			{
				if (_reloading)
					return Mathf.Max(0, RoundWaitInterval - _reloadTimer);

				return 0;
			}
		}

		protected float RoundWaitInterval
		{
			get { return 60 / RoundsPerMinute; }
		}

		protected bool CanShoot
		{
			get { return TimeToNextShot <= 0; }
		}

		protected virtual void Update()
		{
			if (_reloading)
			{
				_reloadTimer += Time.deltaTime;
				if (_reloadTimer >= RoundWaitInterval)
					_reloading = false;
			}
		}

		protected void OnShot()
		{
			_reloadTimer = 0;
			_reloading = true;
		}
	}
}