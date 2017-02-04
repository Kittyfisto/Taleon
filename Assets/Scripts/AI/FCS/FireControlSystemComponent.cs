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
	[RequireComponent(typeof(PointDefenseSystemComponent), typeof(RadarSystem))]
	public sealed class FireControlSystemComponent : MonoBehaviour
	{
		private PointDefenseSystemComponent _pds;
		private RadarSystem _radar;

		private void Start()
		{
			_pds = GetComponent<PointDefenseSystemComponent>();
			_radar = GetComponent<RadarSystem>();
		}

		private void Update()
		{
			var threats = FindThreats();
			_pds.SetTargets(threats);
		}

		private IEnumerable<Threat> FindThreats()
		{
			// For now, threats are rockets that are aimed at us...
			// TODO: This part should be changed to simulate imperfect knowledge
			var rockets = FindObjectsOfType<RocketComponent>()
				.Where(TargetsUs)
				.Select(CreateThreat).ToList();
			return rockets;
		}

		private Threat CreateThreat(RocketComponent rocket)
		{
			return new Threat
			{
				GameObject = rocket.gameObject,
				Distance = Vector3.Distance(rocket.transform.position, transform.position)
			};
		}

		private bool TargetsUs(RocketComponent rocket)
		{
			if (!rocket.IsActivated)
				return false;

			if (rocket.target != gameObject)
				return false;

			if (!FliesTowardsUs(rocket))
				return false;

			return true;
		}

		/// <summary>
		/// Tests if the given rocket is flying towards us.
		/// </summary>
		/// <param name="rocket"></param>
		/// <returns></returns>
		private bool FliesTowardsUs(RocketComponent rocket)
		{
			var body = rocket.GetComponent<Rigidbody>();
			var velocityDirection = body.velocity.normalized;
			var delta = (transform.position - rocket.transform.position).normalized;
			var angle = Vector3.Angle(velocityDirection, delta);
			if (angle >= 90)
				return false;

			return true;
		}
	}
}