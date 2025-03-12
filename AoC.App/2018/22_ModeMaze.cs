using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;
using ErosionMap = System.Collections.Generic.Dictionary<Ujeby.Vectors.v2i, (long erosionLevel, int regionType)>;

namespace Ujeby.AoC.App._2018_22;

[AoCPuzzle(Year = 2018, Day = 22, Answer1 = "8681", Answer2 = null, Skip = false)]
public class ModeMaze : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var depth = input[0].ToNumArray()[0];
		var target = new v2i(input[1].ToNumArray());
		var area = new aab2i(v2i.Zero, target);

		// part1
		var erosionLevels = new ErosionMap();
		foreach (var a in area.EnumPoints())
			erosionLevels.GetOrAdd(a, target, depth);
		//erosionLevels.GetOrAdd(target + v2i.Down, target, depth);
		//erosionLevels.GetOrAdd(target + v2i.Left, target, depth);

		var answer1 = erosionLevels.Sum(x => x.Value.regionType);

		// part2
		//erosionLevels.Print();

		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}

static class Extensions
{
	public static void Print(this ErosionMap erosionMap)
	{
#if DEBUG
		static char GetRegionType(long t) => t switch
		{
			0 => '.',
			1 => '=',
			2 => '|',
			_ => '\0',
		};
	
		var area = aab2i.FromPoints(erosionMap.Keys);

		foreach (var line in erosionMap.GroupBy(x => x.Key.Y).OrderBy(x => x.Key))
			Debug.Line(string.Join("", line.OrderBy(x => x.Key.X).Select(x => GetRegionType(x.Value.regionType))));

		Debug.Line();
#endif
	}

	public static long GetOrAdd(this ErosionMap erosionMap, v2i a, v2i target, long depth)
	{
		if (erosionMap.TryGetValue(a, out (long erosionLevel, int regionType) value))
			return value.erosionLevel;

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
			if (erosionMap.TryGetValue(a + v2i.Left, out (long erosionLevel, int regionType) leftValue))
				left = leftValue.erosionLevel;
			else
				left = erosionMap.GetOrAdd(a + v2i.Left, target, depth);

			long up;
			if (erosionMap.TryGetValue(a + v2i.Down, out (long erosionLevel, int regionType) upValue))
				up = upValue.erosionLevel;
			else
				up = erosionMap.GetOrAdd(a + v2i.Down, target, depth);

			geoIdx = left * up;
		}

		var erosionLevel = (geoIdx + depth) % 20183;
		erosionMap.Add(a, (erosionLevel, (int)(depth + erosionLevel) % 3));

		return erosionLevel;
	}
}