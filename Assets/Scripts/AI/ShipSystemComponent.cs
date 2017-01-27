using Assets.Scripts.FCS;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.AI
{
	/// <summary>
	///     Responsible for controlling the entire ship.
	///     Takes high level goals and delegates them to the appropriate sub-system (such as fire control, engine, etc...)
	/// </summary>
	[RequireComponent(typeof(EngineeringSystemComponent), typeof(FireControlSystemComponent))]
	public sealed class ShipSystemComponent : MonoBehaviour
	{
		private EngineeringSystemComponent _engineering;
		private FireControlSystemComponent _fcs;
		private Vector3? _movementTarget;
		private MovementIndicatorComponent _indicator;
		private UnitComponent _unit;

		/// <summary>
		///     The position this ship shall move to.
		/// </summary>
		public Vector3? MovementTarget
		{
			get { return _movementTarget; }
			set
			{
				_movementTarget = value;
				UpdateMovementVisibility();
			}
		}

		private void UpdateMovementVisibility()
		{
			if (_movementTarget == null || !_unit.IsSelected)
			{
				_indicator.Hide();
			}
			else
			{
				_indicator.Show(gameObject, _movementTarget.Value);
			}
		}

		private void Start()
		{
			_engineering = GetComponent<EngineeringSystemComponent>();
			_fcs = GetComponent<FireControlSystemComponent>();
			_unit = GetComponent<UnitComponent>();

			_indicator = transform.FindChild("MovementIndicator").GetComponent<MovementIndicatorComponent>();
			_indicator.Hide();
		}

		private void Update()
		{
			UpdateMovementVisibility();
		}
	}
}