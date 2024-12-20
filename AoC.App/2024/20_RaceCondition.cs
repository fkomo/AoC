using Ujeby.AoC.Common;
using Ujeby.Grid.CharMapExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_20;

[AoCPuzzle(Year = 2024, Day = 20, Answer1 = "1351", Answer2 = null, Skip = false)]
public class RaceCondition : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToArray()).ToArray();
		Debug.Line($"map size {map.ToAAB2i()}");

		map.Find('S', out var start);
		map.Find('E', out var end);

		var walls = map.EnumAll('#').ToArray();
		Debug.Line($"{walls.Length} walls");

		var empty = new v2i[] { start, end }.Concat(map.EnumAll('.')).ToArray();

		// part1
		var path = GetPath(empty, start, end);
		var answer1 = AllCheats(path, walls).Length;

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	public static v2i[] GetPath(v2i[] empty, v2i start, v2i end)
	{
		var hs = new HashSet<v2i>();

		var p = start;
		while (p != end)
		{
			hs.Add(p);
			p = v2i.UpDownLeftRight.Select(x => p + x).Single(x => empty.Contains(x) && !hs.Contains(x));
		}
		hs.Add(end);

		return [.. hs];
	}

	public static v2i[] AllCheats(v2i[] path, v2i[] walls, int minSavedPs = 100)
	{
		var cheats = new HashSet<v2i>();
		var checkedWalls = new HashSet<v2i>();
		//var shortcuts = new Dictionary<int, int>();

		for (var i = 0; i < path.Length - 1; i++)
		{
			var dir = path[i + 1] - path[i];
			foreach (var cheat in new v2i[] { dir.RotateCCW(), dir.RotateCW(), dir.Inv() })
			{
				var s = path[i] + cheat;
				if (walls.Contains(s) && checkedWalls.Add(s) && path.Contains(s + cheat))
				{
					var shortcut = Array.IndexOf(path, s + cheat);
					if (shortcut > i && shortcut - i > 0)
					{
						var saved = shortcut - i - 2;

						//shortcuts.TryAdd(saved, 0);
						//shortcuts[saved]++;

						if (saved >= minSavedPs)
							cheats.Add(s);
					}
				}
			}
		}

		return [.. cheats];
	}
}