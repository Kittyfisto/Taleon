using UnityEngine;

namespace Assets.Scripts.UI.MainMenu
{
	public class CloseBehaviour : MonoBehaviour
	{
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
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}
