using Assets.Scripts.AI;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class RadarSystemOverlay
		: MonoBehaviour
	{
		private RadarSystem _radar;

		public Texture ShipIcon;
		public Texture EnemyRocketIcon;

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

			foreach (var rocket in _radar.RocketContacts)
			{
				Vector2 position;
				if (IsVisible(camera, rocket, out position))
				{
					DrawIcon(position, EnemyRocketIcon);
				}
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
			position = Vector2.zero;

			if (target == null)
				return false;

			if (camera == null)
				return false;

			var targetPosition = target.transform.position;
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