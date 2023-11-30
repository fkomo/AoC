using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2017_04;

[AoCPuzzle(Year = 2017, Day = 04, Answer1 = "325", Answer2 = "119", Skip = false)]
public class HighEntropyPassphrases : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var answer1 = input.Count(pp => pp
			.Split(' ')
			.GroupBy(x => x)
			.All(x => x.Count() == 1));

		// part2
		var answer2 = input.Count(pp => pp
			.Split(' ')
			.Select(x => new string(x.OrderBy(w => w).ToArray()))
			.GroupBy(x => x)
			.All(x => x.Count() == 1));

		return (answer1.ToString(), answer2.ToString());
	}
}