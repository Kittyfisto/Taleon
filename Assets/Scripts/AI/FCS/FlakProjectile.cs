using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	public class FlakProjectile
		: ProjectileComponent
	{
		/// <summary>
		///     The explosion that is triggered when this projectile
		///     hits the intended target and/or range.
		/// </summary>
		public GameObject ExplosionPrefab;

		protected override void Update()
		{
			base.Update();

			var distanceCovered = Vector3.Distance(transform.position, StartPosition);
			if (distanceCovered >= Solution.InterceptionDistance)
			{
				Explode(null);
			}
		}

		protected override void Explode(GameObject otherGameObject)
		{
			var explosion = Instantiate(ExplosionPrefab);
			explosion.transform.position = transform.position;

			base.Explode(otherGameObject);
		}
	}
}