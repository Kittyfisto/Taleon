using UnityEngine;

namespace Assets.Scripts.UI.MainMenu
{
	using UnityEngine.UI;

	public class MusicVolume : MonoBehaviour
	{
		public Slider VolumeSlider;

		void Update()
		{
			AudioListener.volume = VolumeSlider.value;
		}
	}
}
