using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day09
{
	internal class SmokeBasin : ProblemBase
	{
		protected override (long, long) SolveProblem(string[] input)
		{
			var w = input.First().Length;
			var h = input.Length;

			DebugLine($"map size {w}x{h}");

			// part1
			long result1 = 0;
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
					result1 += (p - '0') + 1;
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
			long result2 = 1;
			foreach (var bs in basinSizes.OrderByDescending(s => s).Take(3))
				result2 *= bs;


			return (result1, result2);
		}

		/// <summary>
		/// x, y
		/// </summary>
		private static (int, int)[] _dir = new[] { (-1, 0), (0, 1), (1, 0), (0, -1) };

		private static int BasinSize(string[] map, int x, int y, int x0, int y0)
		{
			// TODO mark place x,y as visited

			var size = 1;

			foreach (var dir in _dir)
			{
				if (dir.Item1 == x0 && dir.Item2 == y0)
					continue;

				var x1 = x + dir.Item1;
				var y1 = y + dir.Item2;

				if (y1 < 0 || x1 < 0 || x1 >= map.First().Length || y1 >= map.Length ||
					map[y1][x1] <= map[y][x] || map[y1][x1] == '9')
					continue;

				size += BasinSize(map, x1, y1, dir.Item1 * -1, dir.Item2 * -1);
			}

			return size;
		}
	}
}
