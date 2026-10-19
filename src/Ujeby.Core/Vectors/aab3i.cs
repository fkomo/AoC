namespace Ujeby.Vectors
{
	/// <summary>
	/// axis aligned 3d box
	/// </summary>
#pragma warning disable IDE1006 // Naming Styles
	public struct aab3i
#pragma warning restore IDE1006 // Naming Styles
	{
		public v3i Min;
		public v3i Max;
		public readonly v3i Size => Max - Min + new v3i(1);

		public static aab3i Empty => new(new(long.MaxValue), new(long.MinValue));

		public aab3i(v3i[] corners) : this(v3i.Min(corners[0], corners[1]), v3i.Max(corners[0], corners[1]))
		{
		}

		public aab3i(v3i min, v3i max)
		{
			Min = min;
			Max = max;
		}

		public override readonly string ToString() => $"{Min}..{Max}";
		public override readonly bool Equals(object obj) => obj is aab3i aab && this == aab;
		public override int GetHashCode() => Min.GetHashCode() ^ Max.GetHashCode();

		public static bool operator ==(aab3i a, aab3i b) => a.Min == b.Min && a.Max == b.Max;
		public static bool operator !=(aab3i a, aab3i b) => !(a == b);

		public readonly bool Intersect(aab3i other) 
			=> !(other.Min.X > Max.X || other.Min.Y > Max.Y || other.Min.Z > Max.Z || other.Max.X < Min.X || other.Max.Y < Min.Y || other.Max.Z < Min.Z);

		public readonly bool Overlaps(aab3i other, out aab3i result)
		{
			result = default;

			if (!Intersect(other))
				return false;

			var x = new long[] { Min.X, Max.X, other.Min.X, other.Max.X }.OrderBy(i => i).ToArray();
			var y = new long[] { Min.Y, Max.Y, other.Min.Y, other.Max.Y }.OrderBy(i => i).ToArray();
			var z = new long[] { Min.Z, Max.Z, other.Min.Z, other.Max.Z }.OrderBy(i => i).ToArray();

			result = new aab3i(new(x[1], y[1], z[1]), new(x[2], y[2], z[2]));

			return true;
		}

		public readonly bool IntersectsWithOctahedron(v4i oct) => IntersectsWithOctahedron(oct.ToV3i(), oct.W);

		public readonly bool IntersectsWithOctahedron(v3i octPos, long octRadius)
		{
			var clamp = new v3i(
				(octPos.X < Min.X) ? Min.X : (octPos.X > Max.X) ? Max.X : octPos.X,
				(octPos.Y < Min.Y) ? Min.Y : (octPos.Y > Max.Y) ? Max.Y : octPos.Y,
				(octPos.Z < Min.Z) ? Min.Z : (octPos.Z > Max.Z) ? Max.Z : octPos.Z);

			return (clamp - octPos).ManhLength() <= octRadius;
		}

		/// <summary>
		/// returns 8 octants of the axis-aligned 3d box
		/// </summary>
		/// <param name="aabb"></param>
		/// <returns></returns>
		public readonly aab3i[] OctDiv()
		{
			if (Size == 1)
				return [];

			var result = new List<aab3i>();

			var half = v3i.Max(Size / 2, new v3i(1));

			var half1 = Min + half - 1;
			var half2 = Max - (Size - half - 1);

			result.Add(new aab3i(new v3i(Min.X, Min.Y, Min.Z), new v3i(half1.X, half1.Y, half1.Z)));

			if (Size.Y > 1)
				result.Add(new aab3i(new v3i(Min.X, half2.Y, Min.Z), new v3i(half1.X, Max.Y, half1.Z)));

			if (Size.X > 1)
			{
				result.Add(new aab3i(new v3i(half2.X, Min.Y, Min.Z), new v3i(Max.X, half1.Y, half1.Z)));

				if (Size.Y > 1)
					result.Add(new aab3i(new v3i(half2.X, half2.Y, Min.Z), new v3i(Max.X, Max.Y, half1.Z)));
			}

			if (Size.Z > 1)
			{
				result.Add(new aab3i(new v3i(Min.X, Min.Y, half2.Z), new v3i(half1.X, half1.Y, Max.Z)));

				if (Size.Y > 1)
					result.Add(new aab3i(new v3i(Min.X, half2.Y, half2.Z), new v3i(half1.X, Max.Y, Max.Z)));

				if (Size.X > 1)
				{
					result.Add(new aab3i(new v3i(half2.X, Min.Y, half2.Z), new v3i(Max.X, half1.Y, Max.Z)));

					if (Size.Y > 1)
						result.Add(new aab3i(new v3i(half2.X, half2.Y, half2.Z), new v3i(Max.X, Max.Y, Max.Z)));
				}
			}

			return [.. result];
		}
	}
}
