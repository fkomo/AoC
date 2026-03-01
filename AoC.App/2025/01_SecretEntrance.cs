using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2025_01;

[AoCPuzzle(Year = 2025, Day = 01, Answer1 = "1177", Answer2 = "6768", Skip = false)]
public class SecretEntrance : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var rotations = input
			.Select(x => int.Parse(x.Replace('L', '-').TrimStart('R')))
			.ToArray();

		// part1
		var dial = 50L;
		var answer1 = 0;
		foreach (var rot in rotations)
		{
			dial = (100 + dial + rot) % 100;
			if (dial == 0)
				answer1++;
		}

		// part2
		dial = 50L;
		var answer2 = 0;
		foreach (var rot in rotations)
		{
			for (int r = 0, rot1 = rot > 0 ? 1 : -1; r < System.Math.Abs(rot); r++)
			{
				dial = (100 + dial + rot1) % 100;
				if (dial == 0)
					answer2++;
			}
		}

		return (answer1.ToString(), answer2.ToString());
	}
}