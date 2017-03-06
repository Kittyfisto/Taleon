using Assets.Scripts.AI;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class RadarSystemOverlay
		: MonoBehaviour
	{
		private RadarSystem _radar;

		public Texture ShipIcon;
		public Texture FriendlyRocketIcon;
		public Texture NeutralRocketIcon;
		public Texture EnemyRocketIcon;
		public Texture RadarBlipIcon;

		// Use this for initialization
		private void Start()
		{
			var playerUnitController = FindObjectOfType<PlayerUnitController>();
			_radar = playerUnitController.GetComponent<RadarSystem>();
		}

		// Update is called once per frame
		private void OnGUI()
		{
			var camera = Camera.current;

			foreach (var ship in _radar.Ships)
			{
				Vector2 position;
				if (IsVisible(camera, ship, out position))
				{
					DrawIcon(position, ShipIcon);
				}
			}

			foreach (var contact in _radar.RocketContacts)
			{
				Vector2 position;
				if (IsVisible(camera, contact.Rocket, out position))
				{
					DrawIcon(position, GetClassificationIcon(contact));
				}
			}

			foreach (var contact in _radar.Blips)
			{
				Vector2 position;
				if (IsVisible(camera, contact.Transform, out position))
				{
					DrawRadarBlip(contact, position);
				}
			}
		}

		private void DrawRadarBlip(SensorBlip contact, Vector2 position)
		{
			DrawIcon(position, RadarBlipIcon);
			var rect = new Rect(position.x + 20, position.y-16, 120, 60);
			GUI.Label(rect, string.Format("Unknown contact\r\n{0}", contact.Distance));
		}

		private Texture GetClassificationIcon(RocketContact rocket)
		{
			switch (rocket.Classification)
			{
				case RocketClassification.Enemy:
					return EnemyRocketIcon;

				case RocketClassification.Neutral:
					return NeutralRocketIcon;

				case RocketClassification.Friendly:
					return FriendlyRocketIcon;

				default:
					return null;
			}
		}

		private void DrawIcon(Vector2 position, Texture shipIcon)
		{
			if (shipIcon == null)
				return;

			var size = new Vector2(shipIcon.width, shipIcon.height);
			var center = position - size / 2;

			var rect = new Rect(center, new Vector2(shipIcon.width, shipIcon.height));
			GUI.DrawTexture(rect, shipIcon);
		}

		private static bool IsVisible(Camera camera, MonoBehaviour target, out Vector2 position)
		{
			if (target == null)
			{
				position = Vector2.zero;
				return false;
			}

			return IsVisible(camera, target.transform, out position);
		}

		private static bool IsVisible(Camera camera, Transform target, out Vector2 position)
		{
			position = Vector2.zero;
			if (target == null)
				return false;

			if (camera == null)
				return false;

			var targetPosition = target.position;
			var cameraPosition = camera.transform.position;
			var heading = (targetPosition - cameraPosition).normalized;
			var dot = Vector3.Dot(camera.transform.forward, heading);
			if (dot <= 0)
				return false;

			var center = camera.WorldToScreenPoint(targetPosition);
			var rect = camera.pixelRect;
			var screenPosition = new Vector2(center.x, rect.height - center.y);
			position = screenPosition;
			return true;
		}
	}
}