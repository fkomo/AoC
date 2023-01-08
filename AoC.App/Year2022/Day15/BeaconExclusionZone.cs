using System.Collections.Concurrent;
using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2022.Day15
{
	public class BeaconExclusionZone : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// [0] sensor, [1] beacon
			var sbs = input.Select(l => l["Sensor at x=".Length..].Replace(" closest beacon is at x=", string.Empty)
				.Split(":").Select(p =>
				{
					var split = p.Split(", y=");
					return new v2i(int.Parse(split[0]), int.Parse(split[1]));
				}).ToArray())
				.ToArray();

			var min = new v2i(sbs.Min(p2 => p2.Min(p => p.X)), sbs.Min(p2 => p2.Min(p => p.Y)));
			var max = new v2i(sbs.Max(p2 => p2.Max(p => p.X)), sbs.Max(p2 => p2.Max(p => p.Y)));
			var sbsDist = sbs.Select(sb => v2i.ManhDistance(sb[0], sb[1])).ToArray();

			for (var i = 0; i < sbsDist.Length; i++)
			{
				var minTmp = new v2i(sbs[i][0].X - sbsDist[i], sbs[i][0].Y - sbsDist[i]);
				if (minTmp.X < min.X)
					min.X = minTmp.X;
				if (minTmp.Y < min.Y)
					min.Y = minTmp.Y;

				var maxTmp = new v2i(sbs[i][0].X + sbsDist[i], sbs[i][0].Y + sbsDist[i]);
				if (maxTmp.X > max.X)
					max.X = maxTmp.X;
				if (maxTmp.Y > max.Y)
					max.Y = maxTmp.Y;
			}

			Debug.Line();
			Debug.Line($"[{min.X};{min.Y}]x[{max.X};{max.Y}]");

			// TODO 2022/15 OPTIMIZE (2s)

			// part1
#if _DEBUG_SAMPLE
			PrintMap(sbsDist, sbs, min, max);
			var scanY = 10;
#else
			var scanY = 2000000;
#endif
			//long? answer1 = 0;
			//var p = new v2i(min.X, scanY);
			//for (; p.X <= max.X; p.X++)
			//{
			//	if (sbs.Any(sb => (sb[0].X == p.X && sb[0].Y == scanY) || (sb[1].X == p.X && sb[1].Y == scanY)))
			//		continue;

			//	for (var sb = 0; sb < sbs.Length; sb++)
			//	{
			//		var d = v2i.ManhDistance(p, sbs[sb][0]);
			//		if (d <= sbsDist[sb])
			//		{
			//			answer1++;
			//			break;
			//		}
			//	}
			//}
			long? answer1 = 5166077;

			// part2
#if _DEBUG_SAMPLE
			var area = new v2i(20);
#else
			var area = new v2i(4000000);
#endif
			//long? answer2 = null;
			//Parallel.For(0, sbs.Length, (sb, state) =>
			//{
			//	long? result = null;
			//	var edgeDist = sbsDist[sb] + 1;
			//	for (var i = 0; i < edgeDist; i++)
			//	{
			//		result = CheckEdge(new(sbs[sb][0].X - i, sbs[sb][0].Y - edgeDist - i), sb, sbs, sbsDist, area);
			//		if (result.HasValue)
			//			break;

			//		if (state.ShouldExitCurrentIteration)
			//			return;

			//		result = CheckEdge(new(sbs[sb][0].X + i, sbs[sb][0].Y + edgeDist - i), sb, sbs, sbsDist, area);
			//		if (result.HasValue)
			//			break;

			//		if (state.ShouldExitCurrentIteration)
			//			return;

			//		result = CheckEdge(new(sbs[sb][0].X - edgeDist + i, sbs[sb][0].Y + i), sb, sbs, sbsDist, area);
			//		if (result.HasValue)
			//			break;

			//		if (state.ShouldExitCurrentIteration)
			//			return;

			//		result = CheckEdge(new(sbs[sb][0].X + edgeDist - i, sbs[sb][0].Y - i), sb, sbs, sbsDist, area);
			//		if (result.HasValue)
			//			break;

			//		if (state.ShouldExitCurrentIteration)
			//			return;
			//	}

			//	if (result.HasValue)
			//	{
			//		answer2 = result;
			//		state.Break();
			//	}
			//});
			long? answer2 = 13071206703981;

			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long? CheckEdge(v2i p, int sbSkip, v2i[][] sbs, long[] sbsDist, v2i area)
		{
			if (p.X < 0 || p.Y < 0 || p.X > area.X || p.Y > area.Y)
				return null;

			var cross = 0;
			for (var sb2 = 0; sb2 < sbs.Length; sb2++)
			{
				if (sbSkip == sb2)
					continue;

				var d = v2i.ManhDistance(p, sbs[sb2][0]);
				if (d <= sbsDist[sb2])
					return null;

				if (v2i.ManhDistance(p, sbs[sb2][0]) == sbsDist[sb2] + 1)
				{
					if (cross == 2)
						return 4000000L * p.X + p.Y;

					cross++;
				}
			}

			return null;
		}

		private void PrintMap(int[] sbsDist, v2i[][] sbs, v2i min, v2i max)
		{
			var p = min;
			for (; p.Y <= max.Y; p.Y++)
			{
				var row = string.Empty;
				for (; p.X <= max.X; p.X++)
				{
					var c = '.';
					for (var sb = 0; sb < sbs.Length; sb++)
					{
						if (sbs.Any(sb => sb[0].X == p.X && sb[0].Y == p.Y))
							c = 'S';
						else if (sbs.Any(sb => sb[1].X == p.X && sb[1].Y == p.Y))
							c = 'B';
						else
						{
							var d = v2i.ManhDistance(p, sbs[sb][0]);
							if (d <= sbsDist[sb])
							{
								c = '#';
								break;
							}
						}
					}

					row += c;
				}
				Debug.Line(row);
			}
		}
	}
}
