using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day00
{
	internal class Sample : ProblemBase
	{
		protected override (long, long) SolveProblem()
		{
			long result1 = 0;
			long result2 = 0;
			var input = ReadInputLines();
			DebugLine($"{ input.Length } lines");

			return (result1, result2);
		}
	}
}
