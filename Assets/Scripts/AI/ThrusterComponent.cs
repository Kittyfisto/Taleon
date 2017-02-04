using UnityEngine;

namespace Assets.Scripts.AI
{
	/// <summary>
	///     Responsible for controlling both visual and auditory effect that represent this thruster to the user.
	///     Enables / disables them accordingly.
	/// </summary>
	public class ThrusterComponent : MonoBehaviour
	{
		private ParticleSystem _effect;
		private bool _isFiring;
		private float _maximumRate;
		private ParticleSystem.EmissionModule _emission;

		// Use this for initialization
		private void Start()
		{
			_effect = GetComponentInChildren<ParticleSystem>();
			_maximumRate = _effect.emission.rateOverTimeMultiplier;
			_emission = _effect.emission;
		}

		public void Fire(float strength)
		{
			var actualStrength = Mathf.Clamp01(strength);
			if (actualStrength > 0)
			{
				if (!_isFiring)
				{
					_effect.Play(false);
					_isFiring = true;
				}

				float rate = Mathf.Lerp(0, _maximumRate, strength);
				_emission.rateOverTimeMultiplier = rate;
			}
			else
			{
				if (_isFiring)
				{
					_effect.Stop(false, ParticleSystemStopBehavior.StopEmitting);
					_isFiring = false;
				}
			}
		}
	}
}