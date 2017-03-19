using UnityEngine;

namespace Assets.Scripts.UI.MainMenu
{
	using UnityEngine.Audio;
	using UnityEngine.UI;

	public class AudioVolume: MonoBehaviour
	{
		public Slider MasterVolumeSlider;
		public Slider MusicVolumeSlider;
		public Slider SfxVolumeSlider;

		public AudioMixer mainMixer;


		public void SetMusicLevel()
		{
			//mainMixer.SetFloat("musicVol", musicLevel);
			mainMixer.SetFloat("musicVol", MusicVolumeSlider.value);
		}

		public void SetSfxLevel()
		{
			//mainMixer.SetFloat("sfxVol", sfxLevel);
			mainMixer.SetFloat("sfxVol", SfxVolumeSlider.value);
		}

		public void SetMasterLevel()
		{

			// mainMixer.SetFloat("masterVol", masterLevel);
			mainMixer.SetFloat("masterVol", MasterVolumeSlider.value);
		}
	}
}
