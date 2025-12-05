using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2025_05;

[AoCPuzzle(Year = 2025, Day = 05, Answer1 = "726", Answer2 = null, Skip = false)]
public class Cafeteria : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var db = input.Split(string.Empty);

		var fresh = db[0]
			.Select(x => new v2i([.. x.Split('-').Select(x => long.Parse(x))]))
			.ToArray();

		var available = db[1]
			.Select(long.Parse)
			.ToArray();

		// part1
		var answer1 = available.Count(x => fresh.Any(xx => xx.X <= x && x <= xx.Y));

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}