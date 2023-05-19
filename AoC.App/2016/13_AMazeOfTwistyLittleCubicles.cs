using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_13;

[AoCPuzzle(Year = 2016, Day = 13, Answer1 = "96", Answer2 = null, Skip = false)]
public class AMazeOfTwistyLittleCubicles : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var officeDesignerFavNum = long.Parse(input.Single());

		// part1
		var start = new v2i(1, 1);
#if _DEBUG_SAMPLE
		var destination = new v2i(7, 4);
#else
		var destination = new v2i(31, 39);
#endif
		var answer1 = StepsToDestination(start, destination, officeDesignerFavNum, new v2i[] { start });

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	private static bool WallAt(v2i p, long officeDesignerFavNum)
		=> p.X < 0 || p.Y < 0 || Tools.Numbers.DecToBase(officeDesignerFavNum + p.X * p.X + (3 * p.X) + (2 * p.X * p.Y) + p.Y + (p.Y * p.Y))
			.Count(x => x == '1') % 2 != 0;

	private static readonly Dictionary<v2i, bool> _cache = new();

	private static bool WallAtCached(v2i p, long officeDesignerFavNum)
	{
		if (_cache.TryGetValue(p, out bool wall))
			return wall;

		wall = WallAt(p, officeDesignerFavNum);
		_cache.Add(p, wall);
		return wall;
	}

	private static readonly v2i[] _dir = new v2i[] { new v2i(-1, 0), new v2i(0, -1), new v2i(1, 0), new v2i(0, 1) };

	private static long StepsToDestination(v2i p, v2i dest, long officeDesignerFavNum, v2i[] path,
		long maxSteps = long.MaxValue)
	{
		if (p == dest)
			return path.Length - 1;

		// gone too far
		if (path.Length >= maxSteps)
			return long.MaxValue;

		foreach (var d in _dir)
		{
			var p1 = p + d;
			if (path.Contains(p1) || WallAtCached(p1, officeDesignerFavNum))
				continue;

			var s = StepsToDestination(p1, dest, officeDesignerFavNum, new v2i[] { p1 }.Concat(path).ToArray(), maxSteps);
			maxSteps = Math.Min(s, maxSteps);
		}

		return maxSteps;
	}
}