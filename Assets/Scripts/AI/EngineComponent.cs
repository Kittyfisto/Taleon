using UnityEngine;

namespace Assets.Scripts.AI
{
	/// <summary>
	///     Responsible for controlling both visual and auditory effect that represent this engine to the user.
	///     Enables / disables them accordingly.
	/// </summary>
	public class EngineComponent : MonoBehaviour
	{
		private bool _isFiring;
		private ParticleSystem _effect;

		// Use this for initialization
		private void Start()
		{
			_effect = GetComponentInChildren<ParticleSystem>();
		}

		public void Fire(bool fire)
		{
			// This looks like shit
			// TODO: Animate emission rate when starting up / slowing down
			if (fire)
			{
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
}