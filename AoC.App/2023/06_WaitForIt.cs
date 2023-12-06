using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2023_06;

[AoCPuzzle(Year = 2023, Day = 06, Answer1 = "316800", Answer2 = "45647654", Skip = false)]
public class WaitForIt : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var times = input.First().ToNumArray();
		var distances = input.Last().ToNumArray();

		// part1
		var answer1 = times.Zip(distances)
			.Select(x => Enumerable.Range(0, (int)x.First + 1).Count(t => (x.First - t) * t > x.Second))
			.Aggregate((x, y) => x * y);

		// part2
		var time = long.Parse(string.Join(string.Empty, times));
		var distance = long.Parse(string.Join(string.Empty, distances));
		Debug.Line($"time:{time}");
		Debug.Line($"distance: {distance}");

		var answer2 = 0;
		for (long t = 0; t <= time; t++)
			if (((time - t) * t) > distance)
				answer2++;

		return (answer1.ToString(), answer2.ToString());
	}
}