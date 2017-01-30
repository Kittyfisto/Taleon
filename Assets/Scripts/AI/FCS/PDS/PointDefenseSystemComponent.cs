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
		private readonly List<Threat> _sortedThreats;
		private readonly List<AbstractGunPlatform> _turrets;
		private readonly ThreatComparer _threatComparer;
		private readonly HashSet<GameObject> _threats;

		public PointDefenseSystemComponent()
		{
			_sortedThreats = new List<Threat>();
			_threats = new HashSet<GameObject>();
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
			_sortedThreats.Clear();
			_sortedThreats.AddRange(targets);
			_sortedThreats.Sort(_threatComparer);

			_threats.Clear();
			foreach (var target in _sortedThreats)
			{
				_threats.Add(target.GameObject);
			}
		}
		
		private void Update()
		{
			RemoveOldThreats();
			AssignCurrentThreats();
		}

		private void RemoveOldThreats()
		{
			foreach (var turret in _turrets)
			{
				var target = turret.Target;
				if (target != null)
				{
					if (!IsThreat(target))
					{
						turret.Target = null;
					}
				}
			}
		}

		private bool IsThreat(GameObject target)
		{
			return _threats.Contains(target);
		}

		private void AssignCurrentThreats()
		{
			foreach (var threat in _sortedThreats)
			{
				AssignToPds(threat.GameObject);
			}
		}

		private void AssignToPds(GameObject target)
		{
			// Conditions for chosing a turret:
			// 1) The turret doesn't have a target already
			// Conditions for chosing the best turret:
			// 1) The turret has to wait for a minimum amount of time until it can fire
			// 2) The turret can fire at the threat for the longest time (not implemented yet)

			var availableTurrets = _turrets.Where(x => x.Target == null).OrderBy(x => x.TimeToNextShot).ToList();
			foreach (var turret in availableTurrets)
			{
				if (turret.TryAssignTarget(target))
				{
					break;
				}
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