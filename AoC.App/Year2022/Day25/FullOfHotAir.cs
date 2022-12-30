using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day25
{
	public class FullOfHotAir : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			var answer1 = DecToSnafu(input.Sum(line => SnafuToDec(line)));

			// part2
			// TODO 2022/25 p2
			string answer2 = null;

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long SnafuToDec(string snafu) => Tools.Numbers.BaseToDec(snafu, "=-012", -2);
		private static string DecToSnafu(long dec) => Tools.Numbers.DecToBase(dec, "=-012", -2);
	}
}
