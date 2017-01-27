using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AI;
using UnityEngine;

namespace Assets.Scripts.UI
{
	/// <summary>
	///     Responsible for setting units to being hovered, selected and issuing commands via user input.
	/// </summary>
	public class PlayerUnitController : MonoBehaviour
	{
		private bool _isMovementOpen;
		private MeshCollider _mesh;
		private MovementIndicatorComponent _indicator;

		private void Start()
		{
			_mesh = GameObject.Find("PlayerUnitMovementPlane").GetComponent<MeshCollider>();
			_indicator = GameObject.Find("PlayerMovementIndicator").GetComponent<MovementIndicatorComponent>();
		}

		private void Update()
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var hoveredUnits = Physics.RaycastAll(ray).Where(IsUnit).Select(x => x.transform.GetComponent<UnitComponent>()).ToList();
			var allUnits = FindObjectsOfType<UnitComponent>();
			var selectedUnits = allUnits.Where(x => x.IsSelected);
			var selectedUnit = selectedUnits.FirstOrDefault();

			foreach (var unit in allUnits)
				unit.IsHovered = false;

			foreach (var unit in hoveredUnits)
				unit.GetComponent<UnitComponent>().IsHovered = true;

			if (Input.GetMouseButtonUp(MouseButtons.Left))
			{
				if (hoveredUnits.Any())
				{
					SelectUnits(hoveredUnits);
				}

				if (selectedUnit != null && _isMovementOpen)
					MoveUnits(selectedUnit);
			}
			if (Input.GetMouseButtonUp(MouseButtons.Right))
			{
				if (!_isMovementOpen)
				{
					// We want to begin showing a movement indicator that shows the player where he would
					// send his selected units.
					_isMovementOpen = true;
				}
				else
				{
					// We want to cancel our movement...
					_isMovementOpen = false;
				}
			}
			if (Input.GetButtonUp("Cancel"))
			{
				UnselectAll();
			}

			ShowMovementIndicator(selectedUnit);
		}

		private void SelectUnits(IEnumerable<UnitComponent> units)
		{
			if (units.Any())
			{
				UnselectAll();
				foreach (var unit in units)
				{
					var selectable = unit.GetComponent<UnitComponent>();
					selectable.IsSelected = true;
				}
			}
		}

		public void UnselectAll()
		{
			var objects = FindObjectsOfType<UnitComponent>();
			foreach (var component in objects)
				component.IsSelected = false;
		}

		private bool IsUnit(RaycastHit hit)
		{
			var go = hit.transform.gameObject;
			var component = go.GetComponent<UnitComponent>();
			return component != null;
		}
		
		private void MoveUnits(UnitComponent selectable)
		{
			var worldPosition = MouseWorldPosition;
			if (worldPosition != null)
			{
				var controller = selectable.GetComponent<ShipSystemComponent>();
				controller.MovementTarget = worldPosition.Value;
			}
			EndMovement();
		}

		private void ShowMovementIndicator(UnitComponent selectables)
		{
			Vector3? worldPosition;
			if (selectables != null && _isMovementOpen && (worldPosition = MouseWorldPosition) != null)
			{
				_indicator.Show(selectables.gameObject, worldPosition.Value);
			}
			else
			{
				EndMovement();
			}
		}

		private void EndMovement()
		{
			_indicator.Hide();
			_isMovementOpen = false;
		}

		private Vector3? MouseWorldPosition
		{
			get
			{
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				if (_mesh.Raycast(ray, out hitInfo, 500000))
				{
					var worldPoint = hitInfo.point;
					return worldPoint;
				}

				return null;
			}
		}
	}
}