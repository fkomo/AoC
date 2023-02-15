using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2021_09
{
	[AoCPuzzle(Year = 2021, Day = 09, Answer1 = "444", Answer2 = "1168440")]
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
					var pInput = input[y][x];
					if ((pInput == '9') ||
						(x > 0 && input[y][x - 1] < pInput) ||
						(y > 0 && input[y - 1][x] < pInput) ||
						(x < w - 1 && input[y][x + 1] < pInput) ||
						(y < h - 1 && input[y + 1][x] < pInput))
						continue;

					// basin
					answer1 += (pInput - '0') + 1;
				}

			// part2
			var basinSizes = new List<long>();
			var p = new v2i();
			for (p.Y = 0; p.Y < h; p.Y++)
				for (p.X = 0; p.X < w; p.X++)
				{
					var pInput = input[p.Y][(int)p.X];
					if ((pInput == '9') ||
						((int)p.X > 0 && input[p.Y][(int)p.X - 1] < pInput) ||
						(p.Y > 0 && input[p.Y - 1][(int)p.X] < pInput) ||
						((int)p.X < w - 1 && input[p.Y][(int)p.X + 1] < pInput) ||
						(p.Y < h - 1 && input[p.Y + 1][(int)p.X] < pInput))
						continue;

					// basin
					basinSizes.Add(BasinSize(input, p, new()));
				}
			long answer2 = 1;
			foreach (var bs in basinSizes.OrderByDescending(s => s).Take(3))
				answer2 *= bs;

			return (answer1.ToString(), answer2.ToString());
		}

		private static int BasinSize(string[] map, v2i p, v2i p0)
		{
			var pMap = map[p.Y][(int)p.X];

			// mark place x,y as visited
			map[p.Y] = $"{map[p.Y][..(int)p.X]}X{map[p.Y][((int)p.X + 1)..]}";

			var size = 1;
			foreach (var dir in v2i.RightDownLeftUp)
			{
				if (dir == p0)
					continue;

				var p1 = p + dir;
				if (p1.Y < 0 || p1.X < 0 || p1.X >= map.First().Length || p1.Y >= map.Length ||
					map[p1.Y][(int)p1.X] <= pMap || map[p1.Y][(int)p1.X] == '9' || map[p1.Y][(int)p1.X] == 'X')
					continue;

				size += BasinSize(map, p1, dir * -1);
			}

			return size;
		}
	}
}
