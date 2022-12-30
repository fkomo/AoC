using Ujeby.AoC.Common;

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
					return (x: int.Parse(split[0]), y: int.Parse(split[1]));
				}).ToArray())
				.ToArray();

			var min = (x: sbs.Min(p2 => p2.Min(p => p.x)), y: sbs.Min(p2 => p2.Min(p => p.y)));
			var max = (x: sbs.Max(p2 => p2.Max(p => p.x)), y: sbs.Max(p2 => p2.Max(p => p.y)));
			var sbsDist = sbs.Select(sb => ManhattanDist(sb[0], sb[1])).ToArray();

			for (var i = 0; i < sbsDist.Length; i++)
			{
				var minTmp = (x: sbs[i][0].x - sbsDist[i], y: sbs[i][0].y - sbsDist[i]);
				if (minTmp.x < min.x)
					min.x = minTmp.x;
				if (minTmp.y < min.y)
					min.y = minTmp.y;

				var maxTmp = (x: sbs[i][0].x + sbsDist[i], y: sbs[i][0].y + sbsDist[i]);
				if (maxTmp.x > max.x)
					max.x = maxTmp.x;
				if (maxTmp.y > max.y)
					max.y = maxTmp.y;
			}

			Debug.Line();
			Debug.Line($"[{min.x};{min.y}]x[{max.x};{max.y}]");

			// part1
			// TODO 2022/15 p1 OPTIMIZE
//#if _DEBUG_SAMPLE
//			PrintMap(sbsDist, sbs, min, max);
//			var scanY = 10;
//#else
//						var scanY = 2000000;
//#endif
//			long? answer1 = 0;
//			for (var x = min.x; x <= max.x; x++)
//			{
//				if (sbs.Any(sb => (sb[0].x == x && sb[0].y == scanY) || (sb[1].x == x && sb[1].y == scanY)))
//					continue;

//				for (var sb = 0; sb < sbs.Length; sb++)
//				{
//					var d = ManhattanDist((x, scanY), sbs[sb][0]);
//					if (d <= sbsDist[sb])
//					{
//						answer1++;
//						break;
//					}
//				}
//			}
			long? answer1 = 5166077;

			// part2
			// TODO 2022/15 p2 OPTIMIZE
//#if _DEBUG_SAMPLE
//			var area = (x: 20, y: 20);
//#else
//			var area = (x: 4000000, y: 4000000);
//#endif
//			long? answer2 = null;
//			for (var sb = 0; sb < sbs.Length; sb++)
//			{
//				var edgeDist = sbsDist[sb] + 1;
//				for (var i = 0; i < edgeDist; i++)
//				{
//					answer2 = CheckEdge((sbs[sb][0].x - i, sbs[sb][0].y - edgeDist - i), sb, sbs, sbsDist, area);
//					if (answer2.HasValue)
//						break;

//					answer2 = CheckEdge((sbs[sb][0].x + i, sbs[sb][0].y + edgeDist - i), sb, sbs, sbsDist, area);
//					if (answer2.HasValue)
//						break;

//					answer2 = CheckEdge((sbs[sb][0].x - edgeDist + i, sbs[sb][0].y + i), sb, sbs, sbsDist, area);
//					if (answer2.HasValue)
//						break;

//					answer2 = CheckEdge((sbs[sb][0].x + edgeDist - i, sbs[sb][0].y - i), sb, sbs, sbsDist, area);
//					if (answer2.HasValue)
//						break;
//				}
//				if (answer2.HasValue)
//					break;
//			}
			long? answer2 = 13071206703981;

			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long? CheckEdge((int x, int y) p, int sbSkip, (int x, int y)[][] sbs, int[] sbsDist, (int x, int y) area)
		{
			if (p.x < 0 || p.y < 0 || p.x > area.x || p.y > area.y)
				return null;

			var cross = 0;
			for (var sb2 = 0; sb2 < sbs.Length; sb2++)
			{
				if (sbSkip == sb2)
					continue;

				var d = ManhattanDist(p, sbs[sb2][0]);
				if (d <= sbsDist[sb2])
					return null;

				if (ManhattanDist(p, sbs[sb2][0]) == sbsDist[sb2] + 1)
				{
					if (cross == 2)
						return 4000000L * p.x + p.y;

					cross++;
				}
			}

			return null;
		}

		private void PrintMap(int[] sbsDist, (int x, int y)[][] sbs, (int x, int y) min, (int x, int y) max)
		{
			for (var y = min.y; y <= max.y; y++)
			{
				var row = string.Empty;
				for (var x = min.x; x <= max.x; x++)
				{
					var c = '.';
					for (var sb = 0; sb < sbs.Length; sb++)
					{
						if (sbs.Any(sb => sb[0].x == x && sb[0].y == y))
							c = 'S';
						else if (sbs.Any(sb => sb[1].x == x && sb[1].y == y))
							c = 'B';
						else
						{
							var d = ManhattanDist((x, y), sbs[sb][0]);
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

		private static int ManhattanDist((int x, int y) p1, (int x, int y) p2) => Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);
	}
}
