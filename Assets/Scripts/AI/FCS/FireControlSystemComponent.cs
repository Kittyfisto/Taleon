using Assets.Scripts.FCS.PDS;
using UnityEngine;

namespace Assets.Scripts.FCS
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
	}
}