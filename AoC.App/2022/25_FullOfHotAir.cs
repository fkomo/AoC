using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2022_25
{
	public class FullOfHotAir : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			var answer1 = DecToSnafu(input.Sum(line => SnafuToDec(line)));

			return (answer1?.ToString(), "*");
		}

		private static long SnafuToDec(string snafu) => Tools.Numbers.BaseToDec(snafu, "=-012", -2);
		private static string DecToSnafu(long dec) => Tools.Numbers.DecToBase(dec, "=-012", -2);
	}
}
