using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day08
{
	internal class SevenSegmentSearch : ProblemBase
	{
		protected override (long, long) SolveProblem()
		{
			var input = ReadInputLines();
			DebugLine($"{ input.Length } lines");

			// part1
			long result1 = 0;
			foreach (var line in input)
				result1 += line.Split(" | ")[1].Split(' ').Count(s => s.Length == 2 || s.Length == 3 || s.Length == 4 || s.Length == 7);

			// part2
			long result2 = 0;

			return (result1, result2);
		}
	}
}
