using System.Numerics;

namespace Ujeby.Common.Drawing.Entities
{
    public static class Colors
    {
		public static Vector4[] All => new[]
		{
			White,
			Red,
			Yellow,
			Green,
			Cyan,
			Blue,
			Purple
		};

        public static Vector4 White = new(1);
		public static Vector4 Black = new(0, 0, 0, .7f);

		public static Vector4 Red = new(1, 0, 0, .7f);
		public static Vector4 Green = new(0, 1, 0, .7f);
		public static Vector4 Blue = new(0, 0, 1, .7f);

		public static Vector4 Yellow = new(1, 1, 0, .7f);
		public static Vector4 Cyan = new(0, 1, 1, .7f);
		public static Vector4 Purple = new(1, 0, 1, .7f);
	}
}
