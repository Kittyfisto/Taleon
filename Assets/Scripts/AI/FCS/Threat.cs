using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	public struct Threat
	{
		/// <summary>
		///     The actual gameobject of this threat.
		/// </summary>
		public GameObject GameObject;

		/// <summary>
		///     The distance of the threat to this ship.
		/// </summary>
		public float Distance;
	}
}