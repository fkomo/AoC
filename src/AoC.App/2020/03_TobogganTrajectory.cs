using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2020_03
{
	[AoCPuzzle(Year = 2020, Day = 03, Answer1 = "223", Answer2 = "3517401300")]
	public class TobogganTrajectory : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			var answer1 = TraverseSlope(new(), new(3, 1), input);

			// part2
			var answer2 = new v2i[]
			{
				new v2i(1, 1),
				new v2i(3, 1),
				new v2i(5, 1),
				new v2i(7, 1),
				new v2i(1, 2),
			}
			.Select(step => TraverseSlope(new(), step, input))
			.Aggregate((a, b) => a * b);

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long? TraverseSlope(v2i start, v2i step, string[] map)
		{
			var result = 0L;

			var p = start;
			while (p.Y != map.Length - 1)
			{
				p = (p + step) % new v2i(map[0].Length, map.Length);
				if (map[p.Y][(int)p.X] == '#')
					result++;
			}

			return result;
		}
	}
}
