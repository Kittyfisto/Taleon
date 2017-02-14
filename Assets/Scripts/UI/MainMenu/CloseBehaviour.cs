using UnityEngine;

namespace Assets.Scripts.UI.MainMenu
{
	public class CloseBehaviour : MonoBehaviour
	{
		public void OnCloseButton()
		{
			Application.Quit();
		}

		void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				Application.Quit();
			}
		}
	}
}
