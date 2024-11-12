using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2017_15;

[AoCPuzzle(Year = 2017, Day = 15, Answer1 = "573", Answer2 = "294", Skip = false)]
public class DuelingGenerators : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var mod = 2147483647L;
		long[] factors = [16807, 48271];

		// part1
		var answer1 = 0;
		var gen = input.Select(x => x.ToNumArray()[0]).ToArray();
		for (var i = 0; i < 40_000_000; i++)
		{
			gen[0] = (gen[0] * factors[0]) % mod;
			gen[1] = (gen[1] * factors[1]) % mod;
			if ((gen[0] & 0x000000000000ffff) == (gen[1] & 0x000000000000ffff))
				answer1++;
		}

		// part2
		var answer2 = 0;
		long[] muls = [4, 8];
		gen = input.Select(x => x.ToNumArray()[0]).ToArray();
		for (var i = 0; i < 5_000_000; i++)
		{
			for (var g = 0; g < gen.Length; g++)
			{
				do
				{
					gen[g] = (gen[g] * factors[g]) % mod;
				}
				while (gen[g] % muls[g] != 0);
			}

			if ((gen[0] & 0x000000000000ffff) == (gen[1] & 0x000000000000ffff))
				answer2++;
		}

		return (answer1.ToString(), answer2.ToString());
	}
}