using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day02
{
	internal class RockPaperScissors : PuzzleBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			// part1
			long answer1 = input
				.Sum(g => new[,] { { 3, 6, 0 }, { 0, 3, 6 }, { 6, 0, 3 } }[g[0] - 'A', g[2] - 'X'] + g[2] - 'X' + 1);

			// part2
			long answer2 = input
				.Sum(g => new[,] { { 2, 0, 1 }, { 0, 1, 2 }, { 1, 2, 0 } }[g[0] - 'A', g[2] - 'X'] + 1 + (g[2] - 'X') * 3);

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
