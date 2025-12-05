using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2025_04;

[AoCPuzzle(Year = 2025, Day = 04, Answer1 = "1424", Answer2 = null, Skip = false)]
public class PrintingDepartment : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToArray()).ToArray();

		// part1
		var answer1 = map
			.EnumAll('@')
			.Count(x => v2i.PlusMinusOne.Count(xx => map.TryGet(xx + x, out char neighbour) && neighbour == '@') < 4);

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}