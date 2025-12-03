using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2025_02;

[AoCPuzzle(Year = 2025, Day = 02, Answer1 = "55916882972", Answer2 = "76169125915", Skip = false)]
public class GiftShop : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var idRanges = input[0]
			.Split(',')
			.Select(x => new v2i([.. x.ToNumArray().Select(x => System.Math.Max(x, 11))])) // ignore single digit numbers and 10 (valid ids)
			.ToArray();

		// part1
		static bool IsInvalidSimple(string id)
		{
			if (id.Length % 2 != 0)
				return false;

			var half = id.Length / 2;
			for (var i = 0; i < half; i++)
				if (id[i] != id[half + i])
					return false;

			return true;
		}

		var answer1 = 0L;
		for (var r = 0; r < idRanges.Length; r++)
		{
			var range = idRanges[r];
			for (var id = range.X; id <= range.Y; id++)
				if (IsInvalidSimple(id.ToString()))
					answer1 += id;
		}

		// part2
		static bool IsInvalidMultiPattern(string id)
		{
			if (IsInvalidSimple(id)) 
				return true;

			// pattern len = 1
			if (id.All(x => x == id[0]))
				return true;

			var maxRepeatLen = id.Length / 2 - 1;

			// pattern len = 2-maxRepeatLen
			for (var patternLen = maxRepeatLen; patternLen > 1; patternLen--)
			{
				if (id.Length % patternLen != 0)
					continue;

				var invalid = true;
				for (var p1 = 0; p1 < patternLen && invalid; p1++)
					for (var p2 = patternLen + p1; p2 < id.Length && invalid; p2 += patternLen)
						if (id[p1] != id[p2])
							invalid = false;

				if (invalid)
					return true;
			}

			return false;
		}

		var answer2 = 0L;
		for (var r = 0; r < idRanges.Length; r++)
		{
			var range = idRanges[r];
			for (var id = range.X; id <= range.Y; id++)
			{
				if (IsInvalidMultiPattern(id.ToString()))
					answer2 += id;
			}
		}

		return (answer1.ToString(), answer2.ToString());
	}
}