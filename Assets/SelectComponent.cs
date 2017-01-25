using UnityEngine;

[RequireComponent(typeof(Outline))]
public class SelectComponent : MonoBehaviour
{
	private Outline _outline;
	private bool _isSelected;
	private bool _isHovering;

	/// <summary>
	///     Whether or not this object has been selected by the player.
	/// </summary>
	public bool isSelected
	{
		get { return _isSelected; }
		set
		{
			if (value == _isSelected)
				return;

			_isSelected = value;
			UpdateOutline();
		}
	}

	// Use this for initialization
	private void Start()
	{
		_outline = GetComponent<Outline>();
		_outline.Disable();
	}

	private void OnMouseEnter()
	{
		_isHovering = true;
		UpdateOutline();
	}

	private void OnMouseExit()
	{
		_isHovering = false;
		UpdateOutline();
	}

	private void UpdateOutline()
	{
		if (_isHovering || _isSelected)
			_outline.Enable();
		else
			_outline.Disable();
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(0))
		{
			isSelected = true;
		}
	}
}