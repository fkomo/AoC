namespace Ujeby.Vectors
{
	/// <summary>
	/// axis-aligned 2d box
	/// </summary>
#pragma warning disable IDE1006 // Naming Styles
	public struct aab2i
#pragma warning restore IDE1006 // Naming Styles
	{
		public v2i Min { get; private set; }
		public v2i Max { get; private set; }
		public readonly v2i Size => Max - Min + new v2i(1);

		public readonly long Top => Max.Y;
		public readonly long Bottom => Min.Y;
		public readonly long Left => Min.X;
		public readonly long Right => Max.X;

		public override readonly string ToString() => $"{Min}-{Max}";

		/// <summary>
		/// axis-aligned 2d (v2i) box
		/// </summary>
		/// <param name="corners">both corners are inclusive</param>
		public aab2i(v2i[] corners) : this(v2i.Min(corners[0], corners[1]), v2i.Max(corners[0], corners[1]))
		{
		}

		/// <summary>
		/// axis-aligned 2d (v2i) box
		/// </summary>
		/// <param name="min">inclusive</param>
		/// <param name="max">inclusive</param>
		public aab2i(v2i min, v2i max)
		{
			Min = min;
			Max = max;
		}

		public readonly bool Contains(v2i p) => (Min <= p && p <= Max);

		public readonly bool Overlaps(aab2i other, out aab2i result)
		{
			result = default;

			if (Max.X < other.Min.X || Min.X > other.Max.X || Max.Y < other.Min.Y || Min.Y > other.Max.Y)
				return false;

			result = new aab2i(
				new v2i(System.Math.Max(Min.X, other.Min.X), System.Math.Max(Min.Y, other.Min.Y)),
				new v2i(System.Math.Min(Max.X, other.Max.X), System.Math.Min(Max.Y, other.Max.Y)));

			return true;
		}

		/// <summary>
		/// enum all points contained in box
		/// </summary>
		/// <returns></returns>
		public readonly IEnumerable<v2i> EnumPoints()
		{
			for (var y = Min.Y; y <= Max.Y; y++)
				for (var x = Min.X; x <= Max.X; x++)
					yield return new v2i(x, y);
		}

		/// <summary>
		/// enum all border-points
		/// </summary>
		/// <param name="aab"></param>
		/// <returns></returns>
		public readonly IEnumerable<v2i> EnumBorderPoints()
		{
			for (var x = Min.X; x <= Max.X; x++)
				yield return new v2i(x, Min.Y);

			if (Size.Y > 1)
			{
				for (var x = Min.X; x <= Max.X; x++)
					yield return new v2i(x, Max.Y);

				if (Size.Y > 2)
				{
					for (var y = Min.Y + 1; y <= Max.Y - 1; y++)
						yield return new v2i(Min.X, y);

					if (Size.X > 1)
					{
						for (var y = Min.Y + 1; y <= Max.Y - 1; y++)
							yield return new v2i(Max.X, y);
					}
				}
			}
		}

		/// <summary>
		/// create new box containing all points
		/// </summary>
		/// <param name="points"></param>
		/// <returns></returns>
		public static aab2i FromPoints(params v2i[] points) =>
			new(
				new v2i(points.Min(x => x.X), points.Min(x => x.Y)),
				new v2i(points.Max(x => x.X), points.Max(x => x.Y)));

		/// <summary>
		/// create new box containing all points
		/// </summary>
		/// <param name="points"></param>
		/// <returns></returns>
		public static aab2i FromPoints(IEnumerable<v2i> points) =>
			new(
				new v2i(points.Min(x => x.X), points.Min(x => x.Y)),
				new v2i(points.Max(x => x.X), points.Max(x => x.Y)));

		public aab2i Add(v2i min, v2i max)
		{
			Min += min;
			Max += max;

			return this;
		}
	}
}