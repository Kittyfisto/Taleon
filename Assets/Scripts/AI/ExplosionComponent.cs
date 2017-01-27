using UnityEngine;

namespace Assets.Scripts.AI
{
	public sealed class ExplosionComponent
		: MonoBehaviour
	{
		public float maximumLifetime;

		private float _lifetime;

		private void Update()
		{
			_lifetime += Time.deltaTime;
			if (_lifetime >= maximumLifetime)
			{
				Destroy(gameObject);
			}
		}
	}
}