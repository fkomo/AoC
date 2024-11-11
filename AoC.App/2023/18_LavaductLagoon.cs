using Ujeby.AoC.Common;
using Ujeby.Grid.CharMapExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_18;

[AoCPuzzle(Year = 2023, Day = 18, Answer1 = "28911", Answer2 = "77366737561114", Skip = false)]
public class LavaductLagoon : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var digPlan = CreateDigPlan(input);
		var grid = CreateGrid(digPlan, out v2i gridSize);
		grid = FloodFillGridFromOutside(grid, gridSize);
		var answer1 = grid.Sum(y => y.Count(x => x != 'O'));

		// part2
		digPlan = CreateDigPlanPart2(input);
		var trench = CreateTrench(digPlan);

		long answer2 = 0;
		// shoelace formula
		for (var i = 0; i < trench.Length - 1; i++)
			answer2 += trench[i].X * trench[i + 1].Y - trench[i + 1].X * trench[i].Y;
		answer2 /= 2;

		// pick's theorem
		answer2 += digPlan.Sum(x => x.Dist) / 2 + 1;

		return (answer1.ToString(), answer2.ToString());
	}

	public static (int DirId, int Dist)[] CreateDigPlan(string[] input)
	{
		// U,D,L,R to v2i.UpDownLeftRight index
		var _dir = new Dictionary<char, int>
		{
			{ 'U', 1 },
			{ 'D', 0 },
			{ 'L', 2 },
			{ 'R', 3 },
		};

		return input.Select(x =>
		{
			var split = x.Split(' ');
			return (DirId: _dir[split[0][0]], Dist: int.Parse(split[1]));
		}).ToArray();
	}

	public static char[][] FloodFillGridFromOutside(char[][] grid, v2i gridSize)
	{
		// first/last row
		for (var x = 0; x < gridSize.X; x++)
		{
			grid.FloodFillNonRec(new v2i(x, 0), 'O', v2i.PlusMinusOne);
			grid.FloodFillNonRec(new v2i(x, gridSize.Y - 1), 'O', v2i.PlusMinusOne);
		}

		// first/last column
		for (var y = 0; y < gridSize.Y; y++)
		{
			grid.FloodFillNonRec(new v2i(0, y), 'O', v2i.PlusMinusOne);
			grid.FloodFillNonRec(new v2i(gridSize.X - 1, y), 'O', v2i.PlusMinusOne);
		}
		DebugPrintLagoon(grid);

		return grid;
	}

	public static char[][] CreateGrid((int DirId, int Dist)[] digPlan, out v2i gridSize)
	{
		var min = new v2i(long.MaxValue);
		var max = new v2i(long.MinValue);
		var trench = new List<v2i>();
		var p0 = v2i.Zero;
		foreach (var (DirId, Dist) in digPlan)
		{
			for (var i = 0; i < Dist; i++)
			{
				p0 += v2i.UpDownLeftRight[DirId];
				trench.Add(p0);

				min = v2i.Min(min, p0);
				max = v2i.Max(max, p0);
			}
		}
		Debug.Line($"trenchLength: {trench.Count}");

		// create grid
		gridSize = max - min + new v2i(1);
		Debug.Line($"grid {min}..{max}");
		Debug.Line($"gridSize: {gridSize}");

		var grid = new char[gridSize.Y][];
		for (var y = 0; y < gridSize.Y; y++)
		{
			grid[y] = new char[gridSize.X];
			for (var x = 0; x < gridSize.X; x++)
				grid[y][x] = '.';
		}

		// add trench to grid
		foreach (var p in trench)
		{
			var pg = p - min;
			grid[pg.Y][pg.X] = '#';
		}
		DebugPrintLagoon(grid);

		return grid;
	}

	public static v2i[] CreateTrench((int DirId, int Dist)[] digPlan)
	{
		var min = new v2i(long.MaxValue);
		var max = new v2i(long.MinValue);
		var p0 = v2i.Zero;

		var trench = new List<v2i>
		{
			p0
		};

		foreach (var (DirId, Dist) in digPlan)
		{
			p0 += v2i.UpDownLeftRight[DirId] * Dist;
			trench.Add(p0);

			min = v2i.Min(min, p0);
			max = v2i.Max(max, p0);
		}

		return trench.ToArray();
	}

	static void DebugPrintLagoon(char[][] grid)
	{
#if DEBUG
		for (var y = 0; y < grid.Length; y++)
			Debug.Line(new string(grid[y]));
		Debug.Line();
#endif
	}

	public static (int DirId, int Dist)[] CreateDigPlanPart2(string[] input)
	{
		// last color digit to v2i.UpDownLeftRight index
		var _dir = new Dictionary<char, int>
		{
			{ '0', 3 },
			{ '1', 0 },
			{ '2', 2 },
			{ '3', 1 },
		};

		return input.Select(x =>
			{
				var split = x.Split(' ');
				return (DirId: _dir[split[2][^2]], Dist: int.Parse(split[2][2..^2], System.Globalization.NumberStyles.HexNumber));
			}).ToArray();
	}
}