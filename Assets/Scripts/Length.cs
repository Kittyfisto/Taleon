using System;

namespace Assets.Scripts
{
	public struct Length
		: IEquatable<Length>
	{
		public readonly float Units;

		public static readonly Length Zero;
		public static readonly Length OneMeter;
		public static readonly Length OneKilometer;

		private Length(float units)
		{
			Units = units;
		}

		static Length()
		{
			Zero = new Length(0);
			OneMeter = FromUnits(1);
			OneKilometer = 1000 * OneMeter;
		}

		public float Meters
		{
			get { return Units; }
		}

		public float Kilometers
		{
			get { return Meters / 1000; }
		}

		public bool Equals(Length other)
		{
			return Units.Equals(other.Units);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Length && Equals((Length) obj);
		}

		public static Length operator *(Length lhs, float rhs)
		{
			return FromUnits(lhs.Units * rhs);
		}

		public static Length operator *(float lhs, Length rhs)
		{
			return FromUnits(lhs * rhs.Units);
		}

		public override int GetHashCode()
		{
			return Units.GetHashCode();
		}

		public static bool operator <(Length left, Length right)
		{
			return left.Units < right.Units;
		}

		public static bool operator <=(Length left, Length right)
		{
			return left.Units <= right.Units;
		}

		public static bool operator >(Length left, Length right)
		{
			return left.Units > right.Units;
		}

		public static bool operator >=(Length left, Length right)
		{
			return left.Units >= right.Units;
		}

		public static bool operator ==(Length left, Length right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Length left, Length right)
		{
			return !left.Equals(right);
		}

		public static Length FromUnits(float value)
		{
			return new Length(value);
		}

		public static Length FromMeters(float meters)
		{
			return OneMeter * meters;
		}

		public static Length FromKilometers(float kilometers)
		{
			return OneKilometer * kilometers;
		}

		public override string ToString()
		{
			if (this >= OneKilometer)
				return string.Format("{0:F2} km", Kilometers);

			return string.Format("{0:F0} m", Meters);
		}
	}
}