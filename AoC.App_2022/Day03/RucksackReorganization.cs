using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day03
{
	internal class RucksackReorganization : ProblemBase
	{
		protected override (long, long) SolveProblem(string[] input)
		{
			DebugLine($"{ input.Length } rucksacks");

			var items = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

			// part1
			long result1 = input
				.Select(r => items.IndexOf(r[..(r.Length / 2)].Intersect(r[(r.Length / 2)..]).Single()) + 1)
				.Sum();

			// part2
			long result2 = 0;
			for (var i = 0; i < input.Length; i += 3)
				result2 += items.IndexOf(input[i].Intersect(input[i + 1]).Intersect(input[i + 2]).Single()) + 1;

			return (result1, result2);
		}
	}
}