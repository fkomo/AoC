using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2023_15;

[AoCPuzzle(Year = 2023, Day = 15, Answer1 = "506869", Answer2 = null, Skip = false)]
public class LensLibrary : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var answer1 = input.Single().Split(',').Sum(x => HASH(x));

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static byte HASH(string s)
	{
		var hash = 0;
		foreach (var c in s)
		{
			hash += c;
			hash *= 17;
			hash %= 256;
		}

		return (byte)hash;
	}
}