using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2017_02
{
	[AoCPuzzle(Year = 2017, Day = 02, Answer1 = "51139", Answer2 = "272", Skip = false)]
	public class CorruptionChecksum : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			// part1
			var answer1 = input.Sum(r => r.ToNumArray().Max() - r.ToNumArray().Min());

			// part2
			var answer2 = input.Sum(r =>
			{
				var ro = r.ToNumArray().OrderByDescending(x => x).ToArray();
				for (var n1 = 0; n1 < ro.Length - 1; n1++)
					for (var n2 = n1 + 1; n2 < ro.Length; n2++)
						if (ro[n1] % ro[n2] == 0)
							return ro[n1] / ro[n2];

				throw new Exception("?!");
			});

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
