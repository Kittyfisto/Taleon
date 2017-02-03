using Assets.Scripts.AI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class DisplayPlayerStats
		: MonoBehaviour
	{
		private GameObject _playership;
		private Text _currentVelocity;
		private PlayerUnitController _playerUnitController;
		private NavigationSystemComponent _navigation;

		// Use this for initialization
		private void Start()
		{
			_playerUnitController = FindObjectOfType<PlayerUnitController>();
			_navigation = _playerUnitController.GetComponent<NavigationSystemComponent>();
			_playership = _playerUnitController.gameObject;

			_currentVelocity = transform.Find("CurrentVelocityValue").GetComponent<Text>();
		}

		// Update is called once per frame
		private void Update()
		{
			_currentVelocity.text = string.Format("{0:F1} ({1})",
				_navigation.CurrentVelocity.magnitude,
				_playerUnitController.TargetVelocity);
		}
	}
}