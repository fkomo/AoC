using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2015_02
{
	[AoCPuzzle(Year = 2015, Day = 02, Answer1 = "1586300", Answer2 = "3737498")]
	public class IWasToldThereWouldBeNoMath : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var presents = input.Select(line => new v3i(line.ToNumArray())).ToArray();

			// part1
			var answer1 = 0L;

			// part2
			var answer2 = 0L;

			foreach (var present in presents)
			{
				var surface = 0L;
				var smallestSide = long.MaxValue;
				for (var i = 0; i < 3; i++)
				{
					var side = present[i % 3] * present[(i + 1) % 3];
					surface += side;

					if (side < smallestSide)
						smallestSide = side;
				}

				answer1 += smallestSide + 2 * surface;
				answer2 += present.Volume() + present.ToArray().OrderBy(x => x).Take(2).Sum() * 2;
			}

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
