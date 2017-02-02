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

		public Velocity TargetVelocity;
		public Rotation TargetRotation;

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
			if (Input.GetKeyDown(KeyCode.Q))
				SetRotation(TargetRotation - 1);
			if (Input.GetKeyDown(KeyCode.E))
				SetRotation(TargetRotation + 1);
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
			SetVelocity(Velocity.Stop);
		}

		private void SetRotation(Rotation rotation)
		{
			if (rotation < Rotation.FullLeft)
				TargetRotation = Rotation.FullLeft;
			else if (rotation > Rotation.FullRight)
				TargetRotation = Rotation.FullRight;
			else
				TargetRotation = rotation;
			_ship.SetRotation(TargetRotation);
		}

		private void SetVelocity(Velocity velocity)
		{
			if (velocity < Velocity.Stop)
				TargetVelocity = Velocity.Stop;
			else if (velocity > Velocity.FullForwards)
				TargetVelocity = Velocity.FullForwards;
			else
				TargetVelocity = velocity;
			_ship.SetVelocity(TargetVelocity);
		}
	}
}