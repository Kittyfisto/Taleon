using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts
{
	public class UnitMovementController : MonoBehaviour
	{
		/// <summary>
		///     The target position of this unit.
		///     It will try move until the position has been reached.
		/// </summary>
		public Vector3? Target
		{
			get { return _target; }
			set
			{
				_target = value;
				if (value == null)
				{
					_indicator.Hide();
				}
				else
				{
					_indicator.Show(gameObject, value.Value);
				}
			}
		}

		private MovementIndicatorComponent _indicator;
		private Vector3? _target;

		// Use this for initialization
		private void Start()
		{
			_indicator = transform.FindChild("MovementIndicator").GetComponent<MovementIndicatorComponent>();
			_indicator.Hide();
		}
	}
}