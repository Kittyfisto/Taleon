﻿using Assets.Scripts.AI.FCS;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.AI
{
	/// <summary>
	///     Responsible for controlling the entire ship.
	///     Takes high level goals and delegates them to the appropriate sub-system (such as fire control, engine, etc...)
	/// </summary>
	[RequireComponent(typeof(NavigationSystemComponent), typeof(FireControlSystemComponent))]
	public sealed class ShipSystemComponent : MonoBehaviour
	{
		private NavigationSystemComponent _navigation;
		private FireControlSystemComponent _fcs;
		private Vector3? _movementTarget;
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
			}
		}

		public Vector3 CurrentVelocity
		{
			get { return _navigation.CurrentVelocity; }
		}

		public Vector3 WorldFacingDirection
		{
			get { return _navigation.TargetWorldForward; }
		}

		private void Start()
		{
			_navigation = GetComponent<NavigationSystemComponent>();
			_fcs = GetComponent<FireControlSystemComponent>();
			_unit = GetComponent<UnitComponent>();
		}

		public void SetVelocity(VelocitySetting velocity)
		{
			var velocityMagnitude = Mathf.Abs((int)velocity * 10);
			_navigation.SetVelocity(velocityMagnitude);
		}

		public void Rotate(RotationDirection direction)
		{
			_navigation.Rotate(direction);
		}

		public void SetTargetDirection(Vector3 worldTargetDirection)
		{
			_navigation.SetDirection(worldTargetDirection);
		}

		public void TurnAround()
		{
			SetTargetDirection(-WorldFacingDirection);
		}
	}
}