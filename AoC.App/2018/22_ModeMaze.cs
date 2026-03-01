using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;
using ErosionMap = System.Collections.Generic.Dictionary<Ujeby.Vectors.v2i, (long ErosionLevel, Ujeby.AoC.App._2018_22.Region RegionType)>;

namespace Ujeby.AoC.App._2018_22;

[AoCPuzzle(Year = 2018, Day = 22, Answer1 = "8681", Answer2 = "1070", Skip = true)]
public class ModeMaze : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var depth = input[0].ToNumArray()[0];

		var start = v2i.Zero;
		var target = new v2i(input[1].ToNumArray());
		var mapArea = new aab2i(start, target);

		// part1
		var erosionMap = new ErosionMap();
		foreach (var a in mapArea.EnumPoints())
			erosionMap.Get(a, target, depth, out _, out _);

		var answer1 = erosionMap.Sum(x => (int)x.Value.RegionType);

		// part2
		// TODO 2018/22 OPTIMIZE p2 ~22s
		var answer2 = ShortestPath(erosionMap, mapArea.Add(v2i.Zero, new v2i(38, 0)), start, target, depth);

		return (answer1.ToString(), answer2.ToString());
	}

	static int ShortestPath(ErosionMap erosionMap, aab2i mapArea, v2i start, v2i target, long depth)
	{
		var shortest = int.MaxValue;

		var visited = new Dictionary<(v2i, Tool), int>();
		
		var queue = new Queue<(v2i p, int timeTaken, Tool tool)>();
		queue.Enqueue((start, 0, Tool.Torch));

		while (queue.Count > 0)
		{
			var (p, timeTaken, tool) = queue.Dequeue();

			if (timeTaken >= shortest)
				continue;

			if (p == target)
			{
				shortest = timeTaken;
				continue;
			}

			if (visited.TryGetValue((p, tool), out int bestTime) && bestTime <= timeTaken)
				continue;

			visited[(p, tool)] = timeTaken;

			erosionMap.Get(p, target, depth, out _, out Region pRegion);
			foreach (var tool2 in _tools[pRegion])
			{
				var timeTaken2 = (tool2 == tool) ? timeTaken + 1 : timeTaken + 8;

				foreach (var p2 in v2i.UpDownLeftRight.Select(x => x + p))
				{
					if (p2 == target && tool2 != Tool.Torch)
						continue;

					if (!mapArea.Contains(p2) || (visited.TryGetValue((p2, tool2), out int shortestAtVisited) && shortestAtVisited <= timeTaken2))
						continue;

					erosionMap.Get(p2, target, depth, out _, out Region p2Region);
					if (!_regions[tool2].Contains(p2Region))
						continue;

					queue.Enqueue((p2, timeTaken2, tool2));
				}
			}
		}

		return shortest;
	}

	readonly static Dictionary<Region, Tool[]> _tools = new()
	{
		{ Region.Rocky, [Tool.Torch, Tool.ClimbingGear] },
		{ Region.Wet, [Tool.ClimbingGear, Tool.Neither] },
		{ Region.Narrow, [Tool.Torch, Tool.Neither] }
	};

	readonly static Dictionary<Tool, Region[]> _regions = new()
	{
		{ Tool.Torch, [Region.Rocky, Region.Narrow] },
		{ Tool.ClimbingGear, [Region.Rocky, Region.Wet] },
		{ Tool.Neither, [Region.Wet, Region.Narrow] }
	};
}

enum Region : int
{
	Rocky = 0,
	Wet = 1,
	Narrow = 2
}

enum Tool : int
{
	Neither = 0,
	Torch,
	ClimbingGear
}

static class Extensions
{
	public static void Get(this ErosionMap erosionMap, v2i a, v2i target, long depth, out long erosionLevel, out Region region)
	{
		if (erosionMap.TryGetValue(a, out (long ErosionLevel, Region Region) value))
		{
			erosionLevel = value.ErosionLevel;
			region = value.Region;
			return;
		}

		long geoIdx;
		if (a == v2i.Zero || a == target)
			geoIdx = 0;
		else if (a.Y == 0)
			geoIdx = a.X * 16807;
		else if (a.X == 0)
			geoIdx = a.Y * 48271;
		else
		{
			long left;
			if (erosionMap.TryGetValue(a + v2i.Left, out (long ErosionLevel, Region Region) leftValue))
				left = leftValue.ErosionLevel;
			else
				erosionMap.Get(a + v2i.Left, target, depth, out left, out _);

			long up;
			if (erosionMap.TryGetValue(a + v2i.Down, out (long ErosionLevel, Region Region) upValue))
				up = upValue.ErosionLevel;
			else
				erosionMap.Get(a + v2i.Down, target, depth, out up, out _);

			geoIdx = left * up;
		}

		erosionLevel = (geoIdx + depth) % 20183;
		region = (Region)((depth + erosionLevel) % 3);

		erosionMap.Add(a, (erosionLevel, region));
	}
}