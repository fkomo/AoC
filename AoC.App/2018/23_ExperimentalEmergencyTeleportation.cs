using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_23;

[AoCPuzzle(Year = 2018, Day = 23, Answer1 = "691", Answer2 = null, Skip = false)]
public class ExperimentalEmergencyTeleportation : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var nanobots = input
			.Select(x => x.ToNumArray())
			.Select(x => new { Pos = new v3i([.. x.Take(3)]), Radius = x[3] })
			.ToArray();

		// part1
		var largest = nanobots.OrderByDescending(x => x.Radius).First();
		var answer1 = nanobots.Count(x => v3i.ManhDistance(x.Pos, largest.Pos) <= largest.Radius);

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}