using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2023_01;

[AoCPuzzle(Year = 2023, Day = 01, Answer1 = "55002", Answer2 = null, Skip = false)]
public class Trebuchet : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var answer1 = input
			.Select(x => DigitsOnly(x))
			.Select(d => d.Length == 1 ? 11 * (d[0] - '0') : 10 * (d[0] - '0') + (d.Last() - '0'))
			.Sum();

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	public static string DigitsOnly(string s)
		=> s == null ? s : new(s.Where(c => char.IsDigit(c)).ToArray());
}