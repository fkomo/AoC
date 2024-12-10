using Ujeby.AoC.Common;
using Ujeby.Grid.CharMapExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_10;

[AoCPuzzle(Year = 2024, Day = 10, Answer1 = "617", Answer2 = "1477", Skip = false)]
public class HoofIt : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToArray())
			.ToArray();
		
		var starts = map.EnumAll('0')
			.ToArray();

		// part1
		var answer1 = starts.Sum(x => map.Hike(x, []).Length);

		// part2
		var answer2 = starts.Sum(x => map.HikeDistinct([x], []).Count);

		return (answer1.ToString(), answer2.ToString());
	}
}

static class Extensions
{
	public static v2i[] Hike(this char[][] map, v2i pos, v2i[] reached9s)
	{
		var tile = map[pos.Y][pos.X];
		var bounds = new aab2i(v2i.Zero, new v2i(map.Length - 1));

		foreach (var dir in v2i.UpDownLeftRight)
		{
			var pos2 = pos + dir;
			if (!bounds.Contains(pos2))
				continue;

			var tile2 = map[pos2.Y][pos2.X];
			if (tile2 - tile != 1)
				continue;

			if (tile2 == '9' && !reached9s.Contains(pos2))
				reached9s = [.. reached9s, .. new v2i[] { pos2 }];
			else
				reached9s = map.Hike(pos2, reached9s);
		}

		return reached9s;
	}

	public static HashSet<v2i[]> HikeDistinct(this char[][] map, v2i[] trail, HashSet<v2i[]> trails)
	{
		var pos = trail[^1];
		var tile = map[pos.Y][pos.X];
		var bounds = new aab2i(v2i.Zero, new v2i(map.Length - 1));

		foreach (var dir in v2i.UpDownLeftRight)
		{
			var pos2 = pos + dir;
			if (!bounds.Contains(pos2))
				continue;

			var tile2 = map[pos2.Y][pos2.X];
			if (tile2 - tile != 1)
				continue;

			if (tile2 == '9')
				trails.Add([.. trail, .. new v2i[] { pos2 }]);
			else
				trails = map.HikeDistinct([.. trail, .. new v2i[] { pos2 }], trails);
		}

		return trails;
	}
}