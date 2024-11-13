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
		var start = input.Select(x => x.ToNumArray()[0]).ToArray();

		// part1
		var answer1 = 0;
		var sampleCount = 40_000_000;
		var generated = new UInt16[][] { new UInt16[sampleCount], new UInt16[sampleCount] };

		Parallel.For(0, 2, (g) =>
		{
			var last = start[g];
			var gen = generated[g];
			var factor = factors[g];

			for (var i = 0; i < sampleCount; i++)
			{
				last = (last * factor) % mod;
				gen[i] = (UInt16)(last & 0x000000000000ffff);
			}
		});
		for (var i = 0; i < sampleCount; i++)
			if (generated[0][i] == generated[1][i])
				answer1++;

		// part2
		var answer2 = 0;
		long[] muls = [4, 8];
		sampleCount = 5_000_000;
		generated = [new UInt16[sampleCount], new UInt16[sampleCount]];
		
		Parallel.For(0, 2, (g) =>
		{
			var mul = muls[g];
			var last = start[g];
			var gen = generated[g];
			var factor = factors[g];

			for (var i = 0; i < sampleCount; i++)
			{
				do
				{
					last = (last * factor) % mod;
				}
				while (last % mul != 0);

				gen[i] = (UInt16)(last & 0x000000000000ffff);
			}
		});
		for (var i = 0; i < sampleCount; i++)
			if (generated[0][i] == generated[1][i])
				answer2++;

		return (answer1.ToString(), answer2.ToString());
	}
}