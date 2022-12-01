using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day00
{
	internal class Sample : ProblemBase
	{
		protected override (long, long) SolveProblem()
		{
			var input = ReadInputLines();
			DebugLine($"{ input.Length } lines");

			// part1
			long result1 = 0;

			// part2
			long result2 = 0;

			return (result1, result2);
		}
	}
}
