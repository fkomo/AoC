using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_23;

[AoCPuzzle(Year = 2023, Day = 23, Answer1 = "2106", Answer2 = "6350", Skip = true)]
public class ALongWalk : PuzzleBase
{
	readonly static Dictionary<char, v2i> _slopes = new()
	{
		{ '>', v2i.Right },
		{ 'v', v2i.Up },
		{ '<', v2i.Left },
		{ '^', v2i.Down },
	};

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		Debug.Line($"map size: {input.Length}x{input.Length}");

		// part1
		var answer1 = LongestHike(input);

		// part2
		var poi = PointsOfInterest(input);
		Debug.Line($"{poi.Length} pois");

		var p2p = PoiToPoiMap(poi, input);
		var answer2 = LongestHike(p2p, new v2i[] { poi[0] }, poi[1]);
		// TODO 2023/23 OPTIMIZE p2 (40s)

		return (answer1.ToString(), answer2.ToString());
	}

	// TODO 2023/23 add cache
	readonly static Dictionary<string, long> _cache = new();

	static long LongestHike(Dictionary<v2i, Dictionary<v2i, long>> p2p, v2i[] path, v2i end,
		long pathSteps = 0)
	{
		//var cacheKey = (id, maskIdx, dmgGrpIdx);
		//if (_cache.ContainsKey(cacheKey))
		//	return _cache[cacheKey];

		if (path.Last() == end)
			return pathSteps;

		long maxSteps = 0;
		foreach (var pc in p2p[path.Last()])
		{
			if (path.Contains(pc.Key))
				continue;

			var steps = LongestHike(p2p, path.Concat(new v2i[] { pc.Key }).ToArray(), end, pathSteps + pc.Value);
			if (steps > maxSteps)
				maxSteps = steps;
		}

		//_cache.Add(cacheKey, maxSteps);

		return maxSteps;
	}

	static Dictionary<v2i, Dictionary<v2i, long>> PoiToPoiMap(v2i[] poi, string[] input)
	{
		var p2p = new Dictionary<v2i, Dictionary<v2i, long>>();

		foreach (var p in poi)
			p2p.Add(p, new Dictionary<v2i, long>());

		for (var poi1Idx = 0; poi1Idx < poi.Length; poi1Idx++)
		{
			var next = v2i.UpDownLeftRight
				.Where(d =>
				{
					var n = poi[poi1Idx] + d;

					if (n.Y < 0 || n.Y >= input.Length)
						return false;

					var t = input[n.Y][(int)n.X];
					return t == '.' || _slopes.ContainsKey(t);
				})
				.ToArray();

			for (var di = 0; di < next.Length; di++)
			{
				var d0 = next[di];
				var p0 = poi[poi1Idx] + d0;

				var steps = 1;
				while (!poi.Contains(p0))
				{
					steps++;
					var d1 = v2i.UpDownLeftRight
						.Single(d1 =>
						{
							var p1 = p0 + d1;
							var t = input[p1.Y][(int)p1.X];

							// forrest
							if (t == '#')
								return false;

							// cant go back
							if (d1 * -1 == d0)
								return false;

							// clear path ahead
							return true;
						});

					p0 += d1;
					d0 = d1;
				}

				var poi2Idx = Array.IndexOf(poi, p0);

				p2p[poi[poi1Idx]].TryAdd(poi[poi2Idx], steps);
				p2p[poi[poi2Idx]].TryAdd(poi[poi1Idx], steps);

				Debug.Line($"{poi[poi1Idx],10}->{poi[poi2Idx],-10}: {steps}");
			}
		}

		return p2p;
	}

	static v2i[] PointsOfInterest(string[] input)
	{
		var poi = new List<v2i>()
		{
			new v2i(1, 0),
			new v2i(input.Length - 2, input.Length - 1)
		};

		var poiNeighbours = new char[] { 'v', '>', '<', '^', '#' };
		for (var y = 1; y < input.Length - 1; y++)
			for (var x = 1; x < input.Length - 1; x++)
				if (input[y][x] == '.' && v2i.UpDownLeftRight.All(d => poiNeighbours.Contains(input[y + d.Y][x + (int)d.X])))
					poi.Add(new v2i(x, y));

		return poi.ToArray();
	}

	public static long LongestHike(string[] map,
		List<v2i[]> allPaths = null)
	{
		var start = new v2i(1, 0);
		var end = new v2i(map.Length - 2, map.Length - 1);

		var longest = long.MinValue;

		var paths = new Queue<(v2i[] Path, v2i NextDir)>();
		paths.Enqueue((new v2i[] { new v2i(1, 0) }, v2i.Up));
		while (paths.Count > 0)
		{
			var pq = paths.Dequeue();
			var steps = new List<v2i>(pq.Path);
			v2i? dir = pq.NextDir;

			while (dir.HasValue)
			{
				// add step
				var last = steps.Last() + dir.Value;
				steps.Add(last);

				// next steps
				var nextSteps = v2i.UpDownLeftRight
					.Where(d =>
					{
						var n = last + d;
						var t = map[n.Y][(int)n.X];

						// forrest
						if (t == '#')
							return false;

						// cant go against slope
						if (_slopes.ContainsKey(t) && d != _slopes[t])
							return false;

						// cant go back
						if (d * -1 == dir.Value)
							return false;

						// clear path ahead
						return true;
					})
					.ToArray();

				// nowhere to go
				if (nextSteps.Length == 0)
					break;

				dir = nextSteps[0];
				if (nextSteps.Length == 1 && last + dir.Value == end)
				{
					steps.Add(last + dir.Value);
					allPaths?.Add(steps.ToArray());

					var totalSteps = steps.Count - 1; // ignore start tile
					if (totalSteps > longest)
					{
						longest = totalSteps;
						Debug.Line($"{longest}");
					}

					break;
				}
				else
				{
					// split path
					foreach (var s in nextSteps.Skip(1))
						paths.Enqueue((steps.ToArray(), s));
				}
			}
		}

		return longest;
	}
}