using System;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	[Serializable]
	public class ContextMenuItem
	{
		// this class - just a box to some data

		public string text;             // text to display on button
		public Button button;           // sample button prefab
		public Action<Image> action;    // delegate to method that needs to be executed when button is clicked

		public ContextMenuItem(string text, Button button, Action<Image> action)
		{
			this.text = text;
			this.button = button;
			this.action = action;
		}
	}
}