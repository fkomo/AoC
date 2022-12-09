using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day01
{
	internal class CalorieCounting : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			// part1
			long? answer1 = string.Join('|', input)
				.Split("||")
				.Select(e => e.Split('|').Sum(c => int.Parse(c)))
				.Max();

			// part2
			long answer2 = string.Join('|', input)
				.Split("||")
				.Select(e => e.Split('|').Sum(c => int.Parse(c)))
				.OrderByDescending(c => c)
				.Take(3)
				.Sum();

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
