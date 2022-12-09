namespace Ujeby.AoC.App.Year2022.Day09
{
	internal static class Directions
	{
		/// <summary>
		/// x,y
		/// </summary>
		public static Dictionary<char, int[]> NSWE = new()
		{
			{ 'N', new[] { 0, 1 } },
			{ 'S', new[] { 0, -1 } },
			{ 'W', new[] { -1, 0 } },
			{ 'E', new[] { 1, 0 } },
		};

		/// <summary>
		/// x,y
		/// </summary>
		public static Dictionary<string, int[]> All = new()
		{
			{ "N", new[] { 0, 1 } },
			{ "S", new[] { 0, -1 } },
			{ "W", new[] { -1, 0 } },
			{ "E", new[] { 1, 0 } },
			{ "NW", new[] { -1, 1 } },
			{ "NE", new[] { 1, 1 } },
			{ "SW", new[] { -1, -1 } },
			{ "SE", new[] { 1, -1 } },
		};
	}
}