using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_22;

[AoCPuzzle(Year = 2023, Day = 22, Answer1 = "499", Answer2 = null, Skip = false)]
public class SandSlabs : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var slabs = input
			.Select(x => x.ToNumArray())
			.Select(x => new AABox3i(new v3i[] { new v3i(x.Take(3).ToArray()), new v3i(x.Skip(3).ToArray()) }))
			.ToArray();
		Debug.Line($"{slabs.Length} slabs");

		// part1
		slabs = slabs.OrderBy(x => x.Min.Z).ToArray();
		var min = new v2i(slabs.Min(x => x.Min.X), slabs.Min(x => x.Min.Y));
		var max = new v2i(slabs.Max(x => x.Max.X), slabs.Max(x => x.Max.Y));
		var xySize = max - min + new v2i(1);
		Debug.Line($"snapshot size: {min}..{max} => {xySize}");

		// drop slabs
		var ground = new long[xySize.Y, xySize.X];
		for (var s = 0; s < slabs.Length; s++)
		{
			var slab = slabs[s];
			var drop = long.MinValue;
			for (var y = slab.Min.Y; y <= slab.Max.Y; y++)
				for (var x = slab.Min.X; x <= slab.Max.X; x++)
				{
					if (ground[y, x] > drop)
						drop = ground[y, x];
				}

			drop = drop + 1 - slab.Min.Z;
			slabs[s].Min.Z += drop;
			slabs[s].Max.Z += drop;

			for (var y = slab.Min.Y; y <= slab.Max.Y; y++)
				for (var x = slab.Min.X; x <= slab.Max.X; x++)
					ground[y, x] = slabs[s].Max.Z;
		}

		var bottomStack = slabs
			.GroupBy(x => x.Min.Z)
			.ToDictionary(x => x.Key, x => x.ToArray());

		var topStack = slabs
			.GroupBy(x => x.Max.Z)
			.ToDictionary(x => x.Key, x => x.ToArray());

		var answer1 = slabs.Count(s =>
			// no slabs above
			!bottomStack.ContainsKey(s.Max.Z + 1) ||
			// there are some slabs above but not directly
			bottomStack[s.Max.Z + 1].All(bs => !XYIntersection(s, bs)) || 
			// there are some slabs directly above, but also there are other slabs supporting them
			bottomStack[s.Max.Z + 1].All(bs => topStack[s.Max.Z].Except(new AABox3i[] { s }).Any(ts => XYIntersection(bs, ts))));

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static bool XYIntersection(AABox3i aab1, AABox3i aab2)
	{
		if (aab1.Max.X < aab2.Min.X || aab1.Min.X > aab2.Max.X)
			return false;

		if (aab1.Max.Y < aab2.Min.Y || aab1.Min.Y > aab2.Max.Y)
			return false;

		return true;
	}
}