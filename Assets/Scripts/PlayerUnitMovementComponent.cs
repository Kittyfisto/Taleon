using System.Collections.Generic;
using System.Linq;
using Assets;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class PlayerUnitMovementComponent : MonoBehaviour
{
	private bool _isMovementOpen;
	private MeshCollider _mesh;
	private GameObject _indicator;
	private Transform _line;

	// Use this for initialization
	private void Start()
	{
		_mesh = GetComponent<MeshCollider>();
		_indicator = GameObject.Find("MovementIndicator");
		_line = GameObject.Find("MovementLine").GetComponent<Transform>();
	}

	// Update is called once per frame
	private void Update()
	{
		var selectables = FindObjectsOfType<SelectComponent>().FirstOrDefault(x => x.isSelected);
		if (selectables != null)
		{
			HandleInput(selectables);
		}

		ShowMovementIndicator(selectables);
	}

	private void HandleInput(SelectComponent selectable)
	{
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

		if (Input.GetMouseButtonUp(MouseButtons.Left))
		{
			if (_isMovementOpen)
			{
				// We want to move units to the current mouse position...
				MoveUnits(selectable);
			}
		}
	}

	private void MoveUnits(SelectComponent selectable)
	{
		
	}

	private void ShowMovementIndicator(SelectComponent selectables)
	{
		Vector3? worldPosition;
		if (_isMovementOpen && (worldPosition = MouseWorldPosition) != null)
		{
			//var relativePosition = transform.InverseTransformPoint(worldPosition.Value);
			_indicator.transform.position = worldPosition.Value;
			_indicator.SetActive(true);

			var position = selectables.transform.position;
			var delta = worldPosition.Value - position;
			var distance = delta.magnitude;

			_line.localScale = new Vector3(_line.localScale.x,
				distance/2,
				_line.localScale.z);
			_line.up = delta;
			_line.position = position + delta / 2;

			_line.gameObject.SetActive(true);
		}
		else
		{
			_indicator.SetActive(false);
			_line.gameObject.SetActive(false);
		}
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