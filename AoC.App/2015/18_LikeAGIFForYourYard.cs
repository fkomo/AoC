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
			// 2d array + 1 (less ifs in neighbour count), longer CreateLights
			// 

			return (answer1.ToString(), answer2.ToString());
		}

		public static char[][] CreateLights(string[] input)
		{
			//var lights = new char[input.Length * input.Length];
			//for (int i = 0, y = 0; y < input.Length; y++)
			//	for (var x = 0; x < input.Length; x++, i++)
			//		lights[i] = input[y][x];
			//return lights;

			var lights = new char[input.Length + 2][];
			lights[0] = Enumerable.Repeat('.', input.Length + 2).ToArray();
			for (var y = 0; y < input.Length; y++)
				lights[y + 1] = new char[] { '.' }.Concat(input[y]).Concat(new char[] { '.' }).ToArray();
			lights[^1] = Enumerable.Repeat('.', input.Length + 2).ToArray();

			return lights;
		}

		public static char[][] GameOfLifeStepWithFixedCorners(char[][] lights)
		{
			lights[1][1] = '#';
			lights[1][^2] = '#';
			lights[^2][1] = '#';
			lights[^2][^2] = '#';

			lights = GameOfLifeStep(lights);

			lights[1][1] = '#';
			lights[1][^2] = '#';
			lights[^2][1] = '#';
			lights[^2][^2] = '#';

			return lights;
		}

		public static char[][] GameOfLifeStep(char[][] lights)
		{
			var next = lights.Select(l => l.ToArray()).ToArray();
			var size = lights.Length - 2;

			var p = new v2i();
			for (p.Y = 1; p.Y <= size; p.Y++)
				for (p.X = 1; p.X <= size; p.X++)
				{
					var ns = _dir.Count(d => lights[p.Y + d.Y][(int)(p.X + d.X)] == '#');
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
