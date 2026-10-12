using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2015_25
{
	[AoCPuzzle(Year = 2015, Day = 25, Answer1 = "2650453", Answer2 = "*")]
	public class LetItSnow : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var codePosition = new v2i(input.Single().ToNumArray().Reverse().ToArray()) - new v2i(1,1);

			// part1
			long? answer1 = null;
			var prevCode = 20151125L;
			for (var diag = 1L; !answer1.HasValue; diag++)
			{
				var d = new v2i(0, diag);
				for (var di = 0; di <= diag; di++, d += new v2i(1, -1))
				{
					prevCode = prevCode * 252533 % 33554393;
					if (d == codePosition)
					{
						answer1 = prevCode;
						break;
					}
				}
			}

			// part2
			var answer2 = "*";

			return (answer1?.ToString(), answer2);
		}
	}
}
