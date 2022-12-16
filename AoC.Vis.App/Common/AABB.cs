using System.Numerics;

namespace Ujeby.AoC.Vis.App.Common
{
	public struct AABB
	{
		/// <summary>bottom left - world coordinates</summary>
		public Vector2 Min { get; private set; }

		/// <summary>top right - world coordinates</summary>
		public Vector2 Max { get; private set; }

		public Vector2 HalfSize { get; private set; }
		public Vector2 Center { get; private set; }
		public Vector2 Size => Max - Min;

		public double Top => Max.Y;
		public double Bottom => Min.Y;
		public double Left => Min.X;
		public double Right => Max.X;

		public override string ToString() => $"{ Min }-{ Max }";
		public static AABB operator +(AABB bb, Vector2 v) => new(bb.Min + v, bb.Max + v);

		public AABB(Vector2 min, Vector2 max)
		{
			Min = min;
			Max = max;
			Center = (min + max) / 2;
			HalfSize = (max - min) / 2;
		}

		public AABB GetAABB() => this;
	}
}
