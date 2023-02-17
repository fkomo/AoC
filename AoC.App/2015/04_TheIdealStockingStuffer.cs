using System.Security.Cryptography;
using System.Text;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_04
{
	[AoCPuzzle(Year = 2015, Day = 04, Answer1 = "282749", Answer2 = "9962624")]
	public class TheIdealStockingStuffer : PuzzleBase
	{
		private readonly static MD5 _md5 = MD5.Create();

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var secretKey = input.Single();

			// part1
			long answer1 = 1;
			while (true)
			{
				var value = $"{secretKey}{answer1}";
				if (CheckHash(value))
					break;

				answer1++;
			}

			// part2
			long answer2 = answer1 + 1;
			while (true)
			{
				var value = $"{secretKey}{answer2}";
				if (CheckHash(value, zeroes: 6))
					break;

				answer2++;
			}

			return (answer1.ToString(), answer2.ToString());
		}

		private static bool CheckHash(string value,
			int zeroes = 5)
			=> Convert.ToHexString(_md5.ComputeHash(Encoding.ASCII.GetBytes(value)))
				.Take(zeroes).All(c => c == '0');
	}
}
