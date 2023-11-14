using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2015_18
{
	[AoCPuzzle(Year = 2015, Day = 18, Answer1 = "814", Answer2 = "924")]
	public class LikeAGIFForYourYard : PuzzleBase
	{
		private static readonly v2i[] _neighbors = v2i.RightDownLeftUp.Concat(v2i.Corners).ToArray();

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var steps = 100;
			// sample
			//steps = 4;

			// part1
			var lights = CreateLights(input);
			for (var i = 1; i <= steps; i++)
				lights = GameOfLifeStep(lights);
			var answer1 = lights.Sum(i => i.Count(c => c));

			// part2
			lights = CreateLights(input);
			for (var i = 1; i <= steps; i++)
				lights = GameOfLifeStepWithFixedCorners(lights);
			var answer2 = lights.Sum(i => i.Count(c => c));

			// 340ms

			return (answer1.ToString(), answer2.ToString());
		}

		public static bool[][] CreateLights(string[] input)
		{
			return input.Select(i => i.Select(c => c == '#').ToArray()).ToArray();
		}

		public static bool[][] GameOfLifeStepWithFixedCorners(bool[][] current)
		{
			current[0][0] = current[0][^1] = current[^1][0] = current[^1][^1] = true;
			current = GameOfLifeStep(current);
			current[0][0] = current[0][^1] = current[^1][0] = current[^1][^1] = true;

			return current;
		}

		public static bool[][] GameOfLifeStep(bool[][] current)
		{
			var size = current.Length;
			var next = current.Select(x => x.ToArray()).ToArray();

			_ = Parallel.For(0, size, (y) =>
			{
				for (var x = 0; x < size; x++)
				{
					// count neighbors
					var neighbors = 0;
					foreach (var n in _neighbors)
					{
						var pn = new v2i(x, y) + n;
						if (pn.X == -1 || pn.Y == -1 || pn.X == size || pn.Y == size)
							continue;

						if (current[pn.Y][pn.X])
							neighbors++;
					}

					var on = current[y][x];
					next[y][x] = (on && (neighbors == 2 || neighbors == 3)) || (!on && neighbors == 3);
				}
			});

			return next;
		}

		public static void Draw(bool[][] grid)
		{
			for (var y = 0; y < grid.Length; y++)
				Debug.Line($"{ new string(grid[y].Select(c => c ? '#' : '.').ToArray()) }");
			Debug.Line();
		}
	}
}
