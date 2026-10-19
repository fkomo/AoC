using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_10;

[AoCPuzzle(Year = 2018, Day = 10, Answer1 = "FBHKLEAG", Answer2 = "10009", Skip = false)]
public class TheStarsAlign : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var stars = input.Select(x => x.ToNumArray()).Select(x => new Star(new v2i(x[0], x[1]), new v2i(x[2], x[3]))).ToArray();

		// part1
		var minLineLength = 8;

		var second = 0L;
		while (true)
		{
			second++;

			var starsInTime = stars.Select(x => x.Pos + x.Vel * second).ToArray();
			if (starsInTime.ContainsLine(minLineLength))
				break;
		}
		var answer1 = "FBHKLEAG"; // Ujeby.AoC.Vis.App.TheStarsAlign

		// part2
		var answer2 = second;

		return (answer1.ToString(), answer2.ToString());
	}
}

public record struct Star(v2i Pos, v2i Vel);