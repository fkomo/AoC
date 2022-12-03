using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day03
{
	internal class RucksackReorganization : ProblemBase
	{
		protected override (long, long) SolveProblem(string[] input)
		{
			DebugLine($"{ input.Length } rucksacks");

			// part1
			long result1 = 0;
			foreach (var r in input)
			{
				var error = r[..(r.Length / 2)].Intersect(r[(r.Length / 2)..]).Single();
				result1 += error >= 'a' ? error - 'a' + 1 : error - 'A' + 27;
			}

			// part2
			long result2 = 0;
			for (var i = 0; i < input.Length; i += 3)
			{
				var badge = input[i].Intersect(input[i + 1]).Intersect(input[i + 2]).Single();
				result2 += badge >= 'a' ? badge - 'a' + 1 : badge - 'A' + 27;
			}

			return (result1, result2);
		}
	}
}
