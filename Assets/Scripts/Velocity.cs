using System;
using UnityEngine;

namespace Assets.Scripts
{
	public struct Velocity : IEquatable<Velocity>
	{
		public readonly float Units;

		public static readonly Velocity Zero;
		public static readonly Velocity OneMeterPerSecond;
		public static readonly Velocity OneKilometerPerSecond;

		static Velocity()
		{
			Zero = new Velocity(0);
			OneMeterPerSecond = FromMetersPerSecond(1);
			OneKilometerPerSecond = OneMeterPerSecond * 1000;
		}

		public static Velocity FromMetersPerSecond(float value)
		{
			return new Velocity(value);
		}

		public static Velocity FromKilometersPerSecond(float value)
		{
			return OneKilometerPerSecond * 1000 * value;
		}

		private Velocity(float units)
		{
			Units = units;
		}

		public float MetersPerSecond
		{
			get { return this / OneMeterPerSecond; }
		}

		public float KilometersPerSecond
		{
			get { return this / OneKilometerPerSecond; }
		}

		public bool Equals(Velocity other)
		{
			return Units.Equals(other.Units);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Velocity && Equals((Velocity) obj);
		}

		public override int GetHashCode()
		{
			return Units.GetHashCode();
		}

		public override string ToString()
		{
			if (this >= OneKilometerPerSecond)
				return string.Format("{0:F1} km/s", KilometersPerSecond);

			return string.Format("{0:F0} m/s", MetersPerSecond);
		}

		public static bool operator ==(Velocity left, Velocity right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Velocity left, Velocity right)
		{
			return !left.Equals(right);
		}

		public static Velocity operator *(Velocity left, float right)
		{
			return new Velocity(left.Units * right);
		}

		public static Velocity operator *(float left, Velocity right)
		{
			return new Velocity(left * right.Units);
		}

		public static Velocity operator /(Velocity left, float right)
		{
			return new Velocity(left.Units / right);
		}

		public static float operator /(Velocity left, Velocity right)
		{
			return left.Units / right.Units;
		}

		public static bool operator <(Velocity left, Velocity right)
		{
			return left.Units < right.Units;
		}

		public static bool operator <=(Velocity left, Velocity right)
		{
			return left.Units <= right.Units;
		}

		public static bool operator >(Velocity left, Velocity right)
		{
			return left.Units > right.Units;
		}

		public static bool operator >=(Velocity left, Velocity right)
		{
			return left.Units >= right.Units;
		}

		/// <summary>
		///     Calculates the relative velocity between two objects that move at the given absolute velocities in world space.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Velocity Difference(Vector3 left, Vector3 right)
		{
			var delta = Vector3.Distance(left, right);
			return FromUnits(delta);
		}

		public static Velocity FromUnits(float delta)
		{
			return new Velocity(delta);
		}
	}
}