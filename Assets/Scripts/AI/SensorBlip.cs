using Assets.Scripts.AI.FCS;
using UnityEngine;

namespace Assets.Scripts.AI
{
	public struct SensorBlip
	{
		private readonly Length _distance;
		private readonly Length _crossSection;
		private readonly RocketComponent _rocket;
		private readonly GameObject _gameObject;
		private readonly ShipSystemComponent _ship;

		public SensorBlip(RocketComponent rocket, Length distance, Length crossSection)
		{
			_rocket = rocket;
			_gameObject = rocket.gameObject;
			_distance = distance;
			_crossSection = crossSection;

			_ship = null;
		}

		public SensorBlip(ShipSystemComponent ship, Length distance, Length crossSection)
		{
			_ship = ship;
			_gameObject = ship.gameObject;
			_distance = distance;
			_crossSection = crossSection;

			_rocket = null;
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