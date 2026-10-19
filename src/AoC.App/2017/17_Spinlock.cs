using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2017_17;

[AoCPuzzle(Year = 2017, Day = 17, Answer1 = "136", Answer2 = "1080289", Skip = false)]
public class Spinlock : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var step = int.Parse(input[0]);
		var position = 0;

		// part1
		var buffer = new LinkedList<int>();
		var current = buffer.AddFirst(0);
		for (var i = 1; i <= 2017; i++)
		{
			position = (position + step) % i + 1;
			for (var s = 0; s < step; s++)
				current = current.Next ?? buffer.First;

			current = buffer.AddAfter(current, i);
		}
		var answer1 = current.Next?.Value ?? buffer.First.Value;

		// part2
		var answer2 = int.MinValue;
		for (var i = 2018; i <= 50_000_000; i++)
		{
			position = (position + step) % i + 1;
			if (position == 1)
				answer2 = i;
		}

		return (answer1.ToString(), answer2.ToString());
	}
}