using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_20;

[AoCPuzzle(Year = 2024, Day = 20, Answer1 = "1351", Answer2 = null, Skip = false)]
public class RaceCondition : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToArray()).ToArray();
		Debug.Line($"map size {map.ToAAB2i()}");

		// part1
		var metaMap = MetaMap(map, out v2i[] path);
		var answer1 = All2Cheats(metaMap, path).Length;

		// part2
		metaMap = MetaMap(map, out path);
		var answer2 = All20Cheats(metaMap, path, minSavedTime: 50);
		// TODO 2024/20 p2
		// 2388319 too high
		// 1199127 too high
		// 1062908 too high

		return (answer1.ToString(), answer2.ToString());
	}

	public static int[][] MetaMap(char[][] map, out v2i[] path)
	{
		map.Find('S', out var start);
		map.Find('E', out var end);

		var hs = new HashSet<v2i>();

		var p = start;
		while (p != end)
		{
			hs.Add(p);
			p = v2i.UpDownLeftRight.Select(x => p + x).Single(x => map.Get(x) != '#' && !hs.Contains(x));
		}
		hs.Add(end);

		path = [.. hs];

		var meta = map.Select(x => x.Select(x => _wall).ToArray()).ToArray();
		for (var i = 0; i < path.Length; i++)
			meta.Set(path[i], i);

		return meta;
	}

	const int _wall = -1;
	const int _usedWall = -2;

	public static v2i[] All2Cheats(int[][] metaMap, v2i[] path, int minSavedTime = 100)
	{
		var cheats = new List<v2i>();
		var mapArea = metaMap.ToAAB2i();

		for (var i = 0; i < path.Length - 1; i++)
		{
			var dir = path[i + 1] - path[i];
			foreach (var cheat in new v2i[] { dir.RotateCCW(), dir.RotateCW(), dir.Inv() })
			{
				var wall = path[i] + cheat;
				if (metaMap.Get(wall) >= 0)
					continue;

				metaMap.Set(wall, _usedWall);

				if (!mapArea.Contains(wall + cheat))
					continue;

				var shortcut = metaMap.Get(wall + cheat);
				if (shortcut > i)
				{
					var saved = shortcut - i - 2;
					if (saved >= minSavedTime)
						cheats.Add(wall);
				}
			}
		}

		return [.. cheats];
	}

	public static int All20Cheats(int[][] metaMap, v2i[] path, int minSavedTime = 100)
	{
		var mapArea = metaMap.ToAAB2i();
		var offsets20 = new aab2i(new v2i(-20), new v2i(20)).EnumPoints().Where(x => x.ManhLength() <= 20).Except([v2i.Zero]).ToArray();

		var cheats = new HashSet<(int, int)>();

		var shortcuts = new Dictionary<int, int>();

		for (var p1 = 0; p1 < path.Length - 1; p1++)
		{
			foreach (var offset in offsets20)
			{
				var path2 = path[p1] + offset;
				if (!mapArea.Contains(path2))
					continue;

				var p2 = metaMap.Get(path2);
				if (p2 > p1)
				{
					var saved = p2 - p1 - (int)offset.ManhLength();

					shortcuts.TryAdd(saved, 0);
					shortcuts[saved]++;

					if (saved >= minSavedTime)
						cheats.Add((p1, p2));
				}
			}
		}

		return cheats.Count;
	}
}