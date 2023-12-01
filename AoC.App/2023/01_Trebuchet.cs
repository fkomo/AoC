using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2023_01;

[AoCPuzzle(Year = 2023, Day = 01, Answer1 = "55002", Answer2 = "55093", Skip = false)]
public class Trebuchet : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var answer1 = input
			.Select(x => CalibrationValue(x.DigitsOnly()))
			.Sum();

		// part2
		var digits = new string[]
		{
			"zero",
			"one",
			"two",
			"three",
			"four",
			"five",
			"six",
			"seven",
			"eight",
			"nine"
		};

		var digitsChars = Enumerable.Range(1, 9).Select(x => (char)('0' + x)).ToArray();

		// replace first and last text digit with its number representation
		var modifiedInput = input
			.Select(x =>
			{
				var firstDigit = x.IndexOfFirstOccurance(digitsChars);
				if (firstDigit == -1)
					firstDigit = x.Length;

				// work only with substring 0..firstDigit
				var minDigit = -1;
				var minIdx = int.MaxValue;
				for (var i = 0; i < digits.Length; i++)
				{
					var idx = x.IndexOf(digits[i], 0, firstDigit);
					if (idx > -1 && idx < minIdx)
					{
						minIdx = idx;
						minDigit = i;
					}
				}
				if (minDigit != -1)
					x = x[..minIdx] + minDigit.ToString() + x[(minIdx + digits[minDigit].Length)..];

				var lastDigit = x.IndexOfLastOccurance(digitsChars);
				if (lastDigit == -1)
					lastDigit = 0;

				// work olny with substring lastDigit..
				var maxDigit = -1;
				var maxIdx = int.MinValue;
				for (var i = 0; i < digits.Length; i++)
				{
					var idx = x.LastIndexOf(digits[i], x.Length - 1, x.Length - lastDigit);
					if (idx > -1 && idx > maxIdx)
					{
						maxIdx = idx;
						maxDigit = i;
					}
				}
				if (maxDigit != -1)
					x = x[..maxIdx] + maxDigit.ToString() + x[(maxIdx + digits[maxDigit].Length)..];

				return x;
			})
			.ToArray();

		var sums = modifiedInput
			.Select(x => CalibrationValue(x.DigitsOnly()))
			.ToArray();

		for (var i = 0; i < modifiedInput.Length; i++)
			Debug.Line($"{input[i],55} | {modifiedInput[i],55} | {sums[i]}");

		var answer2 = sums.Sum();

		return (answer1.ToString(), answer2.ToString());
	}

	static int CalibrationValue(string s)
		=> s.Length == 0 ? 0 : s.Length == 1 ? 11 * (s[0] - '0') : 10 * (s[0] - '0') + (s.Last() - '0');
}