using UnityEngine;

namespace Assets.Scripts.UI
{
	public class CutscenePanel
		: MonoBehaviour
	{
		private RectTransform _bottom;
		private RectTransform _rectTransform;
		private RectTransform _top;

		public float BarHeight;

		// Use this for initialization
		private void Start()
		{
			_top = transform.FindChild("BlackBar1").GetComponent<RectTransform>();
			_bottom = transform.FindChild("BlackBar2").GetComponent<RectTransform>();

			_rectTransform = GetComponent<RectTransform>();
		}

		// Update is called once per frame
		private void OnGUI()
		{
			var width = Screen.width;
			var height = Screen.height;

			_top.offsetMin = new Vector2(0, height-BarHeight);
			_top.offsetMax = new Vector2(width, 0);

			_bottom.offsetMin = new Vector2(0, 0);
			_bottom.offsetMax = new Vector2(width, BarHeight-height);
		}
	}
}