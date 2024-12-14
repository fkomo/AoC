using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_14;

[AoCPuzzle(Year = 2024, Day = 14, Answer1 = "228421332", Answer2 = "7790", Skip = false)]
public class RestroomRedoubt : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var robots = input.Select(x => x.ToNumArray()).Select(x => new Robot(new v2i(x[0], x[1]), new v2i(x[2], x[3]))).ToArray();
		var space = new v2i(101, 103);

		// part1
		var mid = space / 2;
		var counts = new v4i(0);
		foreach (var pos in RobotsAt(robots, space, 100))
		{
			if (pos.X == mid.X || pos.Y == mid.Y)
				continue;

			if (pos.X < mid.X)
			{
				if (pos.Y < mid.Y)
					counts[0]++;
				else
					counts[2]++;
			}
			else
			{
				if (pos.Y < mid.Y)
					counts[1]++;
				else
					counts[3]++;
			}
		}
		var answer1 = counts.Volume();

		// part2
		var second = 0;
		var somethingFound = false;

		// checks if robots are positioned in horizontal line
		var lineLength = 16;
		var line = Enumerable.Range(1, lineLength).Select(x => new v2i(x, 0)).ToArray();
		while (!somethingFound)
		{
			second += 10; // advance in time by magic constant :)
			
			var robotsAt = RobotsAt(robots, space, second).ToArray();

			if (robotsAt.Any(r => line.All(l => robotsAt.Contains(r + l))))
				break;
		}
		var answer2 = second;

		return (answer1.ToString(), answer2.ToString());
	}

	static IEnumerable<v2i> RobotsAt(Robot[] robots, v2i space, long second) => robots.Select(x => (((x.Pos + x.Vel * second) % space) + space) % space);
}

public record struct Robot(v2i Pos, v2i Vel);