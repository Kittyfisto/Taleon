using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	/// <summary>
	///     Represents a solution of how the turret has to be oriented in order to hit
	///     a designated target.
	/// </summary>
	public struct TargetSolution
	{
		/// <summary>
		/// The required horizontal rotation against <see cref="Vector3.forward"/>.
		/// </summary>
		public float HorizontalAngle;

		/// <summary>
		/// The required vertical rotation again
		/// </summary>
		public float VerticalAngle;
	}
}