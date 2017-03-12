using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.MainMenu
{
	public class SelectOnInput : MonoBehaviour
	{
		public EventSystem EventSystem;
		public GameObject SelectedObject;

		private bool isButtonSelected;

		private void Update()
		{
			if (Input.GetAxisRaw("Vertical") != 0 && !isButtonSelected)
			{
				EventSystem.SetSelectedGameObject(SelectedObject);
				isButtonSelected = true;
			}
		}

		private void OnDisable()
		{
			isButtonSelected = false;
		}
	}
}