using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	public class BarrelHinge
		: MonoBehaviour
	{
		public void RotateBarrels(float rotationDelta)
		{
			transform.Rotate(Vector3.right * rotationDelta);
		}
	}
}
