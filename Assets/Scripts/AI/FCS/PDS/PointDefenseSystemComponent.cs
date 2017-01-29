using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.AI.FCS.PDS
{
	/// <summary>
	/// The point defense system (PDS) is responsible for prioritizing targets based on the threat they pose on the ship
	/// and assigning them to the various point defense platforms of the ship (miniguns, lasers, etc...).
	/// </summary>
	public sealed class PointDefenseSystemComponent : MonoBehaviour
	{
		private readonly List<Threat> _threats;
		private readonly List<AbstractGunPlatform> _turrets;
		private readonly ThreatComparer _threatComparer;

		public PointDefenseSystemComponent()
		{
			_threats = new List<Threat>();
			_turrets = new List<AbstractGunPlatform>();
			_threatComparer = new ThreatComparer();
		}

		private void Start()
		{
			_turrets.AddRange(GetComponentsInChildren<PointDefenseTurretComponent>());
			_turrets.AddRange(GetComponentsInChildren<FlakTurretComponent>());
		}

		public void SetTargets(IEnumerable<Threat> targets)
		{
			_threats.Clear();
			_threats.AddRange(targets);
			_threats.Sort(_threatComparer);
		}

		public void AddTarget(Threat target)
		{
			if (!_threats.Contains(target))
			{
				_threats.Add(target);
			}
		}

		private void Update()
		{
			for (int i = 0; i < _threats.Count;)
			{
				var threat = _threats[i];
				var target = threat.GameObject;
				if (target == null)
				{
					// A target may no longer exist (because we've destroyed it, yeah!),
					// so it must be removed from the list of targets...
					_threats.RemoveAt(i);
				}
				else
				{
					AssignTarget(target);
					++i;
				}
			}
		}

		private void AssignTarget(GameObject target)
		{
			// Conditions for chosing a turret:
			// 1) The turret doesn't have a target already
			// Conditions for chosing the best turret:
			// 1) The turret has to wait for a minimum amount of time until it can fire
			// 2) The turret can fire at the threat for the longest time (not implemented yet)

			var availableTurrets = _turrets.Where(x => x.Target == null).ToList();
			if (availableTurrets.Any())
			{
				var bestTurret = availableTurrets.Aggregate((i1, i2) => i1.TimeToNextShot > i2.TimeToNextShot ? i2 : i1);
				if (bestTurret != null)
				{
					bestTurret.Target = target;
				}
			}
			else
			{
				// Well, this is a problem...
			}
		}

		/// <summary>
		/// Responsible for comparing the threat two gameobjects pose to this ship.
		/// </summary>
		private class ThreatComparer : IComparer<Threat>
		{
			public int Compare(Threat x, Threat y)
			{
				return x.Distance.CompareTo(y.Distance);
			}
		}
	}
}