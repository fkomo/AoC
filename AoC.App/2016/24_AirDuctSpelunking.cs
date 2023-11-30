using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_24;

[AoCPuzzle(Year = 2016, Day = 24, Answer1 = "448", Answer2 = "672", Skip = false)]
public class AirDuctSpelunking : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(y => y.ToArray()).ToArray();
		var mapSize = new v2i(map[0].Length, map.Length);

		var poi = CreatePoi(map, mapSize);

		CreatePaths(poi, map, mapSize, out v2i[,][] path, out int[,] dist);

		// path1
		var answer1 = ShortestPath(poi, dist, new int[] { 0 }).Value.Length;

		// part2
		var answer2 = ShortestPath(poi, dist, new int[] { 0 }, ret: true).Value.Length;

		return (answer1.ToString(), answer2.ToString());
	}

	public static Dictionary<int, v2i> CreatePoi(char[][] map, v2i mapSize)
	{
		var poi = new Dictionary<int, v2i>();
		for (var y = 0; y < mapSize.Y; y++)
			for (var x = 0; x < mapSize.X; x++)
				if (map[y][x] != '.' && map[y][x] != '#')
					poi.Add(map[y][x] - '0', new v2i(x, y));

		return poi;
	}

	public static (int[] Path, long Length)? ShortestPath(Dictionary<int, v2i> poi, int[,] dist, int[] path,
		long pathLength = 0, long shortest = long.MaxValue, bool ret = false)
	{
		if (pathLength > shortest)
			return null;

		if (!ret && path.Length == poi.Count || ret && path.Length == poi.Count + 1)
		{
			Debug.Line($"{pathLength}: {string.Join(", ", path)}");
			return (path, pathLength);
		}

		var keys = poi.Keys.ToArray();

		(int[] Path, long Length)? shortestLocal = (null as int[], long.MaxValue);
		for (var p1 = 0; p1 < keys.Length; p1++)
		{
			if (!ret && path.Contains(keys[p1]))
				continue;

			if (ret && keys[p1] != 0 && path.Contains(keys[p1]))
				continue;

			if (ret && keys[p1] == 0 && path.Length < keys.Length)
				continue;

			var tmp = ShortestPath(poi, dist, path.Concat(new int[] { keys[p1] }).ToArray(),
				pathLength + dist[path.Last(), keys[p1]], shortestLocal.Value.Length, ret: ret);
			if (tmp?.Length < shortestLocal?.Length)
				shortestLocal = tmp;
		}

		return shortestLocal;
	}

	public static void CreatePaths(Dictionary<int, v2i> poi, char[][] map, v2i mapSize,
		out v2i[,][] path, out int[,] dist)
	{
		var keys = poi.Keys.ToArray();
		dist = new int[keys.Length, keys.Length];
		path = new v2i[keys.Length, keys.Length][];
		for (var p1 = 0; p1 < poi.Count; p1++)
		{
			var bfs = new Ujeby.Alg.BreadthFirstSearch(null, mapSize, poi[keys[p1]],
				(to, from, m) => map[to.Y][to.X] != '#');
			bfs.StepFull();

			for (var p2 = p1 + 1; p2 < poi.Count; p2++)
			{
				var p = bfs.Path(poi[keys[p2]]);
				dist[keys[p1], keys[p2]] = dist[keys[p2], keys[p1]] = p.Length;
				path[keys[p1], keys[p2]] = path[keys[p2], keys[p1]] = p.ToArray();
			}
		}

		DebugDist(keys, dist);
	}

	private static void DebugDist(int[] keys, int[,] dist)
	{
		Debug.Text($"  | ");
		for (var x = 0; x < keys.Length; x++)
			Debug.Text($"{keys[x],4}", 0);
		Debug.Line();
		Debug.Text($"--+-");
		Debug.Line(new string(Enumerable.Repeat('-', keys.Length * 4).ToArray()), 0);

		for (var y = 0; y < keys.Length; y++)
		{
			Debug.Text($" {keys[y]}| ");
			for (var x = 0; x < keys.Length; x++)
				Debug.Text($"{dist[keys[y], keys[x]],4}", 0);

			Debug.Line();
		}
	}
}