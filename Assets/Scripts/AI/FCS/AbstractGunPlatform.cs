using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	public abstract class AbstractGunPlatform
		: MonoBehaviour
	{
		public float roundsPerMinute;
		private float _reloadTimer;
		private bool _reloading;

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
			get { return 60 / roundsPerMinute; }
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
				{
					_reloading = false;
				}
			}
		}

		protected void OnShot()
		{
			_reloadTimer = 0;
			_reloading = true;
		}
	}
}