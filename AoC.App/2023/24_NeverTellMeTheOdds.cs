using Microsoft.Z3;
using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_24;

[AoCPuzzle(Year = 2023, Day = 24, Answer1 = "11246", Answer2 = "716599937560103", Skip = true)]
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
		var collisionArea = new aab2i(new v2i(7), new v2i(27));
#else
		var collisionArea = new aab2i(new v2i(200000000000000), new v2i(400000000000000));
#endif

		// normalize velocities to directions
		var directions = hailstones
			.Select(x => x.Velocity.ToV2f().Normalize())
			.ToArray();

		// part1
		var answer1 = 0L;
		for (var i1 = 0; i1 < hailstones.Length; i1++)
			for (var i2 = i1 + 1; i2 < hailstones.Length; i2++)
				if (IntersectsIn(hailstones[i1].Position, directions[i1], hailstones[i2].Position, directions[i2], collisionArea))
					answer1++;

		// part2
		var answer2 = SolveWithZ3(hailstones);

		// TODO 2023/24 OPTIMZE p2 (25min)

		return (answer1.ToString(), answer2.ToString());
	}

	static long SolveWithZ3((v3i Position, v3i Velocity)[] hailstones)
	{
		var ctx = new Context();
		var solver = ctx.MkSolver();

		// rock position
		var rpx = ctx.MkIntConst("rpx");
		var rpy = ctx.MkIntConst("rpy");
		var rpz = ctx.MkIntConst("rpz");

		// rock velocity
		var rvx = ctx.MkIntConst("rvx");
		var rvy = ctx.MkIntConst("rvy");
		var rvz = ctx.MkIntConst("rvz");

		// For each iteration, we will add 3 new equations and one new condition to the solver.
		// We want to find 9 variables (x, y, z, vx, vy, vz, t0, t1, t2) that satisfy all the equations, so a system of 9 equations is enough.
		for (var i = 0; i < 3; i++)
		{
			var t = ctx.MkIntConst($"t{i}"); // time for the rock to reach the hail
			var hail = hailstones[i];

			var hpx = ctx.MkInt(hail.Position.X);
			var hpy = ctx.MkInt(hail.Position.Y);
			var hpz = ctx.MkInt(hail.Position.Z);

			var hpvx = ctx.MkInt(hail.Velocity.X);
			var hpvy = ctx.MkInt(hail.Velocity.Y);
			var hpvz = ctx.MkInt(hail.Velocity.Z);

			// rock path: rp + ti * rv
			var xLeft = ctx.MkAdd(rpx, ctx.MkMul(t, rvx));
			var yLeft = ctx.MkAdd(rpy, ctx.MkMul(t, rvy));
			var zLeft = ctx.MkAdd(rpz, ctx.MkMul(t, rvz));

			// hailstone path: hp + ti * hv
			var xRight = ctx.MkAdd(hpx, ctx.MkMul(t, hpvx));
			var yRight = ctx.MkAdd(hpy, ctx.MkMul(t, hpvy));
			var zRight = ctx.MkAdd(hpz, ctx.MkMul(t, hpvz));

			// rp + ti * rv = hp + t * hv
			solver.Add(t >= 0);
			solver.Add(ctx.MkEq(xLeft, xRight));
			solver.Add(ctx.MkEq(yLeft, yRight));
			solver.Add(ctx.MkEq(zLeft, zRight));
		}

		solver.Check();
		var model = solver.Model;

		return
			long.Parse(model.Eval(rpx).ToString()) +
			long.Parse(model.Eval(rpy).ToString()) +
			long.Parse(model.Eval(rpz).ToString());
	}

	static bool IntersectsIn(v3i p1, v2f d1, v3i p2, v2f d2, aab2i collisionArea)
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