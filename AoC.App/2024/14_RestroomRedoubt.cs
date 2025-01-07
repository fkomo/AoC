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

		// check if robots form a border
		// if there is at least 2 horizontal and 2 vertical lines they probably form a border with tree image inside
		// also not checking for "straight" lines, just check if enough robots are at given tim in single row / column
		var borderLength = 30;
		var rows = new Dictionary<long, int>();
		var cols = new Dictionary<long, int>();
		while (cols.Count(x => x.Value >= borderLength) < 2 || rows.Count(x => x.Value >= borderLength) < 2)
		{
			second++;
			rows.Clear();
			cols.Clear();

			foreach (var robot in robots)
			{
				var pos = (((robot.Pos + robot.Vel * second) % space) + space) % space;

				if (!rows.ContainsKey(pos.Y))
					rows[pos.Y] = 0;

				if (!cols.ContainsKey(pos.X))
					cols[pos.X] = 0;

				rows[pos.Y]++;
				cols[pos.X]++;
			}
		}
		var answer2 = second;

		return (answer1.ToString(), answer2.ToString());
	}

	static IEnumerable<v2i> RobotsAt(Robot[] robots, v2i space, long second) => robots.Select(x => (((x.Pos + x.Vel * second) % space) + space) % space);
}

public record struct Robot(v2i Pos, v2i Vel);