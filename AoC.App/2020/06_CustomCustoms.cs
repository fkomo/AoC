using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2020_06
{
	[AoCPuzzle(Year = 2020, Day = 06, Answer1 = "6297", Answer2 = "3158")]
	public class CustomCustoms : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			long? answer1 = input.Split(string.Empty)
				.Sum(g => string.Join(string.Empty, g).Distinct().Count());

			// part2
			long? answer2 = input.Split(string.Empty)
				.Sum(g =>
				{
					var intersect = g[0].ToCharArray();
					for (var i = 1; i < g.Length; i++)
						intersect = intersect.Intersect(g[i]).ToArray();

					return intersect.Distinct().Count();
				});

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
