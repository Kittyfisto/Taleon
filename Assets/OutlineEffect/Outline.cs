using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Outline : MonoBehaviour
{
	public int color;
	public bool eraseRenderer;

	[HideInInspector] public int originalLayer;

	[HideInInspector] public Material[] originalMaterials;

	public OutlineEffect outlineEffect;

	private void Start()
	{
	}

	public void Enable()
	{
		if (outlineEffect == null)
			outlineEffect = Camera.main.GetComponent<OutlineEffect>();
		outlineEffect.AddOutline(this);
	}

	public void Disable()
	{
		outlineEffect.RemoveOutline(this);
	}

	void OnEnable()
	{
		Enable();
	}

	void OnDisable()
	{
		Disable();
	}
}