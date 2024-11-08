using Ujeby.AoC.Common;
using Ujeby.Tools;

namespace Ujeby.AoC.App._2017_06;

[AoCPuzzle(Year = 2017, Day = 06, Answer1 = "6681", Answer2 = "2392", Skip = false)]
public class MemoryReallocation : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var banks = input[0].Split('\t').Select(byte.Parse).ToArray();
		Debug.Line($"total block count {banks.Sum(x => x)}");

		// part1
		var answer1 = Realloc(banks, out byte[] result);

		// part2
		var answer2 = Realloc(result, out _);

		return (answer1.ToString(), answer2.ToString());
	}

	static int Realloc(byte[] banks, out byte[] result)
	{
		result = [.. banks];

		var hs = new HashSet<byte[]>(new ByteArrayComparer());
		while (hs.Add([.. result]))
		{
			var max = 0;
			var maxBlocks = result[0];
			for (var i = 1; i < result.Length; i++)
			{
				if (result[i] <= maxBlocks)
					continue;

				max = i;
				maxBlocks = result[i];
			}

			result[max] = 0;
			while (maxBlocks > 0)
			{
				max = (max + 1) % result.Length;
				result[max]++;
				maxBlocks--;
			}
		}

		return hs.Count;
	}
}