using System.Collections.Concurrent;
using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_11;

[AoCPuzzle(Year = 2018, Day = 11, Answer1 = "235,60", Answer2 = "233,282,11", Skip = true)]
public class ChronalCharge : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var gridSerialNumber = int.Parse(input[0]);

		var grid = new aab2i(new v2i(1), new v2i(300))
			.EnumPoints()
			.AsJaggedArray(x => x - 1, x => GetPowerLevelAt(x, gridSerialNumber));

		// part1
		GetMaxPowerSquare(grid, 3, out v2i topLeft);

		var answer1 = $"{topLeft.X},{topLeft.Y}";

		// part2
		// TODO 2018/11 OPTIMIZE p2 (50s)
		var bag = new ConcurrentBag<(int, v2i, long)>();

		Parallel.For(4, 301, (size) =>
		{
			var maxPower = GetMaxPowerSquare(grid, size, out v2i topLeft);
			bag.Add((size, topLeft, maxPower));
		});

		var maxSquare = bag.OrderByDescending(x => x.Item3).First();

		var answer2 = $"{maxSquare.Item2.X},{maxSquare.Item2.Y},{maxSquare.Item1}";

		return (answer1?.ToString(), answer2?.ToString());
	}

	static long GetMaxPowerSquare(long[][] grid, int squareSize, out v2i cell)
	{
		var square = new aab2i(new v2i(0), new v2i(squareSize - 1)).EnumPoints().ToArray();

		var maxPowerSquareCell = new aab2i(new v2i(1), new v2i(300) - (squareSize - 1))
			.EnumPoints()
			.ToDictionary(x => x, x => square.Select(xx => grid.Get(x + xx - 1)).Sum())
			.OrderByDescending(x => x.Value)
			.First();

		cell = maxPowerSquareCell.Key;
		
		return maxPowerSquareCell.Value;
	}

	static long GetPowerLevelAt(v2i cell, int gridSerialNumber)
	{
		var rackId = cell.X + 10;
		return (((rackId * cell.Y + gridSerialNumber) * rackId % 1000) / 100) - 5;
	}
}