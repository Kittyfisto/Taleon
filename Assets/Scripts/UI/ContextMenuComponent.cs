using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class ContextMenuComponent : MonoBehaviour {

		public Image contentPanel;              // content panel prefab
		public Canvas canvas;                   // link to main canvas, where will be Context Menu
		
		public void CreateContextMenu(List<ContextMenuItem> items, Vector2 position)
		{
			// here we are creating and displaying Context Menu

			Image panel = Instantiate(contentPanel, new Vector3(position.x, position.y, 0), Quaternion.identity) as Image;
			panel.transform.SetParent(canvas.transform);
			panel.transform.SetAsLastSibling();
			panel.rectTransform.anchoredPosition = position;

			foreach (var item in items)
			{
				ContextMenuItem tempReference = item;
				Button button = Instantiate(item.button) as Button;
				Text buttonText = button.GetComponentInChildren(typeof(Text)) as Text;
				buttonText.text = item.text;
				button.onClick.AddListener(delegate { tempReference.action(panel); });
				button.transform.SetParent(panel.transform);
			}
		}
	}
}