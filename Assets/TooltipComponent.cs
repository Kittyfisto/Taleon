using UnityEngine;

public class TooltipComponent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void OnMouseEnter()
	{
		Tooltip.Show(gameObject);
	}

	void OnMouseExit()
	{
		Tooltip.Hide();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
