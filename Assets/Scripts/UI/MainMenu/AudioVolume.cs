using UnityEngine;

namespace Assets.Scripts.UI.MainMenu
{
	using UnityEngine.UI;

	public class AudioVolume: MonoBehaviour
	{
		public Slider VolumeSlider;

		void Update()
		{
			AudioListener.volume = VolumeSlider.value;
		}

		private void AdjustVolume(float newVolume)
		{
			AudioListener.volume = newVolume;
		}
	}
}
