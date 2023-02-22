using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2021_03
{
	[AoCPuzzle(Year = 2021, Day = 03, Answer1 = "2250414", Answer2 = "6085575")]
	public class BinaryDiagnostic : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var inputN = input
				.Select(s => Convert.ToInt32(s, 2)).ToArray();

			var bitCount = input.First().Length;
			var inputMask = (long)Math.Pow(2, bitCount) - 1;

			// part1
			var shift = 1;
			var gammaRate = 0;
			for (var i = bitCount - 1; i >= 0; i--)
			{
				var _1count = inputN.Count(b => (b & shift) == shift);
				var significant = _1count > (inputN.Length - _1count) ? 1 : 0;
				gammaRate += significant * shift;
				shift <<= 1;
			}
			var epsilonRate = ~gammaRate & inputMask;
			var answer1 = gammaRate * epsilonRate;

			// part2
			var tmpInput = inputN;
			for (shift = bitCount - 1; tmpInput.Length > 1 && shift >= 0; shift--)
			{
				var mostSig = tmpInput.Count(i => (i >> shift & 1) == 1) * 2 >= tmpInput.Length ? 1 : 0;
				tmpInput = tmpInput.Where(i => (i >> shift & 1) == mostSig).ToArray();
			}
			var oxygenGeneratorRating = tmpInput.Single();

			tmpInput = inputN;
			for (shift = bitCount - 1; tmpInput.Length > 1 && shift >= 0; shift--)
			{
				var leastSig = tmpInput.Count(i => (i >> shift & 1) == 1) * 2 >= tmpInput.Length ? 0 : 1;
				tmpInput = tmpInput.Where(i => (i >> shift & 1) == leastSig).ToArray();
			}
			var co2ScrubberRating = tmpInput.Single();

			var answer2 = oxygenGeneratorRating * co2ScrubberRating;

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
