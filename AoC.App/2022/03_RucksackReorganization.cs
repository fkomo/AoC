using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2022_03
{
	[AoCPuzzle(Year = 2022, Day = 03, Answer1 = "8394", Answer2 = "2413")]
	internal class RucksackReorganization : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var allChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

			// part1
			long answer1 = input
				.Select(r => allChars.IndexOf(r[..(r.Length / 2)].Intersect(r[(r.Length / 2)..]).Single()) + 1)
				.Sum();

			// part2
			long answer2 = 0;
			for (var i = 0; i < input.Length; i += 3)
				answer2 += allChars.IndexOf(input[i].Intersect(input[i + 1]).Intersect(input[i + 2]).Single()) + 1;

			return (answer1.ToString(), answer2.ToString());
		}
	}
}