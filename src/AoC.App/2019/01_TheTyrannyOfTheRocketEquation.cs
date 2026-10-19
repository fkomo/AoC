using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2019_01;

[AoCPuzzle(Year = 2019, Day = 01, Answer1 = "3386686", Answer2 = null, Skip = false)]
public class TheTyrannyOfTheRocketEquation : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var answer1 = input
			.Select(x => long.Parse(x))
			.Sum(x => x / 3 - 2);

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}