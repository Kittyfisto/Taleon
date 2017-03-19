using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.UI.MainMenu
{
	using UnityEngine.Audio;

	public class SettingsBehaviour : MonoBehaviour
	{
		private bool _isLoaded;
		public Slider MasterVolumeSlider;
		public Slider MusicVolumeSlider;
		public Slider SfxVolumeSlider;

		public AudioMixer MainMixer;

		void Start()
		{
			if (!_isLoaded)
			{
				if (PlayerPrefs.HasKey("masterVol"))
				{
					var value = PlayerPrefs.GetFloat("masterVol");
					MasterVolumeSlider.value = value;
					MainMixer.SetFloat("masterVol", value);
				}
				if (PlayerPrefs.HasKey("musicVol"))
				{
					var value = PlayerPrefs.GetFloat("musicVol");
					MusicVolumeSlider.value = value;
					MainMixer.SetFloat("musicVol", value);
				}
				if (PlayerPrefs.HasKey("sfxVol"))
				{
					var value = PlayerPrefs.GetFloat("sfxVol");
					SfxVolumeSlider.value = value;
					MainMixer.SetFloat("sfxVol", value);
				}
				_isLoaded = true;
			}
		}

	}
}
