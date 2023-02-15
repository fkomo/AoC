using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2022_01
{
	[AoCPuzzle(Year = 2022, Day = 01, Answer1 = "66306", Answer2 = "195292")]
	internal class CalorieCounting : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
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
