using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2016_18;

[AoCPuzzle(Year = 2016, Day = 18, Answer1 = "1974", Answer2 = "19991126", Skip = false)]
public class LikeARogue : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var firstRow = input.Single().Select(x => x == '.').ToArray();

		// part1
		var answer1 = SafeTilesCount(firstRow, 40);

		// part2
		var answer2 = SafeTilesCount(firstRow, 400000);

		return (answer1.ToString(), answer2.ToString());
	}

	private static int SafeTilesCount(bool[] firstRowSafeTiles, int rowsCount)
	{
		var row = firstRowSafeTiles;
		var count = row.Count(x => x);
		for (var i = 1; i < rowsCount; i++)
		{
			row = NextRowSafeTiles(row);
			count += row.Count(x => x);
		}

		return count;
	}

	private static bool[] NextRowSafeTiles(bool[] prevRowSafeTiles)
	{
		var newRowSafeTiles = new bool[prevRowSafeTiles.Length];

		newRowSafeTiles[0] = !IsTrap(false, !prevRowSafeTiles[0], !prevRowSafeTiles[1]);
		for (var i = 1; i < prevRowSafeTiles.Length - 1; i++)
			newRowSafeTiles[i] = !IsTrap(!prevRowSafeTiles[i - 1], !prevRowSafeTiles[i], !prevRowSafeTiles[i + 1]);
		newRowSafeTiles[prevRowSafeTiles.Length - 1] = !IsTrap(!prevRowSafeTiles[^2], !prevRowSafeTiles[^1], false);

		return newRowSafeTiles;
	}

	private static bool IsTrap(bool leftTrap, bool centerTrap, bool rightTrap) => 
		(leftTrap && centerTrap && !rightTrap) ||
		(!leftTrap && centerTrap && rightTrap) ||
		(leftTrap && !centerTrap && !rightTrap) ||
		(!leftTrap && !centerTrap && rightTrap);
}