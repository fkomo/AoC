using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2025_10;

[AoCPuzzle(Year = 2025, Day = 10, Answer1 = null, Answer2 = null, Skip = false)]
public class Factory : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var machines = input
			.Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
			.Select(x => (
				LightDiagram: x[0].Trim('[', ']'), 
				Buttons: x[1..^1].Select(xx => xx.Trim(')', '(').Split(',').Select(xxx => int.Parse(xxx)).ToArray()).ToArray(),
				Joltage: x[^1].Trim('{', '}').Split(',').Select(xx => int.Parse(xx)).ToArray())
				)
			.ToArray();

        // part1
        string answer1 = null;

		// part2
		string answer2 = null;

		return (answer1?.ToString(), answer2?.ToString());
	}
}