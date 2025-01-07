using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_20;

[AoCPuzzle(Year = 2024, Day = 20, Answer1 = "1351", Answer2 = "966130", Skip = false)]
public class RaceCondition : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToArray()).ToArray();
		Debug.Line($"map size {map.ToAAB2i()}");

		// part1
		var metaMap = MetaMap(map, out v2i[] path);
		var answer1 = AllCheats(metaMap, path, 2, minSavedTime: 100).Length;

		Debug.Line();

		// part2
		metaMap = MetaMap(map, out path);
		var answer2 = AllCheats(metaMap, path, 20, minSavedTime: 100).Length;

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

	public static (v2i, v2i)[] AllCheats(int[][] metaMap, v2i[] path, int maxCheat, int minSavedTime = 100)
	{
		var mapArea = metaMap.ToAAB2i();
		var offsets = new aab2i(new v2i(-maxCheat), new v2i(maxCheat))
			.EnumPoints()
			.Where(x => x.ManhLength() >= 2 && x.ManhLength() <= maxCheat)
			.Except([v2i.Zero])
			.ToArray();

		var cheats = new List<(v2i, v2i)>();

		for (var p1 = 0; p1 < path.Length - minSavedTime; p1++)
		{
			var path1 = path[p1];
			foreach (var path2 in offsets.Select(x => path1 + x)
				.Where(x => mapArea.Contains(x) && metaMap.Get(x) > p1))
			{
				var p2 = metaMap.Get(path2);
				var saved = p2 - p1 - (int)v2i.ManhDistance(path1, path2);

				if (saved >= minSavedTime)
					cheats.Add((path1, path2));
			}
		}

		return [.. cheats];
	}
}