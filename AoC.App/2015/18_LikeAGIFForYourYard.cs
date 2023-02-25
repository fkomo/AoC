using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2015_18
{
	[AoCPuzzle(Year = 2015, Day = 18, Answer1 = "814", Answer2 = "924")]
	public class LikeAGIFForYourYard : PuzzleBase
	{
		private static readonly v2i[] _dir = v2i.RightDownLeftUp.Concat(v2i.Corners).ToArray();

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
			var answer1 = lights.Sum(i => i.Count(l => l == '#'));

			// part2
			lights = CreateLights(input);
			for (var i = 1; i <= steps; i++)
				lights = GameOfLifeStepWithFixedCorners(lights);
			var answer2 = lights.Sum(i => i.Count(l => l == '#'));

			// 1d array 390-420ms
			// 2d array 

			return (answer1.ToString(), answer2.ToString());
		}

		public static char[][] CreateLights(string[] input)
		{
			//var lights = new char[input.Length * input.Length];
			//for (int i = 0, y = 0; y < input.Length; y++)
			//	for (var x = 0; x < input.Length; x++, i++)
			//		lights[i] = input[y][x];
			//return lights;

			return input.Select(i => i.ToArray()).ToArray();
		}

		public static char[][] GameOfLifeStepWithFixedCorners(char[][] lights)
		{
			lights[0][0] = '#';
			lights[0][^1] = '#';
			lights[^1][0] = '#';
			lights[^1][^1] = '#';

			lights = GameOfLifeStep(lights);

			lights[0][0] = '#';
			lights[0][^1] = '#';
			lights[^1][0] = '#';
			lights[^1][^1] = '#';

			return lights;
		}

		public static char[][] GameOfLifeStep(char[][] lights)
		{
			var next = lights.Select(l => l.ToArray()).ToArray();
			var size = lights.Length;

			var p = new v2i();
			for (p.Y = 0; p.Y < size; p.Y++)
				for (p.X = 0; p.X < size; p.X++)
				{
					var ns = 0;
					foreach (var dir in _dir)
					{
						var p1 = p + dir;
						if (p1.X < 0 || p1.Y < 0 || p1.X == size || p1.Y == size)
							continue;

						if (lights[p1.Y][p1.X] == '#')
							ns++;
					}

					if ((lights[p.Y][p.X] == '#' && (ns == 2 || ns == 3)) || 
						(lights[p.Y][p.X] != '#' && ns == 3))
						next[p.Y][p.X] = '#';
					else
						next[p.Y][p.X] = '.';
				}

			return next;
		}
	}
}
