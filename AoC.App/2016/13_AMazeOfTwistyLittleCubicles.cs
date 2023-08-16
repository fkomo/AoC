using Ujeby.Alg;
using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_13;

[AoCPuzzle(Year = 2016, Day = 13, Answer1 = "96", Answer2 = "141", Skip = false)]
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
		var maxSteps = 50;
		var bfs = new BreadthFirstSearch(null, start + new v2i(maxSteps), start, 
			(p1, p2, map) => !WallAtCached(p1, officeDesignerFavNum));
		bfs.StepFull();
		var answer2 = bfs.VisitedHashSet.Where(p => bfs.Path(p).Length <= maxSteps).Count();

		return (answer1.ToString(), answer2.ToString());
	}

	public static bool WallAt(v2i p, long officeDesignerFavNum)
		=> p.X < 0 || p.Y < 0 || Tools.Numbers.DecToBase(officeDesignerFavNum + p.X * p.X + (3 * p.X) + (2 * p.X * p.Y) + p.Y + (p.Y * p.Y))
			.Count(x => x == '1') % 2 != 0;

	private static readonly Dictionary<v2i, bool> _cache = new();

	public static bool WallAtCached(v2i p, long officeDesignerFavNum)
	{
		if (_cache.TryGetValue(p, out bool wall))
			return wall;

		wall = WallAt(p, officeDesignerFavNum);
		_cache.Add(p, wall);
		return wall;
	}

	private static long StepsToDestination(v2i p, v2i dest, long officeDesignerFavNum, v2i[] path,
		long maxSteps = long.MaxValue)
	{
		if (p == dest)
			return path.Length - 1;

		// gone too far
		if (path.Length >= maxSteps)
			return long.MaxValue;

		foreach (var d in v2i.RightDownLeftUp)
		{
			var p1 = p + d;
			if (path.Contains(p1) || WallAtCached(p1, officeDesignerFavNum))
				continue;

			var s = StepsToDestination(p1, dest, officeDesignerFavNum, new v2i[] { p1 }.Concat(path).ToArray(), maxSteps);
			maxSteps = Math.Min(s, maxSteps);
		}

		return maxSteps;
	}

	public static void Reset()
	{
		_cache.Clear();
	}

	public static v2i[] Path(v2i p, v2i dest, long officeDesignerFavNum, v2i[] path,
		long maxSteps = long.MaxValue)
	{
		if (p == dest)
			return path;

		// gone too far
		if (path.Length >= maxSteps)
			return null;

		var bestPath = null as v2i[];
		foreach (var d in v2i.RightDownLeftUp)
		{
			var p1 = p + d;
			if (path.Contains(p1) || WallAtCached(p1, officeDesignerFavNum))
				continue;

			var tmp = Path(p1, dest, officeDesignerFavNum, path.Concat(new v2i[] { p1 }).ToArray(), maxSteps);
			if (tmp?.Length < maxSteps)
			{
				bestPath = tmp.ToArray();
				maxSteps = tmp.Length;
			}
		}

		return bestPath;
	}
}