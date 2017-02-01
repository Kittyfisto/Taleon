using UnityEngine;

namespace Assets.Scripts
{
	public class DistantObjectComponent
		: MonoBehaviour
	{
		private Vector3 _offset;

		// Use this for initialization
		void Start ()
		{
			_offset = transform.position;
		}

		public Vector3 Offset
		{
			get { return _offset; }
		}
	}
}
