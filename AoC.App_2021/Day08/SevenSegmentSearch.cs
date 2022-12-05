using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day08
{
	internal class SevenSegmentSearch : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			// part1
			long answer1 = input
				.Sum(i => i.Split(" | ")[1].Split(' ').Count(s => s.Length == 2 || s.Length == 3 || s.Length == 4 || s.Length == 7));

			// part2
			// TODO 2021/08 part2
			long? answer2 = null;

			return (answer1.ToString(), answer2?.ToString());
		}
	}
}
