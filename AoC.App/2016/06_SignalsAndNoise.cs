using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2016_06
{
	[AoCPuzzle(Year = 2016, Day = 06, Answer1 = "gebzfnbt", Answer2 = "fykjtwyn")]
	public class SignalsAndNoise : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			// part1
			var answer1 = new string(Enumerable.Range(0, input.First().Length).Select(i => 
				input.Select(x => x[i])
					.GroupBy(x => x)
					.ToDictionary(x => x.Key, x => x.Count())
					.OrderByDescending(x => x.Value)
					.First()
					.Key).ToArray());

			// part2
			var answer2 = new string(Enumerable.Range(0, input.First().Length).Select(i =>
				input.Select(x => x[i])
					.GroupBy(x => x)
					.ToDictionary(x => x.Key, x => x.Count())
					.OrderBy(x => x.Value)
					.First()
					.Key).ToArray());

			return (answer1, answer2);
		}
	}
}
