using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day02
{
	internal class RockPaperScissors : ProblemBase
	{
		protected override (long?, long?) SolveProblem(string[] input)
		{
			// part1
			long result1 = input
				.Sum(g => new[,] { { 3, 6, 0 }, { 0, 3, 6 }, { 6, 0, 3 } }[g[0] - 'A', g[2] - 'X'] + g[2] - 'X' + 1);

			// part2
			long result2 = input
				.Sum(g => new[,] { { 2, 0, 1 }, { 0, 1, 2 }, { 1, 2, 0 } }[g[0] - 'A', g[2] - 'X'] + 1 + (g[2] - 'X') * 3);

			return (result1, result2);
		}
	}
}
