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

	public static v2i[] All2Cheats(int[][] metaMap, v2i[] path, int minSavedTime = 100)
	{
		var cheats = new List<v2i>();

		//var shortcuts = new Dictionary<int, int>();

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

					//shortcuts.TryAdd(saved, 0);
					//shortcuts[saved]++;

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

		for (var i = 0; i < path.Length - 1; i++)
		{
			foreach (var cheat in offsets20)
			{
				var outOfWall = path[i] + cheat;
				if (!mapArea.Contains(outOfWall))
					continue;

				var shortcut = metaMap.Get(outOfWall);
				if (shortcut > i)
				{
					var saved = shortcut - i - (int)cheat.ManhLength();

					shortcuts.TryAdd(saved, 0);
					shortcuts[saved]++;

					if (saved >= minSavedTime)
						cheats.Add((i, shortcut));
				}
			}
		}

		return cheats.Count;
	}

	const int _wall = -1;
	const int _usedWall = -2;
}