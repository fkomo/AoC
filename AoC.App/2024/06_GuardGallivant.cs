using Ujeby.AoC.Common;
using Ujeby.Vectors;
using Ujeby.Grid.CharMapExtensions;

namespace Ujeby.AoC.App._2024_06;

[AoCPuzzle(Year = 2024, Day = 06, Answer1 = "5331", Answer2 = "1812", Skip = false)]
public class GuardGallivant : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToCharArray()).ToArray();
		
		map.Find('^', out v2i start);
		map[start.Y][(int)start.X] = '.'; 

		// part1
		var dir = v2i.Down;
		var pos = start;
		var visited = new HashSet<v2i>();
		while (true)
		{
			visited.Add(pos);

			var nextPos = pos + dir;
			if (nextPos.X < 0 || nextPos.Y < 0 || nextPos.Y == map.Length || nextPos.X == map[nextPos.Y].Length)
				break;

			var m = map[nextPos.Y][(int)nextPos.X];
			if (m == '.')
				pos = nextPos;

			else if (m == '#')
				dir = dir.RotateCCW();
		}
		var answer1 = visited.Count;

		// part2
		// TODO 2024/06 OPTIMIZE p2 (1s)
		var answer2 = 0L;
		var possibleObastacles = visited.Skip(1).ToArray();
		Parallel.ForEach(possibleObastacles, (o) =>
		{
			if (map.IsLoop(start, v2i.Down, o))
				answer2++;
		});

		return (answer1.ToString(), answer2.ToString());
	}
}

static class Extensions
{
	internal static bool IsLoop(this char[][] map, v2i pos, v2i dir, v2i obstacle)
	{
		var hs = new HashSet<(v2i Pos, v2i Dir)>();
		while (hs.Add((pos, dir)))
		{
			var next = pos + dir;
			if (next.X < 0 || next.Y < 0 || next.Y == map.Length || next.X == map[next.Y].Length)
				return false;

			var m = map[next.Y][(int)next.X];
			if (m == '#' || next == obstacle)
				dir = dir.RotateCCW();

			else if (m == '.')
				pos = next;
		}

		return true;
	}
}