namespace Ujeby.AoC.App.Year2021.Day05
{
	internal class Point
	{
		public int X;
		public int Y;

		public Point(int x = 0, int y = 0)
		{
			X = x;
			Y = y;
		}

		public override string ToString() => $"{X}x{Y}";
	}
}
