using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2025_05;

[AoCPuzzle(Year = 2025, Day = 05, Answer1 = "726", Answer2 = "354226555270043", Skip = false)]
public class Cafeteria : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var db = input.Split(string.Empty);

		var fresh = db[0]
			.Select(x => new v2i([.. x.Split('-').Select(x => long.Parse(x))]))
			.OrderBy(x => x.X)
			.ToArray();

		var available = db[1]
			.Select(long.Parse)
			.ToArray();

		// part1
		var answer1 = available.Count(x => fresh.Any(xx => xx.X <= x && x <= xx.Y));

		// part2
		var answer2 = 0L;
		var from = fresh[0].X;
		var to = fresh[0].Y;
		for (var i = 1; i < fresh.Length; i++)
		{
			if (fresh[i].Y <= to)
				continue;

			if (fresh[i].X > to)
			{
				answer2 += to - from + 1;

				from = fresh[i].X;
				to = fresh[i].Y;
			}
			else
			{
				answer2 += fresh[i].X - from;

				from = fresh[i].X;
				to = fresh[i].Y;
			}
		}
		answer2 += to - from + 1;

		return (answer1.ToString(), answer2.ToString());
	}
}