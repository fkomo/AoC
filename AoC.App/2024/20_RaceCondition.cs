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
		//var answer1 = All2Cheats(metaMap, path).Length;
		var answer1 = All2Cheats(metaMap, path, minSavedTime: 50).Length;

		// part2
		metaMap = MetaMap(map, out path);
		var answer2 = AllCheats(metaMap, path, 1, minSavedTime: 64).Count;
		//var answer2 = AllCheats(metaMap, path, 19, minSavedTime: 100).Count;
		// 285 - sample
		// TODO 2024/20 p2
		// 1062908 too high
		// 1048331 not right

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

	public static HashSet<(v2i, v2i)> AllCheats(int[][] metaMap, v2i[] path, 
		int maxCheat, int minSavedTime = 100)
	{
		var mapArea = metaMap.ToAAB2i();
		var offsets = new aab2i(new v2i(-maxCheat), new v2i(maxCheat))
			.EnumPoints()
			.Where(x => x.ManhLength() <= maxCheat)
			.Except([v2i.Zero])
			.ToArray();

		var cheats = new HashSet<(v2i, v2i)>();

		var shortcuts = new Dictionary<int, int>();

		for (var p1 = 0; p1 < path.Length - 1; p1++)
		{
			var path1 = path[p1];
			foreach (var wall1 in v2i.UpDownLeftRight.Select(x => x + path1)
				.Where(x => metaMap.Get(x) < 0))
			{
				foreach (var wall2 in offsets.Select(x => wall1 + x)
					.Where(x => mapArea.Contains(x) && metaMap.Get(x) < 0))
				{
					foreach (var path2 in v2i.UpDownLeftRight.Select(x => wall2 + x)
						.Where(x => mapArea.Contains(x) && metaMap.Get(x) == p1 + minSavedTime))
					{
						var saved = metaMap.Get(path2) - p1 - (int)v2i.ManhDistance(path1, path2);

						shortcuts.TryAdd(saved, 0);
						shortcuts[saved]++;

						cheats.Add((path1, path2));
					}
				}
			}
		}

		return cheats;
	}
}