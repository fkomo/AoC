using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2025_01;

[AoCPuzzle(Year = 2025, Day = 01, Answer1 = "1177", Answer2 = null, Skip = false)]
public class SecretEntrance : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var rotations = input
			.Select(x => long.Parse(x.Replace('L', '-').TrimStart('R')))
			.ToArray();

		// part1
		var dial = 50L;
		var answer1 = 0L;
		for (var i = 0; i < rotations.Length; i++)
		{
			dial = (dial + rotations[i] + 100) % 100;
			if (dial == 0)
				answer1++;
		}

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}