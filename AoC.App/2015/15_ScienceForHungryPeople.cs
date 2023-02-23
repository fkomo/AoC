using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2015_15
{
	[AoCPuzzle(Year = 2015, Day = 15, Answer1 = "222870", Answer2 = "117936")]
	public class ScienceForHungryPeople : PuzzleBase
	{
		private const int _teaspoons = 100;
		private static int[] _idx;

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var ingredients = input.Select(i => i.ToNumArray()).ToArray();

			_idx = Enumerable.Range(0, ingredients.Length).ToArray();

			// part1 & part2
#if _RELEASE
			var (answer1, answer2) = CheckRecipeReleaseOnly(ingredients);
#else
			var (answer1, answer2) = CheckRecipeRec(ingredients, new int[ingredients.Length]);
#endif
			return (answer1.ToString(), answer2.ToString());
		}

		/// <summary>
		/// general solution, bit slow for release
		/// </summary>
		/// <param name="ingredients"></param>
		/// <param name="idx"></param>
		/// <param name="i"></param>
		/// <returns></returns>
		private static (long, long) CheckRecipeRec(long[][] ingredients, int[] idx,
			int i = 0)
		{
			long score1 = 0L, score2 = 0L;

			var props = ingredients[0].Length - 1;
			for (var x = 0; x < _teaspoons; x++)
			{
				idx[i] = x;

				if (i + 1 == idx.Length)
				{
					if (idx.Sum() != 100)
						continue;

					var score = 1L;
					for (var p = 0; p < props; p++)
					{
						score *= Math.Max(0, _idx.Sum(x => idx[x] * ingredients[x][p]));
						if (score == 0)
							break;
					}

					score1 = Math.Max(score, score1);
					if (_idx.Sum(x => idx[x] * ingredients[x][props]) == 500)
						score2 = Math.Max(score, score2);
				}
				else
				{
					var (s1, s2) = CheckRecipeRec(ingredients, idx, i: i + 1);
					score1 = Math.Max(s1, score1);
					score2 = Math.Max(s2, score2);
				}
			}

			return (score1, score2);
		}

		/// <summary>
		/// fast, 4 ingredients only
		/// </summary>
		/// <param name="ingredients"></param>
		/// <returns></returns>
		private static (long, long) CheckRecipeReleaseOnly(long[][] ingredients)
		{
			var answer1 = 0L;
			var answer2 = 0L;

			for (var i1 = 0; i1 < _teaspoons; i1++)
			{
				for (var i2 = 0; i2 < _teaspoons; i2++)
				{
					for (var i3 = 0; i3 < _teaspoons; i3++)
					{
						for (var i4 = 0; i4 < _teaspoons; i4++)
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

			return (answer1, answer2);
		}
	}
}
