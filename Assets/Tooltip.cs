using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
	private Text _text;
	private GameObject _target;
	private static Tooltip _instance;
	private RectTransform _transform;

	public static void Show(GameObject target)
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
		if (_target != null)
			_text.text = _target.name;

		transform.position = new Vector3(position.x, position.y) +
		                     new Vector3(_transform.rect.width, -_transform.rect.height);
	}
}