using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_04;

[AoCPuzzle(Year = 2024, Day = 04, Answer1 = "2662", Answer2 = "2034", Skip = false)]
public class CeresSearch : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var extraLine = new string(Enumerable.Repeat('.', input.Length + 2).ToArray());
		string[] inputExtra = [extraLine, .. input.Select(x => "." + x + ".").ToArray(), extraLine];

		// part1
		var answer1 = new aab2i(new v2i(1), new v2i(extraLine.Length - 2))
			.EnumPoints()
			.Sum(inputExtra.XMASCountAt);

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
		bool CheckMAS(v2i pX, v2i dMAS)
		{
			var pattern = "MAS";
			for (var i = 1; i <= pattern.Length; i++)
				if (s[pX.Y + dMAS.Y * i][(int)(pX.X + dMAS.X * i)] != pattern[i - 1])
					return false;

			return true;
		}

		return (s[p.Y][(int)p.X] != 'X') ? 0 : v2i.PlusMinusOne.Count(x => CheckMAS(p, x));
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