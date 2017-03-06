using Assets.Scripts.AI.FCS;
using UnityEngine;

namespace Assets.Scripts.AI
{
	public struct SensorBlip
	{
		private readonly Length _distance;
		private readonly Length _crossSection;
		private readonly Velocity _deltaV;
		private readonly RocketComponent _rocket;
		private readonly GameObject _gameObject;
		private readonly ShipSystemComponent _ship;

		public static bool TryCreate(ShipSystemComponent shipSystem, GameObject possibleContact, out SensorBlip blip)
		{
			var ship = possibleContact.GetComponent<ShipSystemComponent>();
			if (ship != null)
			{
				blip = new SensorBlip(shipSystem, ship);
				return true;
			}

			var rocket = possibleContact.GetComponent<RocketComponent>();
			if (rocket != null)
			{
				blip = new SensorBlip(shipSystem, rocket);
				return true;
			}

			blip = new SensorBlip();
			return false;
		}

		private static Length CalculateDistance(ShipSystemComponent ship, GameObject possibleContact)
		{
			var otherTransform = possibleContact.transform;
			var distance = Vector3.Distance(otherTransform.position, ship.transform.position);
			// TODO: Introduce jitter
			return Length.FromUnits(distance);
		}

		private static Length CalculateCrossSection(GameObject possibleContact)
		{
			var blipCollider = possibleContact.GetComponentInChildren<Collider>();
			if (blipCollider == null)
				return Length.Zero;

			var bb = blipCollider.bounds;
			// TODO: Actually calculate proper cross section...
			// TODO: Introduce jitter
			var cross = bb.extents.magnitude * 2;
			return Length.FromUnits(cross);
		}

		private SensorBlip(ShipSystemComponent shipSystem, GameObject gameObject)
		{
			_rocket = null;
			_ship = null;
			_deltaV = Velocity.Zero;

			_gameObject = gameObject;
			_distance = CalculateDistance(shipSystem, gameObject);
			_crossSection = CalculateCrossSection(gameObject);
		}

		private SensorBlip(ShipSystemComponent shipSystem, RocketComponent rocket)
			: this(shipSystem, rocket.gameObject)
		{
			_rocket = rocket;
		}

		private SensorBlip(ShipSystemComponent ship, ShipSystemComponent contact)
			: this(ship, contact.gameObject)
		{
			_ship = contact;
			_deltaV = Velocity.Difference(ship.CurrentVelocity, contact.CurrentVelocity);
		}

		public Velocity DeltaV
		{
			get { return _deltaV; }
		}

		public Length Distance
		{
			get { return _distance; }
		}

		public Length CrossSection
		{
			get { return _crossSection; }
		}

		public GameObject GameObject
		{
			get { return _gameObject; }
		}

		public Transform Transform
		{
			get { return GameObject.transform; }
		}

		public override string ToString()
		{
			return string.Format("Distance: {0}, Cross-section: {1}", _distance, _crossSection);
		}

		public bool TryResolve(out RocketComponent rocket)
		{
			if (_distance > Length.FromKilometers(10))
			{
				rocket = null;
				return false;
			}

			return (rocket = _rocket) != null;
		}

		public bool TryResolve(out ShipSystemComponent ship)
		{
			if (_distance >= Length.FromKilometers(1))
			{
				ship = null;
				return false;
			}

			return (ship = _ship) != null;
		}
	}
}