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

		public int TargetVelocity;

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
		}

		private void SetVelocity(int velocity)
		{
			const int minimum = -4;
			const int maximum = 4;

			TargetVelocity = Mathf.Clamp(velocity, minimum, maximum);
			_ship.SetVelocity(TargetVelocity * 10);
		}
	}
}