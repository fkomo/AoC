using System.Text;
using Ujeby.AoC.Common;
using Ujeby.Grid;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_18;

[AoCPuzzle(Year = 2023, Day = 18, Answer1 = "28911", Answer2 = null, Skip = false)]
public class LavaductLagoon : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var digPlan = CreateDigPlan(input);

		// part1
		var grid = CreateGrid(digPlan, out v2i gridSize);
		grid = FloodFillGridFromOutside(grid, gridSize);

		var answer1 = grid.Sum(y => y.Count(x => x != 'O'));

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static readonly Dictionary<char, int> _dir = new()
	{
		{ 'U', 1 },
		{ 'D', 0 },
		{ 'L', 2 },
		{ 'R', 3 },
	};

	public static (int DirId, int Dist, v3i Color)[] CreateDigPlan(string[] input)
		=> input.Select(x =>
		{
			var split = x.Split(' ');
			return (DirId: _dir[split[0][0]], Dist: int.Parse(split[1]), Color: new v3i(split[2][1..^1]));
		}).ToArray();

	public static char[][] FloodFillGridFromOutside(char[][] grid, v2i gridSize)
	{
		// first/last row
		for (var x = 0; x < gridSize.X; x++)
		{
			CharMap.FloodFillNonRec(grid, new v2i(x, 0), 'O');
			CharMap.FloodFillNonRec(grid, new v2i(x, gridSize.Y - 1), 'O');
		}

		// first/last column
		for (var y = 0; y < gridSize.Y; y++)
		{
			CharMap.FloodFillNonRec(grid, new v2i(0, y), 'O');
			CharMap.FloodFillNonRec(grid, new v2i(gridSize.X - 1, y), 'O');
		}
		DebugPrintLagoon(grid);

		return grid;
	}

	public static char[][] CreateGrid((int DirId, int Dist, v3i Color)[] digPlan, out v2i gridSize)
	{
		var min = new v2i(long.MaxValue);
		var max = new v2i(long.MinValue);
		var trench = new List<v2i>();
		var p0 = v2i.Zero;
		foreach (var (DirId, Dist, Color) in digPlan)
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

	static void DebugPrintLagoon(char[][] grid)
	{
#if DEBUG
		for (var y = 0; y < grid.Length; y++)
			Debug.Line(new string(grid[y]));
		Debug.Line();
#endif
	}
}