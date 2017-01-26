using UnityEngine;

namespace Assets.Scripts
{
	public class BillboardComponent : MonoBehaviour {

		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update ()
		{
			var mainCamera = Camera.main;
			transform.LookAt(mainCamera.transform.position,
				mainCamera.transform.up);
		}
	}
}
