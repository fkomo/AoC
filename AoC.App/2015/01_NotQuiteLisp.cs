using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_01
{
	[AoCPuzzle(Year = 2015, Day = 01, Answer1 = "280", Answer2 = "1797")]
	public class NotQuiteLisp : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var instructions = input.Single();

			// part1
			var answer1 = (instructions.Count(i => i == '(') - instructions.Count(i => i == ')')).ToString();

			// part2
			string answer2 = null;
			for (int floor = 0, i = 0; i < instructions.Length; i++)
			{
				floor += instructions[i] == '(' ? 1 : -1;
				if (floor != -1)
					continue;

				answer2 = (i + 1).ToString();
				break;
			}

			return (answer1, answer2);
		}
	}
}
