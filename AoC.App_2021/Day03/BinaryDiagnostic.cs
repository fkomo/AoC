using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day03
{
	internal class BinaryDiagnostic : ProblemBase
	{
		protected override (long, long) SolveProblem()
		{
			var input = ReadInputLines()
				.Select(s => Convert.ToInt32(s, 2)).ToArray();
			DebugLine($"{ input.Length } diagnostic reports");

			var bitCount = ReadInputLine(0).Length;
			var inputMask = (long)Math.Pow(2, bitCount) - 1;

			// part1
			var shift = 1;
			var gammaRate = 0;
			for (var i = bitCount - 1; i >= 0; i--)
			{
				var _1count = input.Count(b => (b & shift) == shift);
				var significant = _1count > (input.Length - _1count) ? 1 : 0;
				gammaRate += significant * shift;
				shift <<= 1;
			}
			var epsilonRate = ~gammaRate & inputMask;
			var result1 = gammaRate * epsilonRate;

			// part2
			var tmpInput = input;
			for (shift = bitCount - 1; tmpInput.Length > 1 && shift >= 0; shift--)
			{
				var mostSig = tmpInput.Count(i => (i >> shift & 1) == 1) * 2 >= tmpInput.Length ? 1 : 0;
				tmpInput = tmpInput.Where(i => (i >> shift & 1) == mostSig).ToArray();
			}
			var oxygenGeneratorRating = tmpInput.Single();

			tmpInput = input;
			for (shift = bitCount - 1; tmpInput.Length > 1 && shift >= 0; shift--)
			{
				var leastSig = tmpInput.Count(i => (i >> shift & 1) == 1) * 2 >= tmpInput.Length ? 0 : 1;
				tmpInput = tmpInput.Where(i => (i >> shift & 1) == leastSig).ToArray();
			}
			var co2ScrubberRating = tmpInput.Single();

			var result2 = oxygenGeneratorRating * co2ScrubberRating;

			return (result1, result2);
		}
	}
}
