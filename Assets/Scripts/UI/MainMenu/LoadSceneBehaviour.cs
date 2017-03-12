using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.MainMenu
{
	public class LoadSceneBehaviour : MonoBehaviour
	{
		public void OnStartGameButton()
		{
			SceneManager.LoadScene("Scenes/Chapters/Chapter 01 - Intro", LoadSceneMode.Single);
		}

	}
}