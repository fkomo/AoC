using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2024_01;

[AoCPuzzle(Year = 2024, Day = 01, Answer1 = "3246517", Answer2 = "29379307", Skip = false)]
public class HistorianHysteria : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var listValues = input.Select(x => x.ToNumArray());
		var left = listValues.Select(x => x[0]).OrderBy(x => x).ToArray();
		var right = listValues.Select(x => x[1]).OrderBy(x => x).ToArray();

		// part1
		var answer1 = 0L;
		for (var i = 0; i < left.Length; i++)
			answer1 += System.Math.Abs(left[i] - right[i]);

		// part2
		var answer2 = left.Sum(x => right.Count(xx => x == xx) * x);

		return (answer1.ToString(), answer2.ToString());
	}
}