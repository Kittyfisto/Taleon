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
		private readonly List<GameObject> _targets;
		private readonly List<PointDefenseTurretComponent> _turrets;

		public PointDefenseSystemComponent()
		{
			_targets = new List<GameObject>();
			_turrets = new List<PointDefenseTurretComponent>();
		}

		private void Start()
		{
			_turrets.AddRange(GetComponentsInChildren<PointDefenseTurretComponent>());
		}

		public void SetTargets(IEnumerable<GameObject> targets)
		{
			_targets.Clear();
			_targets.AddRange(targets);
		}

		public void AddTarget(GameObject target)
		{
			if (!_targets.Contains(target))
			{
				_targets.Add(target);
			}
		}

		private void Update()
		{
			for (int i = 0; i < _targets.Count;)
			{
				var target = _targets[i];
				if (target == null)
				{
					// A target may no longer exist (because we've destroyed it, yeah!),
					// so it must be removed from the list of targets...
					_targets.RemoveAt(i);
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
			// Not only should we try to use a turret that can fire at the target, but
			// we should also chose a turret that can fire the soonest.
			var availableTurrets = _turrets.Where(x => x.target == null);
			var bestTurret = availableTurrets.Aggregate((i1, i2) => i1.TimeToNextShot > i2.TimeToNextShot ? i2 : i1);
			if (bestTurret != null)
			{
				bestTurret.target = target;
			}
		}
	}
}