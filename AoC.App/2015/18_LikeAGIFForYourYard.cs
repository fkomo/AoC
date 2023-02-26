using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2015_18
{
	[AoCPuzzle(Year = 2015, Day = 18, Answer1 = "814", Answer2 = "924")]
	public class LikeAGIFForYourYard : PuzzleBase
	{
		private static readonly v2i[] _neighbors = v2i.RightDownLeftUp.Concat(v2i.Corners).ToArray();
		private static readonly v2i[] _dir2 = 
			v2i.RightDownLeftUp.Concat(v2i.Corners).Concat(new v2i[] { new v2i() }).ToArray();

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var steps = 100;
#if _DEBUG_SAMPLE
			steps = 4;
#endif
			// part1
			var lights = CreateLights(input);
			for (var i = 1; i <= steps; i++)
				lights = GameOfLifeStep(lights);
			var answer1 = lights.Sum(i => i.Count(c => c == '#'));

			// part2
			lights = CreateLights(input);
			for (var i = 1; i <= steps; i++)
				lights = GameOfLifeStepWithFixedCorners(lights);
			var answer2 = lights.Sum(i => i.Count(c => c == '#'));

			return (answer1.ToString(), answer2.ToString());
		}

		public static char[][] CreateLights(string[] input)
		{
			return input.Select(i => i.ToArray()).ToArray();
		}

		public static char[][] GameOfLifeStepWithFixedCorners(char[][] current)
		{
			current[0][0] = '#';
			current[0][^1] = '#';
			current[^1][0] = '#';
			current[^1][^1] = '#';

			var next = GameOfLifeStep(current);

			next[0][0] = '#';
			next[0][^1] = '#';
			next[^1][0] = '#';
			next[^1][^1] = '#';

			return next;
		}

		public static char[][] GameOfLifeStep(char[][] current)
		{
			var size = current.Length;
			var next = current.Select(x => x.ToArray()).ToArray();

			var p = new v2i();
			for (p.Y = 0; p.Y < size; p.Y++)
				for (p.X = 0; p.X < size; p.X++)
				{
					// count neighbors
					var neighbors = 0;
					foreach (var dir in _neighbors)
					{
						var p1 = p + dir;
						if (p1.X < 0 || p1.Y < 0 || p1.X == size || p1.Y == size)
							continue;
						if (current[p1.Y][p1.X] == '#')
							neighbors++;
					}

					var i = p.Y * size + p.X;
					if ((current[p.Y][p.X] == '#' && (neighbors == 2 || neighbors == 3)) ||
						(current[p.Y][p.X] != '#' && neighbors == 3))
					{
						next[p.Y][p.X] = '#';
					}
					else
						next[p.Y][p.X] = '.';
				}

			return next;
		}

		public static void Draw(char[][] grid)
		{
			for (var y = 0; y < grid.Length; y++)
				Debug.Line($"{ new string(grid[y]) }");
			Debug.Line();
		}
	}
}
