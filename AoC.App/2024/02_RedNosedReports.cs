using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2024_02;

[AoCPuzzle(Year = 2024, Day = 02, Answer1 = "502", Answer2 = "544", Skip = false)]
public class RedNosedReports : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var reports = input.Select(x => x.ToNumArray()).ToArray();

		// part1
		var safe = reports.Where(IsSafe).ToArray();
		var answer1 = safe.Length;

		// part2
		var answer2 = safe.Length + reports.Except(safe)
			.Count(x => Enumerable.Range(0, x.Length).Any(ie => IsSafe(x.ToArrayExceptAt(ie))));

		return (answer1.ToString(), answer2.ToString());
	}

	static bool IsSafe(long[] levels)
	{
		bool? asc = null;
		for (var i = 1; i < levels.Length; i++)
		{
			var diff = System.Math.Abs(levels[i - 1] - levels[i]);
			if (diff < 1 || diff > 3)
				return false;

			if (asc == null)
				asc = levels[i] > levels[i - 1];

			else if ((asc.Value && levels[i] < levels[i - 1]) || (!asc.Value && levels[i] > levels[i - 1]))
				return false;
		}

		return true;
	}
}