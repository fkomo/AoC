using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2017_19;

[AoCPuzzle(Year = 2017, Day = 19, Answer1 = "GEPYAWTMLK", Answer2 = "17628", Skip = false)]
public class ASeriesOfTubes : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var answer1 = FollowPath(new v2i(input[0].IndexOf('|'), 0), v2i.Up, input, out int steps);

		// part2
		var answer2 = steps;

		return (answer1?.ToString(), answer2.ToString());
	}

	static string FollowPath(v2i p, v2i dir, string[] map, out int steps)
	{
		var path = string.Empty;

		var current = map[p.Y][(int)p.X];
		for (steps = 1; current != ' '; steps++)
		{
			if (char.IsLetter(current))
				path += current;

			p += dir;
			current = map[p.Y][(int)p.X];

			// end of line
			if (current == ' ')
				break;

			if (current != '+')
				continue;

			// crossroads
			steps++;
			foreach (var d in v2i.UpDownLeftRight)
			{
				if (d == dir.Inv())
					continue;

				var next = p + d;
				if (next.X < 0 || next.Y < 0 || next.Y >= map.Length || next.X >= map[next.Y].Length)
					continue;

				if (map[next.Y][(int)next.X] != ' ')
				{
					p = next;
					dir = d;
					current = map[p.Y][(int)p.X];
					break;
				}
			}
		}

		return path;
	}
}