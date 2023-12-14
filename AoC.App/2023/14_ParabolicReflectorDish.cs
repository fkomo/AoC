using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2023_14;

[AoCPuzzle(Year = 2023, Day = 14, Answer1 = "109098", Answer2 = null, Skip = false)]
public class ParabolicReflectorDish : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		long answer1 = 0;
		var stack = Enumerable.Repeat(-1, input[0].Length).ToArray();
		for (var i = 0; i < input.Length; i++)
		{
			for (var x = 0; x < input[i].Length; x++)
			{
				if (input[i][x] == '#')
					stack[x] = i;

				else if (input[i][x] == 'O')
				{
					stack[x]++;
					answer1 += input.Length - stack[x];
				}
			}
		}

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}