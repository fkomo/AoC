using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2018_01;

[AoCPuzzle(Year = 2018, Day = 01, Answer1 = "520", Answer2 = "394", Skip = false)]
public class ChronalCalibration : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var changes = input.Select(int.Parse).ToArray();

		// part1
		var answer1 = changes.Sum();

		// part2
		var freqs = new HashSet<long>();

		var freq = 0L;
		for (var i = 0; freqs.Add(freq); i = (i + 1) % changes.Length)
			freq += changes[i];

		var answer2 = freq;

		return (answer1.ToString(), answer2.ToString());
	}
}