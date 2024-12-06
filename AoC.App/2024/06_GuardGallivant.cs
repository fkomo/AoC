using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_06;

[AoCPuzzle(Year = 2024, Day = 06, Answer1 = "5331", Answer2 = null, Skip = false)]
public class GuardGallivant : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToCharArray()).ToArray();
		
		map.IndexOf('^', out v2i guard);
		map[guard.Y][(int)guard.X] = '.'; 

		// part1
		var gDir = v2i.Down;
		var hs = new HashSet<v2i>();
		while (true)
		{
			hs.Add(guard);

			var next = guard + gDir;
			if (next.X < 0 || next.Y < 0 || next.Y == map.Length || next.X == map[next.Y].Length)
				break;

			var m = map[next.Y][(int)next.X];
			if (m == '.')
				guard = next;

			else if (m == '#')
				gDir = gDir.RotateCCW();
		}

		var answer1 = hs.Count;

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}

static class Extensions
{
	internal static bool IndexOf(this char[][] map, char c, out v2i result)
	{
		result = v2i.Zero;

		var y = 0;
		for (; y < map.Length; y++)
			if (map[y].Contains(c))
				break;

		if (y == map.Length)
			return false;

		var x = Array.IndexOf(map[y], c);
		if (x == -1)
			return false;

		result = new v2i(x, y);
		return true;
	}
}