using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day01
{
	internal class SonarSweep : ProblemBase
	{
		protected override (long, long) SolveProblem()
		{
			long result1 = 0;
			long result2 = 0;
			var input = ReadInputLines();
			DebugLine($"{ input.Length } depth measurements");

			// part1
			for (var i = 1; i < input.Length; i++)
				if (int.Parse(input[i - 1]) < int.Parse(input[i]))
					result1++;

			// part2
			for (var i = 0; i < input.Length - 3; i++)
			{
				var sum1 = 
					int.Parse(input[i + 0]) + 
					int.Parse(input[i + 1]) + 
					int.Parse(input[i + 2]);

				var sum2 =
					int.Parse(input[i + 1]) +
					int.Parse(input[i + 2]) +
					int.Parse(input[i + 3]);

				if (sum2 > sum1)
					result2++;
			}

			return (result1, result2);
		}
	}
}
