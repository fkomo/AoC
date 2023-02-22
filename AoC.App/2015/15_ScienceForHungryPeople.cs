using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2015_15
{
	[AoCPuzzle(Year = 2015, Day = 15, Answer1 = "222870", Answer2 = "117936")]
	public class ScienceForHungryPeople : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var teaspoons = 100;
			var ingredients = input.Select(/*i => i[..i.IndexOf(':')], */i => i.ToNumArray()).ToArray();

			// part1
			var answer1 = 0L;
			// part2
			var answer2 = 0L;

			for (var i1 = 0; i1 < teaspoons; i1++)
			{
				for (var i2 = 0; i2 < teaspoons; i2++)
				{
					for (var i3 = 0; i3 < teaspoons; i3++)
					{
						for (var i4 = 0; i4 < teaspoons; i4++)
						{
							if (i1 + i2 + i3 + i4 != 100)
								continue;

							var score = 1L;
							for (var i = 0; i < 4; i++)
							{
								score *= Math.Max(0,
									i1 * ingredients[0][i] +
									i2 * ingredients[1][i] +
									i3 * ingredients[2][i] +
									i4 * ingredients[3][i]);
								if (score == 0)
									break;
							}

							var cal = i1 * ingredients[0][4] + i2 * ingredients[1][4] + i3 * ingredients[2][4] + i4 * ingredients[3][4];
							if (cal == 500 && score > answer2)
								answer2 = score;

							else if (score > answer1)
								answer1 = score;
						}
					}
				}
			}

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
