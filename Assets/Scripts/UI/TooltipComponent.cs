using System;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class TooltipComponent : MonoBehaviour
	{
		private readonly Timer _timer;

		public string tooltipText;

		public TooltipComponent()
		{
			_timer = new Timer
			{
				OneShot = true,
				Interval = TimeSpan.FromMilliseconds(500)
			};
			_timer.Elapsed += TimerOnElapsed;
		}

		private void TimerOnElapsed()
		{
			Tooltip.Show(this);
		}

		private void OnMouseEnter()
		{
			_timer.Start();
		}

		private void OnMouseExit()
		{
			_timer.Stop();
			Tooltip.Hide();
		}

		// Update is called once per frame
		private void Update()
		{
			_timer.Update();
		}
	}
}