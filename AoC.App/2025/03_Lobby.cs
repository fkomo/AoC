using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2025_03;

[AoCPuzzle(Year = 2025, Day = 03, Answer1 = "17435", Answer2 = "172886048065379", Skip = false)]
public class Lobby : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var banks = input
			.Select(x => x
				.Select(xx => xx - '0')
				.ToArray())
			.ToArray();

		// part1
		var answer1 = banks.Sum(x => GetNBatteryJoltage(x));

		// part2
		var answer2 = banks.Sum(x => GetNBatteryJoltage(x, batteryCount: 12));

		return (answer1.ToString(), answer2.ToString());
	}

	static long GetNBatteryJoltage(int[] bank, int batteryCount = 2)
	{
		var batteries = Enumerable.Repeat(-1, batteryCount).ToArray();

		for (var bankIdx = 0; bankIdx < bank.Length; bankIdx++)
		{
			var batteriesLeft = bank.Length - bankIdx;
			for (var batteryIdx = System.Math.Max(0, batteryCount - batteriesLeft); batteryIdx < batteryCount; batteryIdx++)
			{
				if (bank[bankIdx] > batteries[batteryIdx])
				{
					batteries[batteryIdx] = bank[bankIdx];

					for (var i = batteryIdx + 1; i < batteries.Length; i++)
						batteries[i] = -1;

					break;
				}
			}
		}

		var joltage = 0L;
		var n = 1L;
		for (int i = batteryCount - 1; i >= 0; i--, n *= 10)
			joltage += batteries[i] * n;

		return joltage;
	}
}