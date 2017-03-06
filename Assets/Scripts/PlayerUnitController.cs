using Assets.Scripts.AI;
using UnityEngine;

namespace Assets.Scripts
{
	/// <summary>
	///     Responsible for setting units to being hovered, selected and issuing commands via user input.
	/// </summary>
	[RequireComponent(typeof(ShipSystemComponent))]
	public class PlayerUnitController : MonoBehaviour
	{
		private ShipSystemComponent _ship;

		public VelocitySetting TargetVelocity;

		private void Start()
		{
			_ship = GetComponent<ShipSystemComponent>();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.W))
				SetVelocity(TargetVelocity + 1);
			if (Input.GetKeyDown(KeyCode.S))
				SetVelocity(TargetVelocity - 1);
			if (Input.GetKey(KeyCode.A))
				Rotate(RotationDirection.Left);
			if (Input.GetKey(KeyCode.D))
				Rotate(RotationDirection.Right);
			if (Input.GetMouseButton(MouseButtons.Left))
				ChangeDirectionToMouse();
		}

		private void ChangeDirectionToMouse()
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var direction = ray.direction;
			_ship.SetTargetDirection(direction);
		}

		public void TurnAround()
		{
			_ship.TurnAround();
		}

		public void StopShip()
		{
			SetVelocity(VelocitySetting.Stop);
		}

		private void Rotate(RotationDirection direction)
		{
			_ship.Rotate(direction);
		}

		private void SetVelocity(VelocitySetting velocity)
		{
			if (velocity < VelocitySetting.Stop)
				TargetVelocity = VelocitySetting.Stop;
			else if (velocity > VelocitySetting.FullForwards)
				TargetVelocity = VelocitySetting.FullForwards;
			else
				TargetVelocity = velocity;
			_ship.SetVelocity(TargetVelocity);
		}
	}
}