using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day03
{
	internal class RucksackReorganization : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			// part1
			long result1 = input
				.Select(r => 
					"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
						.IndexOf(r[..(r.Length / 2)].Intersect(r[(r.Length / 2)..]).Single()) + 1)
				.Sum();

			// part2
			long result2 = 0;
			for (var i = 0; i < input.Length; i += 3)
				result2 += "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
					.IndexOf(input[i].Intersect(input[i + 1]).Intersect(input[i + 2]).Single()) + 1;

			return (result1.ToString(), result2.ToString());
		}
	}
}