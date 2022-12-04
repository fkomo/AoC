using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day08
{
	internal class SevenSegmentSearch : ProblemBase
	{
		protected override (long?, long?) SolveProblem(string[] input)
		{
			// part1
			long result1 = input
				.Sum(i => i.Split(" | ")[1].Split(' ').Count(s => s.Length == 2 || s.Length == 3 || s.Length == 4 || s.Length == 7));

			// part2
			// TODO 2021/08 part2
			long? result2 = null;

			return (result1, result2);
		}
	}
}
