using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
	public class QuitGameComponent
		: MonoBehaviour
	{
		public GameObject QuitDialog;

		// Update is called once per frame
		void Update ()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				ToggleVisibility();
			}
		}

		private void ToggleVisibility()
		{
			QuitDialog.SetActive(!QuitDialog.activeInHierarchy);
		}

		public void OnQuit()
		{
			SceneManager.LoadScene("Scenes/MainMenu", LoadSceneMode.Single);
		}

		public void OnContinue()
		{
			QuitDialog.SetActive(false);
		}
	}
}
