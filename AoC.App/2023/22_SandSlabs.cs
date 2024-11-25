using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_22;

[AoCPuzzle(Year = 2023, Day = 22, Answer1 = "499", Answer2 = "95059", Skip = false)]
public class SandSlabs : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var bricks = input
			.Select(x => x.ToNumArray())
			.Select(x => new aab3i(new v3i[] { new v3i(x.Take(3).ToArray()), new v3i(x.Skip(3).ToArray()) }))
			.ToArray();
		Debug.Line($"{bricks.Length} bricks");

		bricks = bricks.OrderBy(x => x.Min.Z).ToArray();
		var groundSize =
			new v2i(bricks.Max(x => x.Max.X), bricks.Max(x => x.Max.Y)) -
			new v2i(bricks.Min(x => x.Min.X), bricks.Min(x => x.Min.Y)) + new v2i(1);

		// part1
		bricks = DropBricks(bricks, groundSize, out _);

		var bottomStack = bricks
			.GroupBy(x => x.Min.Z)
			.ToDictionary(x => x.Key, x => x.ToArray());

		var topStack = bricks
			.GroupBy(x => x.Max.Z)
			.ToDictionary(x => x.Key, x => x.ToArray());

		var answer1 = bricks.Count(s =>
			// no bricks above
			!bottomStack.ContainsKey(s.Max.Z + 1) ||
			// there are some bricks above but not directly
			bottomStack[s.Max.Z + 1].All(bs => !XYIntersection(s, bs)) ||
			// there are some bricks directly above, but also there are other bricks supporting them
			bottomStack[s.Max.Z + 1].All(bs => topStack[s.Max.Z].Except(new aab3i[] { s }).Any(ts => XYIntersection(bs, ts))));

		// part2
		var answer2 = bricks.Sum(s => DroppedBricks(bricks.Where(x => x != s).ToArray(), groundSize));

		return (answer1.ToString(), answer2.ToString());
	}

	static int DroppedBricks(aab3i[] bricks, v2i groundSize)
	{
		DropBricks(bricks, groundSize, out int dropped);
		return dropped;
	}

	static aab3i[] DropBricks(aab3i[] bricks, v2i groundSize, out int dropped)
	{
		dropped = 0;

		var bottom = new long[groundSize.Y, groundSize.X];
		for (var s = 0; s < bricks.Length; s++)
		{
			var brick = bricks[s];
			var drop = long.MinValue;
			for (var y = brick.Min.Y; y <= brick.Max.Y; y++)
				for (var x = brick.Min.X; x <= brick.Max.X; x++)
				{
					if (bottom[y, x] > drop)
						drop = bottom[y, x];
				}

			drop = drop + 1 - brick.Min.Z;
			if (drop != 0)
			{
				dropped++;
				bricks[s].Min.Z += drop;
				bricks[s].Max.Z += drop;
			}

			for (var y = brick.Min.Y; y <= brick.Max.Y; y++)
				for (var x = brick.Min.X; x <= brick.Max.X; x++)
					bottom[y, x] = bricks[s].Max.Z;
		}

		return bricks;
	}

	static bool XYIntersection(aab3i aab1, aab3i aab2)
		=> !(aab1.Max.X < aab2.Min.X || aab1.Min.X > aab2.Max.X || aab1.Max.Y < aab2.Min.Y || aab1.Min.Y > aab2.Max.Y);
}