using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AI.FCS.PDS;
using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	/// <summary>
	/// The fire control system is aware of all hostile targets that have been reported by the sensors.
	/// It is responsible for deciding which weapons to on which target:
	/// - Rockets, debris, etc.. is reported to the point defense system
	/// - 
	/// </summary>
	[RequireComponent(typeof(PointDefenseSystemComponent))]
	public sealed class FireControlSystemComponent : MonoBehaviour
	{
		private PointDefenseSystemComponent _pds;

		private void Start()
		{
			_pds = GetComponent<PointDefenseSystemComponent>();
		}

		private void Update()
		{
			var threats = FindThreats();
			_pds.SetTargets(threats);
		}

		private IEnumerable<GameObject> FindThreats()
		{
			// For now, threats are rockets that are aimed at us...
			// TODO: This part should be changed to simulate imperfect knowledge
			var rockets = FindObjectsOfType<RocketComponent>()
				.Where(TargetsUs)
				.Select(x => x.gameObject).ToList();
			return rockets;
		}

		private bool TargetsUs(RocketComponent rocket)
		{
			return rocket.target == gameObject;
		}
	}
}