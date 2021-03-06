﻿using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class Tooltip : MonoBehaviour
	{
		private Text _text;
		private TooltipComponent _target;
		private static Tooltip _instance;
		private RectTransform _transform;

		public static void Show(TooltipComponent target)
		{
			if (_instance != null)
			{
				_instance.gameObject.SetActive(true);
				_instance._target = target;
			}
		}

		public static void Hide()
		{
			if (_instance != null)
			{
				_instance.gameObject.SetActive(false);
			}
		}

		// Use this for initialization
		private void Start()
		{
			_text = GetComponentInChildren<Text>();
			_instance = this;
			_transform = GetComponent<RectTransform>();
			Hide();
		}

		// Update is called once per frame
		private void Update()
		{
			var position = Input.mousePosition;
			transform.position = new Vector3(position.x, position.y) +
			                     new Vector3(_transform.rect.width, -_transform.rect.height);
			if (_target != null)
				_text.text = _target.tooltipText;
		}
	}
}