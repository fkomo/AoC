using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_04
{
	[AoCPuzzle(Year = 2015, Day = 04, Answer1 = "282749", Answer2 = "9962624", Skip = true)]
	public class TheIdealStockingStuffer : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var secretKey = input.Single();

			// TODO 2015/04 OPTIMIZE (4s)

			// part1
			var answer1 = FindZeroHash(secretKey, 1, 5);

			// part2
			var answer2 = FindZeroHash(secretKey, answer1 + 1, 6);

			return (answer1.ToString(), answer2.ToString());
		}

		private static long FindZeroHash(string secretKey, long start, int leadingZeroes)
		{
			while (!VerifyHash($"{secretKey}{start++}", leadingZeroes))
			{ 
			}

			return start - 1;
		}

		private static bool VerifyHash(string value, int leadingZeroes)
			=> Tools.Hashing.HashMd5(value)
				.Take(leadingZeroes)
				.All(c => c == '0');
	}
}
