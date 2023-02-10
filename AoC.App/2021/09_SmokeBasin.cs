using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2021_09
{
	// TODO remove!
	public static class Directions
	{
		/// <summary>
		/// 4 x,y directions (90deg)
		/// </summary>
		public readonly static Dictionary<char, int[]> NSWE = new()
		{
			{ 'W', new[] { -1, 0 } },
			{ 'E', new[] { 1, 0 } },
			{ 'S', new[] { 0, -1 } },
			{ 'N', new[] { 0, 1 } },

			//{ 'E', new[] { 1, 0 } },
			//{ 'S', new[] { 0, -1 } },
			//{ 'N', new[] { 0, 1 } },
			//{ 'W', new[] { -1, 0 } },
		};

		/// <summary>
		/// 8 x,y directions (45deg)
		/// </summary>
		public readonly static Dictionary<string, int[]> All = new()
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

		/// <summary>
		/// 
		/// </summary>
		public readonly static Dictionary<char, char> OppositeNSWE = new()
		{
			{ 'W', 'E'},
			{ 'E', 'W' },
			{ 'S', 'N' },
			{ 'N', 'S' },
		};
	}

	internal class SmokeBasin : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var w = input.First().Length;
			var h = input.Length;

			// part1
			long answer1 = 0;
			for (var y = 0; y < h; y++)
				for (var x = 0; x < w; x++)
				{
					var p = input[y][x];
					if ((p == '9') ||
						(x > 0 && input[y][x - 1] < p) ||
						(y > 0 && input[y - 1][x] < p) ||
						(x < w - 1 && input[y][x + 1] < p) ||
						(y < h - 1 && input[y + 1][x] < p))
						continue;

					// basin
					answer1 += (p - '0') + 1;
				}

			// part2
			var basinSizes = new List<long>();
			for (var y = 0; y < h; y++)
				for (var x = 0; x < w; x++)
				{
					var p = input[y][x];
					if ((p == '9') ||
						(x > 0 && input[y][x - 1] < p) ||
						(y > 0 && input[y - 1][x] < p) ||
						(x < w - 1 && input[y][x + 1] < p) ||
						(y < h - 1 && input[y + 1][x] < p))
						continue;

					// basin
					basinSizes.Add(BasinSize(input, x, y, 0, 0));
				}
			long answer2 = 1;
			foreach (var bs in basinSizes.OrderByDescending(s => s).Take(3))
				answer2 *= bs;


			return (answer1.ToString(), answer2.ToString());
		}

		private static int BasinSize(string[] map, int x, int y, int x0, int y0)
		{
			var xy = map[y][x];

			// mark place x,y as visited
			map[y] = $"{map[y].Substring(0, x)}X{map[y].Substring(x + 1)}";

			var size = 1;

			foreach (var dir in Directions.NSWE.Values)
			{
				if (dir[0] == x0 && dir[1] == y0)
					continue;

				var x1 = x + dir[0];
				var y1 = y + dir[1];

				if (y1 < 0 || x1 < 0 || x1 >= map.First().Length || y1 >= map.Length ||
					map[y1][x1] <= xy || map[y1][x1] == '9' || map[y1][x1] == 'X')
					continue;

				size += BasinSize(map, x1, y1, dir[0] * -1, dir[1] * -1);
			}

			return size;
		}
	}
}
