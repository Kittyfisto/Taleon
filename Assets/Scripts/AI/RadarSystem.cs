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
		private readonly List<RocketContact> _rocketContacts;
		private readonly List<ShipSystemComponent> _ships;

		public float Radius;
		private Faction _faction;

		public RadarSystem()
		{
			_rocketContacts = new List<RocketContact>();
			_ships = new List<ShipSystemComponent>();
		}

		public IEnumerable<ShipSystemComponent> Ships
		{
			get { return _ships; }
		}

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

				var rocket = possibleContact.GetComponent<RocketComponent>();
				if (rocket != null)
				{
					_rocketContacts.Add(CreateContact(rocket));
				}
				else
				{
					var ship = possibleContact.GetComponent<ShipSystemComponent>();
					if (ship != null)
						_ships.Add(ship);
				}
			}
		}

		private RocketContact CreateContact(RocketComponent rocket)
		{
			var classification = Classify(rocket);
			return new RocketContact(rocket, classification);
		}

		private RocketClassification Classify(RocketComponent rocket)
		{
			var target = rocket.target;
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