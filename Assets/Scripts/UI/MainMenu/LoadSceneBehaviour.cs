using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.MainMenu
{
	public class LoadSceneBehaviour : MonoBehaviour
	{
		public void OnCloseButton()
		{
			SceneManager.LoadScene("Chapters/Demo", LoadSceneMode.Additive);
		}
	}
}