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
	public sealed class RadarSystem
		: MonoBehaviour
	{
		private readonly List<RocketComponent> _rocketContacts;
		private readonly List<ShipSystemComponent> _ships;

		public float Radius;

		public RadarSystem()
		{
			_rocketContacts = new List<RocketComponent>();
			_ships = new List<ShipSystemComponent>();
		}

		public IEnumerable<ShipSystemComponent> Ships
		{
			get { return _ships; }
		}

		public IEnumerable<RocketComponent> RocketContacts
		{
			get { return _rocketContacts; }
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
					_rocketContacts.Add(rocket);
				}
				else
				{
					var ship = possibleContact.GetComponent<ShipSystemComponent>();
					if (ship != null)
						_ships.Add(ship);
				}
			}
		}
	}
}