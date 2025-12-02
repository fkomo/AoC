using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2025_02;

[AoCPuzzle(Year = 2025, Day = 02, Answer1 = "55916882972", Answer2 = null, Skip = false)]
public class GiftShop : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var ranges = input[0]
			.Split(',')
			.Select(x => new v2i(x.ToNumArray()))
			.ToArray();

		// part1
		long answer1 = 0L;
		foreach (var range in ranges)
		{
			for (var n = range.X; n <= range.Y; n++)
			{
				var nStr = n.ToString();
				if (nStr.Length % 2 != 0)
					continue;

				var digits = nStr.Select(x => x - '0').ToArray();

				static bool IsInvalid(int[] digits)
				{
					for (var i = 0; i < digits.Length / 2; i++)
						if (digits[i] != digits[digits.Length / 2 + i])
							return false;

					return true;
				}

				if (!IsInvalid(digits))
					continue;

				Debug.Line(n.ToString());
				answer1 += n;
			}
		}

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}