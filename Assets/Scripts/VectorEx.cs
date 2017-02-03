using UnityEngine;

namespace Assets.Scripts
{
	public static class VectorEx
	{
		public static bool TryGetNormalized(this Vector3 that, float minimumMagnitude, out Vector3 normalized)
		{
			var magnitude = that.magnitude;
			if (magnitude < minimumMagnitude)
			{
				normalized = Vector3.zero;
				return false;
			}

			normalized = that / magnitude;
			return true;
		}

		public static Vector3 TryGetNormalized(this Vector3 that, float minimumMagnitude, Vector3 @default)
		{
			Vector3 normalized;
			if (!TryGetNormalized(that, minimumMagnitude, out normalized))
				return @default;

			return normalized;
		}
	}
}