using System;
using UnityEngine;

namespace Assets.Scripts
{
	public sealed class Timer
	{
		public event Action Elapsed;
		private bool _isEnabled;
		private float _elapsed;
		private bool _oneShot;

		public bool IsEnabled
		{
			get { return _isEnabled; }
			set
			{
				_isEnabled = value;
				if (!value)
				{
					_elapsed = 0;
				}
			}
		}

		public void Start()
		{
			IsEnabled = true;
		}

		public void Stop()
		{
			IsEnabled = false;
		}

		public bool OneShot
		{
			get { return _oneShot; }
			set { _oneShot = value; }
		}

		public TimeSpan Interval { get; set; }

		public void Update()
		{
			if (IsEnabled)
			{
				_elapsed += Time.deltaTime;
				if (_elapsed > Interval.TotalSeconds)
				{
					var fn = Elapsed;
					if (fn != null)
						fn();

					if (OneShot)
					{
						IsEnabled = false;
					}
				}
			}
		}
	}
}