using System.Text;
using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_17;

[AoCPuzzle(Year = 2023, Day = 17, Answer1 = null, Answer2 = null, Skip = false)]
public class ClumsyCrucible : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var grid = input.Select(y => y.Select(x => x - '0').ToArray()).ToArray();

		// part1
		long? answer1 = null;
		// TODO 2023/17 p1

		//var answer1 = MinHeathLoss(grid, new v2i[] { v2i.Zero }, Array.Empty<int>(),
		//	minHeatLoss: 2 * input.Length * 9
		//	);

		// part2
		long? answer2 = null;
		// TODO 2023/17 p2

		return (answer1?.ToString(), answer2?.ToString());
	}

	static readonly Dictionary<(v2i Pos, int IncomingDir), long> _cache = new();

	/// <summary>
	/// TODO result is wrong, looks like it counts even first block (0,0)
	/// </summary>
	/// <param name="grid"></param>
	/// <param name="path"></param>
	/// <param name="last3"></param>
	/// <param name="minHeatLoss"></param>
	/// <param name="heatLoss"></param>
	/// <returns></returns>
	static long MinHeathLoss(int[][] grid, v2i[] path, int[] last3, 
		long minHeatLoss = long.MaxValue, long heatLoss = 0)
	{
		var p = path[^1];
		heatLoss += grid[p.Y][p.X];
		if (heatLoss > minHeatLoss)
			return long.MaxValue;

		var heatLossToEnd = long.MaxValue;
		if (p.X == grid.Length - 1 && p.Y == grid.Length - 1)
		{
			Debug.Line($"heatLoss: {heatLoss}");
			DebugGridPath(grid, path);

			return grid[p.Y][p.X];
		}

		//if (last3.Any() && _cache.TryGetValue((path[^1], last3[^1]), out long cachedHeatLoss))
		//	return cachedHeatLoss;

		for (var i = 0; i < v2i.UpDownLeftRight.Length; i++) 
		{
			// max 3 blocks in single direction
			if (last3.Length == 3 && last3[0] == last3[1] && last3[1] == last3[2] && last3[2] == i)
				continue;

			var p2 = p + v2i.UpDownLeftRight[i];

			// out of bounds
			if (p2.X < 0 || p2.Y < 0 || p2.X >= grid.Length || p2.Y >= grid.Length)
				continue;

			// not visiting same block twice (so no coming back)
			if (path.Contains(p2))
				continue;

			var mhl = MinHeathLoss(grid, 
				path.Concat(new v2i[] { p2 }).ToArray(),
				last3.Skip(last3.Length == 3 ? 1 : 0).Concat(new int[] { i }).ToArray(),
				minHeatLoss: minHeatLoss,
				heatLoss: heatLoss);
			if (mhl < heatLossToEnd)
			{
				heatLossToEnd = mhl;
				minHeatLoss = heatLoss + heatLossToEnd;
			}
		}

		if (heatLossToEnd == long.MaxValue)
			return long.MaxValue;

		heatLossToEnd += grid[p.Y][p.X];
		//if (last3.Any() && minHeatLoss < 2 * grid.Length * 9)
		//	_cache.Add((path[^1], last3[^1]), heatLossToEnd);

		return heatLossToEnd;
	}

	static void DebugGridPath(int[][] grid, v2i[] path)
	{
		var sb = new StringBuilder();
		for (var y  = 0; y < grid.Length; y++)
		{
			sb.Clear();
			for (var x = 0; x < grid[y].Length; x++)
			{
				if (path.Contains(new v2i(x, y)))
					sb.Append('█');
				else
					sb.Append(grid[y][x]);
			}
			Debug.Line(sb.ToString());
		}
		Debug.Line();
	}
}