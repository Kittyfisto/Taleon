using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI
{
	/// <summary>
	///     Responsible for setting units to being hovered, selected and issuing commands via user input.
	/// </summary>
	public class PlayerUnitController : MonoBehaviour
	{
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
			}

			if (Input.GetButtonUp("Cancel"))
			{
				UnselectAll();
			}
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
	}
}