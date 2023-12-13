using Ujeby.AoC.Common;
using Ujeby.Graphics.Entities;
using Ujeby.Tools.ArrayExtensions;

namespace Ujeby.AoC.App._2023_13;

[AoCPuzzle(Year = 2023, Day = 13, Answer1 = "36015", Answer2 = null, Skip = false)]
public class PointOfIncidence : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// parse patterns into binary representation of rows/columns
		var patterns = input
			.Split(string.Empty)
			.Select(p =>
			{
				var rows = p.Select(r => Math.BaseToDec(r, ".#"))
					.ToArray();
				
				var columns = Enumerable.Range(0, p[0].Length)
					.Select(x => Math.BaseToDec(new string(p.Select(r => r[x]).ToArray()), ".#"))
					.ToArray();

				return (Rows: rows, Columns: columns);
			})
			.ToArray();
#if DEBUG
		Debug.Line($"{patterns.Length} patterns{Environment.NewLine}");
		foreach (var p in patterns)
		{
			Debug.Line($"{string.Join(",", p.Rows)}");
			Debug.Line($"{string.Join(",", p.Columns)}");
			Debug.Line();
		}
#endif
		// part1
		var mirrors = patterns
			.Select(p => (Vertical: Mirror(p.Columns), Horizontal: Mirror(p.Rows)))
			.ToArray();
		var answer1 = mirrors.Sum(m => m.Vertical + 100* m.Horizontal);

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static int Mirror(long[] pattern)
	{
		for (var m = 0; m < pattern.Length - 1; m++)
		{
			if (pattern[m] == pattern[m + 1])
			{
				var mirrored = true;
				for (var i = 1; i < System.Math.Min(m + 1, pattern.Length - m - 1); i++)
				{
					if (pattern[m - i] != pattern[m + 1 + i])
					{
						mirrored = false;
						break;
					}
				}
				if (mirrored)
					return m + 1;
			}
		}

		return 0;
	}
}