using UnityEngine;

namespace Assets.Scripts.UI.MainMenu
{
	using UnityEngine.Audio;
	using UnityEngine.UI;

	public class CloseBehaviour : MonoBehaviour
	{
		public AudioMixer MainMixer;

		public void OnCloseButton()
		{
			QuitApplication();
		}

		void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				QuitApplication();
			}
		}

		private void QuitApplication()
		{
			float value;
			MainMixer.GetFloat("masterVol", out value);
			PlayerPrefs.SetFloat("masterVol", value);

			MainMixer.GetFloat("musicVol", out value);
			PlayerPrefs.SetFloat("musicVol", value);

			MainMixer.GetFloat("sfxVol", out value);
			PlayerPrefs.SetFloat("sfxVol", value);
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}
