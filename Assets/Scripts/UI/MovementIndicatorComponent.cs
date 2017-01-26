using UnityEngine;

namespace Assets.Scripts.UI
{
	/// <summary>
	///     This component is responsible for showing the target of a unit.
	/// </summary>
	public class MovementIndicatorComponent : MonoBehaviour
	{
		private Transform _targetIndicator;
		private Transform _line;
		private GameObject _source;
		private Vector3 _target;

		/// <summary>
		///     Shows an indicator that points from the given source to the given (static) target position.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void Show(GameObject source, Vector3 target)
		{
			_source = source;
			_target = target;
			gameObject.SetActive(true);
		}

		/// <summary>
		///     Hides this indicator.
		/// </summary>
		public void Hide()
		{
			gameObject.SetActive(false);
		}

		// Use this for initialization
		private void Start()
		{
			_targetIndicator = transform.Find("TargetIndicator").GetComponent<Transform>();
			_line = transform.Find("MovementLine").GetComponent<Transform>();
		}

		// Update is called once per frame
		private void Update()
		{
			if (_source != null)
			{
				//var relativePosition = transform.InverseTransformPoint(worldPosition.Value);
				_targetIndicator.transform.position = _target;
				_targetIndicator.gameObject.SetActive(true);

				var position = _source.transform.position;
				var delta = _target - position;
				var distance = delta.magnitude;

				_line.localScale = new Vector3(_line.localScale.x,
					distance / 2,
					_line.localScale.z);
				_line.up = delta;
				_line.position = position + delta / 2;

				_line.gameObject.SetActive(true);
			}
		}
	}
}