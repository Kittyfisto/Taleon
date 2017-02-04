using UnityEngine;

namespace Assets.Scripts.AI
{
	public sealed class FactionComponent
		: MonoBehaviour
	{
		/// <summary>
		///     The faction this game object (and its children) belong to.
		/// </summary>
		public Faction Faction;
	}
}