using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2015_12
{
	[AoCPuzzle(Year = 2015, Day = 12, Answer1 = "111754", Answer2 = null)]
	public class JSAbacusFrameworkio : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			string answer2 = null;

			// part1
			var answer1 = input.Single().ToNumArray().Sum();

			// part2


			return (answer1.ToString(), answer2);
		}
	}
}
