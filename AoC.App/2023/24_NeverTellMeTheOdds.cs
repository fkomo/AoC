using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_24;

[AoCPuzzle(Year = 2023, Day = 24, Answer1 = "11246", Answer2 = null, Skip = false)]
public class NeverTellMeTheOdds : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var hailstones = input
			.Select(x => (
				Position: new v3i(x.ToNumArray().Take(3).ToArray()),
				Velocity: new v3i(x.ToNumArray().Skip(3).ToArray())))
			.ToArray();
		Debug.Line($"{hailstones.Length} hailstones");

#if DEBUG
		var collisionArea = new AABox2i(new v2i(7), new v2i(27));
#else
		var collisionArea = new AABox2i(new v2i(200000000000000), new v2i(400000000000000));
#endif

		// normalize velocities to directions
		var directions = hailstones
			.Select(x => x.Velocity.ToV2f().Normalize())
			.ToArray();

		// part1
		long answer1 = 0;
		for (var i1 = 0; i1 < hailstones.Length; i1++)
			for (var i2 = i1 + 1; i2 < hailstones.Length; i2++)
				if (IntersectsIn(hailstones[i1].Position, directions[i1], hailstones[i2].Position, directions[i2], collisionArea))
					answer1++;

		// part2
		string answer2 = null;
		// TODO 2023/24 p2

		return (answer1.ToString(), answer2?.ToString());
	}

	static bool IntersectsIn(v3i p1, v2f d1, v3i p2, v2f d2, AABox2i collisionArea)
	{
		var u = (p1.Y * d2.X + d2.Y * p2.X - p2.Y * d2.X - d2.Y * p1.X) / (d1.X * d2.Y - d1.Y * d2.X);
		var v = (p1.X + d1.X * u - p2.X) / d2.X;

		if (u > 0 && v > 0)
		{
			var p = p1.ToV2f() + d1 * u;
			if (collisionArea.Min.X <= p.X && collisionArea.Max.X >= p.X && collisionArea.Min.Y <= p.Y && collisionArea.Max.Y >= p.Y)
			{
				Debug.Line($"collision! {p1}->{d1} X {p2}-{d2}: u={u} v={v}");
				return true;
			}
		}

		Debug.Line($"{p1}->{d1} X {p2}-{d2}: u={u} v={v}");
		return false;
	}
}