using UnityEngine;

namespace Assets.Scripts.AI.FCS.PDS
{
	public struct FiringSolution
	{
		public Vector3 TargetPosition;
		public Vector3 FiringDirection;
		public float DistanceError;
		public float TimeError;
	}
}