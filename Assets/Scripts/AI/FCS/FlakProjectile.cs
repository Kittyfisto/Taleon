using System.Linq;
using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	[RequireComponent(typeof(SphereCollider))]
	public class FlakProjectile
		: ProjectileComponent
	{
		/// <summary>
		///     The explosion that is triggered when this projectile
		///     hits the intended target and/or range.
		/// </summary>
		public GameObject ExplosionPrefab;

		/// <summary>
		///     The minimum amount of distance this projectile must travel before its fuse activates.
		/// </summary>
		public float MinimumFuseRange;

		private SphereCollider _fuse;

		protected void Start()
		{
			_fuse = GetComponent<SphereCollider>();
			_fuse.enabled = false;
		}

		protected override void Update()
		{
			base.Update();

			if (DistanceTravelled >= MinimumFuseRange)
				_fuse.enabled = true;

			var distanceCovered = Vector3.Distance(transform.position, StartPosition);
			if (distanceCovered >= Solution.InterceptionDistance)
				Explode();
		}

		protected override void OnHit(GameObject otherGameObject)
		{
			var rocket = otherGameObject.GetComponent<RocketComponent>();
			if (rocket == null)
				return;

			Explode();
		}

		private void Explode()
		{
			if (!_fuse.enabled)
				return;

			var explosion = Instantiate(ExplosionPrefab);
			explosion.transform.position = transform.position;
			
			var objectsInRange = Physics.OverlapSphere(transform.position, _fuse.radius);
			var rockets = objectsInRange.Select(x => x.gameObject.GetComponent<RocketComponent>())
				.Where(x => x != null).ToList();
			foreach (var tmp in rockets)
				tmp.TakeDamage(this);

			Destroy(gameObject);
		}
	}
}