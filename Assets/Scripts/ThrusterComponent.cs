using UnityEngine;

namespace Assets.Scripts
{
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
			_effect.Stop();
		}

		// Update is called once per frame
		private void Update()
		{
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