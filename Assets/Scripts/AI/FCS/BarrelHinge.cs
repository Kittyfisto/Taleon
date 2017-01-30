using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	public class BarrelHinge
		: MonoBehaviour
	{
		public void ApplyRotation(Quaternion rotationDelta)
		{
			var rotation = transform.localRotation * rotationDelta;
			transform.localRotation = rotation;
		}
	}
}
