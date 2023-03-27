using System.Security.Cryptography;
using System.Text;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2016_05
{
	[AoCPuzzle(Year = 2016, Day = 05, Answer1 = "1a3099aa", Answer2 = null)]
	public class HowAboutANiceGameOfChess : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var doorId = input.Single();

			// part1
			string answer1 = string.Empty;
			for (long salt = 0, i = 0; i < 8; i++)
			{
				salt = Parallel.For(salt + 1, long.MaxValue, (s, state) =>
				{
					if (state.ShouldExitCurrentIteration && state.LowestBreakIteration < s)
						return;

					if (VerifyHash($"{doorId}{s}", 5, out string hash))
						state.Break();

				}).LowestBreakIteration.Value;

				VerifyHash($"{doorId}{salt}", 5, out string hash);
				answer1 += hash[5];
			}

			// part2
			var answer2 = 0;

			return (answer1, answer2.ToString());
		}

		private static bool VerifyHash(string value, int leadingZeroes, out string hash)
		{
			hash = Convert.ToHexString(MD5.HashData(Encoding.ASCII.GetBytes(value))).ToLower();
			return hash
				.Take(leadingZeroes)
				.All(c => c == '0');
		}
	}
}
