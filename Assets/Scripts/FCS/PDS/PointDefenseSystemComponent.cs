using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FCS.PDS
{
	/// <summary>
	/// The point defense system (PDS) is responsible for prioritizing targets based on the threat they pose on the ship
	/// and assigning them to the various point defense platforms of the ship (miniguns, lasers, etc...).
	/// </summary>
	public sealed class PointDefenseSystemComponent : MonoBehaviour
	{
		private List<GameObject> _targets;

		private void Start()
		{
			_targets = new List<GameObject>();
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
					++i;
				}
			}
		}
	}
}