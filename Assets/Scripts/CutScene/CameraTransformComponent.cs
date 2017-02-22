using UnityEngine;

namespace Assets.Scripts.CutScene
{
	public sealed class CameraTransformComponent
		: MonoBehaviour
	{
		public float DisplayLength = 10;

		private void Update()
		{
			Debug.DrawRay(transform.position, transform.forward*DisplayLength, Color.red);
		}
	}
}