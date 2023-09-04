using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2017_05;

[AoCPuzzle(Year = 2017, Day = 05, Answer1 = "374269", Answer2 = "27720699", Skip = false)]
public class AMazeOfTwistyTrampolinesAllAlike : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var i = 0L;
		var answer1 = 0L;
		var jumps = input.Select(x => long.Parse(x)).ToArray();
		while (i >= 0 && i < jumps.Length)
		{
			var j = jumps[i];
			
			jumps[i]++;
			
			i += j;
			answer1++;
		}

		// part2
		i = 0L;
		var answer2 = 0L;
		jumps = input.Select(x => long.Parse(x)).ToArray();
		while (i >= 0 && i < jumps.Length)
		{
			var j = jumps[i];
			
			if (j >= 3)
				jumps[i]--;
			else
				jumps[i]++;

			i += j;
			answer2++;
		}

		// 720 too low

		return (answer1.ToString(), answer2.ToString());
	}
}