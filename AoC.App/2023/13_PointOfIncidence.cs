using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_13;

[AoCPuzzle(Year = 2023, Day = 13, Answer1 = "36015", Answer2 = "35335", Skip = false)]
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
		var reflections = patterns
			.Select(p => new v2i(Reflection(p.Columns), Reflection(p.Rows)))
			.ToArray();
		var answer1 = reflections.Sum(m => m.X + 100 * m.Y);

		// part2
		long answer2 = 0;
		for (var i = 0; i < patterns.Length; i++)
		{
			var pattern = patterns[i];
			var oldReflection = reflections[i];

			var newReflectionFound = false;
			for (var y = 0; y < pattern.Rows.Length; y++)
			{
				for (var x = 0; x < pattern.Columns.Length; x++)
				{
					var newRows = pattern.Rows.ToArray();
					var newColumns = pattern.Columns.ToArray();

					// turn mirror at [x;y]
					newRows[y] ^= (long)System.Math.Pow(2, pattern.Columns.Length - 1 - x);
					newColumns[x] ^= (long)System.Math.Pow(2, pattern.Rows.Length - 1 - y);

					var newReflection = new v2i(Reflection(newColumns, (int)oldReflection.X), Reflection(newRows, (int)oldReflection.Y));
					if (newReflection != v2i.Zero)
					{
						answer2 += newReflection.X + newReflection.Y * 100;
						newReflectionFound = true;
						break;
					}
				}

				if (newReflectionFound)
					break;
			}
		}

		return (answer1.ToString(), answer2.ToString());
	}

	static int Reflection(long[] pattern, 
		int skip = 0)
	{
		for (var m = 0; m < pattern.Length - 1; m++)
		{
			if (m + 1 == skip)
				continue;

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