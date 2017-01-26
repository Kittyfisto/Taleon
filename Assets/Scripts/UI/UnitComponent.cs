using UnityEngine;

namespace Assets.Scripts.UI
{
	/// <summary>
	///     Responsible for highlighting selected units.
	/// </summary>
	[RequireComponent(typeof(Outline))]
	public class UnitComponent : MonoBehaviour
	{
		private bool _isSelected;
		private Outline _outline;
		private bool _isHovered;

		/// <summary>
		/// Whether or not this object is being hovered by the player.
		/// </summary>
		public bool IsHovered
		{
			get { return _isHovered; }
			set
			{
				_isHovered = value;
				UpdateOutline();
			}
		}

		/// <summary>
		///     Whether or not this object has been selected by the player.
		/// </summary>
		public bool IsSelected
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

		private void UpdateOutline()
		{
			if (IsHovered || _isSelected)
				_outline.Enable();
			else
				_outline.Disable();
		}
	}
}