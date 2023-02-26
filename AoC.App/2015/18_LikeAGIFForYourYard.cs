using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2015_18
{
	[AoCPuzzle(Year = 2015, Day = 18, Answer1 = "814", Answer2 = "924")]
	public class LikeAGIFForYourYard : PuzzleBase
	{
		private static readonly v2i[] _dir = v2i.RightDownLeftUp.Concat(v2i.Corners).ToArray();
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
				lights = GameOfLifeStep(lights.Grid, lights.On);
			var answer1 = lights.On.Length;

			// part2
			lights = CreateLights(input);
			for (var i = 1; i <= steps; i++)
				lights = GameOfLifeStepWithFixedCorners(lights.Grid, lights.On);
			var answer2 = lights.On.Length;

			// 1d array 390-420ms
			// 2d array 
			// 2d array + 1 (less ifs in neighbour count), longer CreateLights
			// 

			return (answer1.ToString(), answer2.ToString());
		}

		public static (char[][] Grid, v2i[] On) CreateLights(string[] input)
		{
			var lights = new char[input.Length + 2][];
			lights[0] = Enumerable.Repeat('.', input.Length + 2).ToArray();
			for (var y = 0; y < input.Length; y++)
				lights[y + 1] = new char[] { '.' }.Concat(input[y]).Concat(new char[] { '.' }).ToArray();
			lights[^1] = Enumerable.Repeat('.', input.Length + 2).ToArray();

			var on = new List<v2i>();
			for (int y = 0; y < lights.Length; y++)
				for (var x = 0; x < input.Length; x++)
					if (lights[y][x] == '#')
						on.Add(new v2i(y, x));

			return (lights, on.ToArray());
		}

		public static (char[][] Grid, v2i[] On) GameOfLifeStepWithFixedCorners(char[][] grid, v2i[] on)
		{
			grid[1][1] = '#';
			grid[1][^2] = '#';
			grid[^2][1] = '#';
			grid[^2][^2] = '#';

			var lights = GameOfLifeStep(grid, on);

			lights.Grid[1][1] = '#';
			lights.Grid[1][^2] = '#';
			lights.Grid[^2][1] = '#';
			lights.Grid[^2][^2] = '#';

			return lights;
		}

		public static (char[][] Grid, v2i[] On) GameOfLifeStep(char[][] grid, v2i[] on)
		{
			var size = grid.Length - 2;

			var nextOn = new List<v2i>();
			var nextGrid = grid.Select(l => l.ToArray()).ToArray();

			foreach (var light in on)
			{
				foreach (var pn in _dir2)
				{
					var p = pn + light;
					var ns = _dir.Count(d => grid[p.Y + d.Y][(int)(p.X + d.X)] == '#');
					if ((grid[p.Y][p.X] == '#' && (ns == 2 || ns == 3)) ||
						(grid[p.Y][p.X] != '#' && ns == 3))
					{
						nextGrid[p.Y][p.X] = '#';
						nextOn.Add(p);
					}
					else
						nextGrid[p.Y][p.X] = '.';
				}
			}

			return (nextGrid, nextOn.Distinct().ToArray());
		}
	}
}
