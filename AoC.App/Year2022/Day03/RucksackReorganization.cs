using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day03
{
	internal class RucksackReorganization : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			long answer1 = input
				.Select(r => Const.AllChars.IndexOf(r[..(r.Length / 2)].Intersect(r[(r.Length / 2)..]).Single()) + 1)
				.Sum();

			// part2
			long answer2 = 0;
			for (var i = 0; i < input.Length; i += 3)
				answer2 += Const.AllChars.IndexOf(input[i].Intersect(input[i + 1]).Intersect(input[i + 2]).Single()) + 1;

			return (answer1.ToString(), answer2.ToString());
		}
	}
}