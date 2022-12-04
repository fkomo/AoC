using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day01
{
	internal class CalorieCounting : ProblemBase
	{
		protected override (long?, long?) SolveProblem(string[] input)
		{
			// part1
			long result1 = string.Join('|', input)
				.Split("||")
				.Select(e => e.Split('|').Sum(c => int.Parse(c)))
				.Max();

			// part2
			long result2 = string.Join('|', input)
				.Split("||")
				.Select(e => e.Split('|').Sum(c => int.Parse(c)))
				.OrderByDescending(c => c)
				.Take(3)
				.Sum();

			return (result1, result2);
		}
	}
}
