using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2025_03;

[AoCPuzzle(Year = 2025, Day = 03, Answer1 = "17435", Answer2 = null, Skip = false)]
public class Lobby : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var banks = input.Select(x => x.Select(x => x - '0').ToArray()).ToArray();

		// part1
		var answer1 = banks.Sum(GetJoltage);

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static int GetJoltage(int[] bank)
	{
		var first = -1;
		var second = -1;

		for (var b = 0; b < bank.Length - 1; b++)
		{
			if (bank[b] > first)
			{
				first = bank[b];
				second = -1;
			}
			else if (bank[b] > second)
			{
				second = bank[b];
				if (second == 9)
					return 99;
			}
		}

		if (bank[^1] > second)
			second = bank[^1];

		return 10 * first + second;
	}
}