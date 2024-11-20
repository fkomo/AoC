using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_03;

[AoCPuzzle(Year = 2018, Day = 03, Answer1 = "115348", Answer2 = "188", Skip = false)]
public class NoMatterHowYouSliceIt : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var rects = input
			.Select(x => x.ToNumArray())
			.Select(x => new AABox2i(new(x[1], x[2]), new(x[1] + x[3] - 1, x[2] + x[4] - 1)))
			.ToArray();

		// part1
		var overlaps = new HashSet<v2i>();
		for (var r1 = 0; r1 < rects.Length; r1++)
			for (var r2 = 0; r2 < r1; r2++)
			{
				if (!rects[r1].Overlaps(rects[r2], out AABox2i r1r2))
					continue;

				foreach (var p in r1r2.Points())
					overlaps.Add(p);
			}
		var answer1 = overlaps.Count;

		// part2
		int? answer2 = null;
		for (var r1 = 0; r1 < rects.Length && !answer2.HasValue; r1++)
		{
			answer2 = r1 + 1;
			for (var r2 = 0; r2 < rects.Length; r2++)
			{
				if (r1 == r2)
					continue;

				if (rects[r1].Overlaps(rects[r2], out _))
				{
					answer2 = null;
					break;
				}
			}
		}

		return (answer1.ToString(), answer2.ToString());
	}
}