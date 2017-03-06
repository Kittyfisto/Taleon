using System.Collections.Generic;
using Assets.Scripts.AI.FCS;
using UnityEngine;

namespace Assets.Scripts.AI
{
	/// <summary>
	///     Responsible for finding contacts around this ship.
	///     Performs a preliminary categorization of those contacts and primarily offers them to other ship system,
	///     for example the <see cref="FireControlSystemComponent" />.
	/// </summary>
	[RequireComponent(typeof(FactionComponent))]
	public sealed class RadarSystem
		: MonoBehaviour
	{
		private readonly List<SensorBlip> _blips;
		private readonly List<RocketContact> _rocketContacts;
		private readonly List<ShipSystemComponent> _ships;
		private Faction _faction;

		public float Radius;

		public RadarSystem()
		{
			_blips = new List<SensorBlip>();
			_rocketContacts = new List<RocketContact>();
			_ships = new List<ShipSystemComponent>();
		}

		/// <summary>
		///     All sensor contacts that couldn't be properly resolved by this radar system.
		///     If something is in this list then the system doesn't effectively know
		///     what the object is, just that it's there.
		/// </summary>
		public IEnumerable<SensorBlip> Blips
		{
			get { return _blips; }
		}

		/// <summary>
		///     The list of all known ships in the vincinity.
		/// </summary>
		public IEnumerable<ShipSystemComponent> Ships
		{
			get { return _ships; }
		}

		/// <summary>
		///     The list of all known rockets in the vincinity.
		/// </summary>
		public IEnumerable<RocketContact> RocketContacts
		{
			get { return _rocketContacts; }
		}

		private void Start()
		{
			_faction = GetComponent<FactionComponent>().Faction;
		}

		private void Update()
		{
			_blips.Clear();
			_rocketContacts.Clear();
			_ships.Clear();

			var possibleContacts = Physics.OverlapSphere(transform.position, Radius);
			foreach (var collider in possibleContacts)
			{
				// We are most likely hitting the child (collider/mesh) of a game object that is
				// a ship or rocket.
				// By design, all important ship systems are part of the root game object and therefore we
				// first have to travel to the root before finding those components:
				var root = collider.transform.root;
				var possibleContact = (root != null ? (Component) root : collider).gameObject;
				if (ReferenceEquals(gameObject, possibleContact))
					continue;

				SensorBlip blip;
				if (TryCreateRadarBlip(possibleContact, out blip))
				{
					RocketComponent rocket;
					ShipSystemComponent ship;
					if (blip.TryResolve(out rocket))
						_rocketContacts.Add(CreateContact(rocket));
					else if (blip.TryResolve(out ship))
						_ships.Add(ship);
					else
						_blips.Add(blip);
				}
			}
		}

		private bool TryCreateRadarBlip(GameObject possibleContact, out SensorBlip blip)
		{
			var ship = possibleContact.GetComponent<ShipSystemComponent>();
			if (ship != null)
			{
				var distance = CalculateDistance(possibleContact);
				var crossSection = CalculateCrossSection(possibleContact);
				blip = new SensorBlip(ship, distance, crossSection);
				return true;
			}

			var rocket = possibleContact.GetComponent<RocketComponent>();
			if (rocket != null)
			{
				var distance = CalculateDistance(possibleContact);
				var crossSection = CalculateCrossSection(possibleContact);
				blip = new SensorBlip(rocket, distance, crossSection);
				return true;
			}

			blip = new SensorBlip();
			return false;
		}

		private Length CalculateDistance(GameObject possibleContact)
		{
			var otherTransform = possibleContact.transform;
			var distance = Vector3.Distance(otherTransform.position, transform.position);
			// TODO: Introduce jitter
			return Length.FromUnits(distance);
		}

		private Length CalculateCrossSection(GameObject possibleContact)
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

		private RocketContact CreateContact(RocketComponent rocket)
		{
			var classification = Classify(rocket);
			return new RocketContact(rocket, classification);
		}

		private RocketClassification Classify(RocketComponent rocket)
		{
			var target = rocket.Target;
			var tmp = target.GetComponent<FactionComponent>();
			if (tmp == null)
				return RocketClassification.Neutral;

			var targetFaction = tmp.Faction;
			if (targetFaction == _faction)
				return RocketClassification.Enemy;

			return RocketClassification.Friendly;
		}
	}
}