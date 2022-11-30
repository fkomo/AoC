using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day03
{
	internal class BinaryDiagnostic : ProblemBase
	{
		protected override (long, long) SolveProblem()
		{
			long result1 = 0;
			long result2 = 0;
			var input = ReadInputLines();
			DebugLine($"{ input.Length } diagnostic reports");

			var bits = input.First().Length;

			var gammaRate = 0;

			var shift = 1;
			for (var i = bits - 1; i >= 0; i--)
			{
				var significant = input.Count(l => l[i] == '1') > (input.Length / 2) ? 1 : 0;
				gammaRate += significant * shift;
				shift *= 2;
			}
			var epsilonRate = ~gammaRate & ((long)Math.Pow(2, bits) - 1);
			result1 = gammaRate * epsilonRate;

			return (result1, result2);
		}
	}
}
