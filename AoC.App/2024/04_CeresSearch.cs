using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_04;

[AoCPuzzle(Year = 2024, Day = 04, Answer1 = "2662", Answer2 = "2034", Skip = false)]
public class CeresSearch : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var answer1 = new aab2i(v2i.Zero, new v2i(input.Length - 1))
			.EnumPoints()
			.Sum(input.XMASCountAt);

		// part2
		var answer2 = new aab2i(new v2i(1, 1), new v2i(input.Length - 2))
			.EnumPoints()
			.Count(input.X_MASAt);

		return (answer1.ToString(), answer2.ToString());
	}
}

static class Extensions
{
	public static int XMASCountAt(this string[] s, v2i p)
	{
		var count = 0;
		if (s[p.Y][(int)p.X] != 'X')
			return count;

		var pattern = "MAS";

		bool CheckMAS(v2i p, v2i dir)
		{
			for (var i = 1; i <= pattern.Length; i++)
				if (s[p.Y + dir.Y * i][(int)(p.X + dir.X * i)] != pattern[i - 1])
					return false;

			return true;
		}

		var x = (int)p.X;
		var y = (int)p.Y;
		var row = s[y];

		count += ((x + pattern.Length) < row.Length && CheckMAS(p, new v2i(1, 0))) ? 1 : 0;
		count += ((x - pattern.Length) >= 0 && CheckMAS(p, new v2i(-1, 0))) ? 1 : 0;
		count += ((y + pattern.Length) < s.Length && CheckMAS(p, new v2i(0, 1))) ? 1 : 0;
		count += ((y - pattern.Length) >= 0 && CheckMAS(p, new v2i(0, -1))) ? 1 : 0;
		count += (x + pattern.Length < row.Length && y + pattern.Length < s.Length && CheckMAS(p, new v2i(1, 1))) ? 1 : 0;
		count += (x - pattern.Length >= 0 && y - pattern.Length >= 0 && CheckMAS(p, new v2i(-1, -1))) ? 1 : 0;
		count += (x + pattern.Length < row.Length && y - pattern.Length >= 0 && CheckMAS(p, new v2i(1, -1))) ? 1 : 0;
		count += (x - pattern.Length >= 0 && y + pattern.Length < s.Length && CheckMAS(p, new v2i(-1, 1))) ? 1 : 0;

		return count;
	}

	public static bool X_MASAt(this string[] s, v2i p)
	{
		var x = (int)p.X;
		var y = (int)p.Y;
		return (s[y][x] == 'A') &&
			(
				(s[y + 1][x + 1] == 'S' && s[y - 1][x + 1] == 'S' && s[y - 1][x - 1] == 'M' && s[y + 1][x - 1] == 'M') ||
				(s[y + 1][x + 1] == 'M' && s[y - 1][x + 1] == 'M' && s[y - 1][x - 1] == 'S' && s[y + 1][x - 1] == 'S') ||
				(s[y + 1][x + 1] == 'S' && s[y - 1][x + 1] == 'M' && s[y - 1][x - 1] == 'M' && s[y + 1][x - 1] == 'S') ||
				(s[y + 1][x + 1] == 'M' && s[y - 1][x + 1] == 'S' && s[y - 1][x - 1] == 'S' && s[y + 1][x - 1] == 'M')
			);
	}
}