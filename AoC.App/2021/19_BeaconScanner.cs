using Ujeby.AoC.Common;
using Ujeby.Tools.ArrayExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2021_19
{
	[AoCPuzzle(Year = 2021, Day = 19, Answer1 = "483", Answer2 = "14804", Skip = true)]
	public class BeaconScanner : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			Debug.Line();

			var scanners = input.Split(string.Empty)
				.Select(scanner =>
					AllRotations(scanner.Skip(1).Select(p => new v3i(p.Split(',').Select(pc => long.Parse(pc)).ToArray())).ToArray())
						.ToArray())
				.ToArray();

			Debug.Line($"{scanners.Length} scanners");

			// TODO 2021/19 OPTIMIZE (26s)

			// scannerIdx, scannerPosition, scannerBeacons
			var fixedScanners = new Dictionary<int, (v3i Position, v3i[] Beacons)>()
			{
				{ 0, (Position: new(0, 0, 0), Beacons: scanners[0][0]) },
			};
			Debug.Line($"fixed {0:d2}:{0:d2}");

			var fixedBeacons = new List<v3i>();
			fixedBeacons.AddRange(fixedScanners.First().Value.Beacons);

			while (fixedScanners.Count != scanners.Length)
			{
				var fixedBefore = fixedScanners.Count;
				for (var is2 = 1; is2 < scanners.Length; is2++)
				{
					if (fixedScanners.ContainsKey(is2))
						continue;

					for (var irs2 = 0; irs2 < scanners[is2].Length; irs2++)
					{
						foreach (var fs in fixedScanners)
						{
							if (!Overlapps(fs.Value.Beacons, scanners[is2][irs2], out v3i s2Offset, out v3i[] s2fs))
								continue;

							Debug.Line($"fixed {is2:d2}:{irs2:d2} with {fs.Key:d2}, offset={s2Offset}");
							fixedScanners.Add(is2, (Position: s2Offset, Beacons: s2fs));
							fixedBeacons.AddRange(s2fs);

							irs2 = scanners[is2].Length;
							break;
						}
					}
				}

				Debug.Line($"fixed {fixedScanners.Count}/{scanners.Length}");
			}

			// part1
			long? answer1 = fixedBeacons.Distinct().Count();

			// part2
			long? answer2 = long.MinValue;
			foreach (var s1 in fixedScanners)
				foreach (var s2 in fixedScanners)
				{
					if (s1.Key == s2.Key)
						continue;

					var md = v3i.ManhDistance(s1.Value.Position, s2.Value.Position);
					if (md > answer2)
						answer2 = md;
				}

			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static bool Overlapps(v3i[] pa1, v3i[] pa2, out v3i offset, out v3i[] pa2pa1)
		{
			offset = new();
			pa2pa1 = null;

			for (var ip1 = 0; ip1 < pa1.Length; ip1++)
				for (var ip2 = 0; ip2 < pa2.Length; ip2++)
				{
					var pa2Offset = pa2[ip2] - pa1[ip1];
					pa2pa1 = pa2.Select(p => p - pa2Offset).ToArray();

					if (pa1.Intersect(pa2pa1).Count() >= 12)
					{
						offset = pa2Offset;
						return true;
					}
				}

			return false;
		}

		private static IEnumerable<v3i[]> AllRotations(v3i[] points)
		{
			var rp = points.ToArray();
			yield return rp;

			for (var i = 0; i < 3; i++)
			{
				rp = rp.Select(p => p.RotateCWZ()).ToArray();
				yield return rp;
			}

			rp = points.Select(p => p.RotateCWY()).ToArray();
			yield return rp;

			for (var i = 0; i < 3; i++)
			{
				rp = rp.Select(p => p.RotateCWX()).ToArray();
				yield return rp;
			}

			rp = points.Select(p => p.RotateCWY().RotateCWY()).ToArray();
			yield return rp;

			for (var i = 0; i < 3; i++)
			{
				rp = rp.Select(p => p.RotateCWZ()).ToArray();
				yield return rp;
			}

			rp = points.Select(p => p.RotateCCWY()).ToArray();
			yield return rp;

			for (var i = 0; i < 3; i++)
			{
				rp = rp.Select(p => p.RotateCWX()).ToArray();
				yield return rp;
			}

			rp = points.Select(p => p.RotateCWX()).ToArray();
			yield return rp;

			for (var i = 0; i < 3; i++)
			{
				rp = rp.Select(p => p.RotateCWY()).ToArray();
				yield return rp;
			}

			rp = points.Select(p => p.RotateCCWX()).ToArray();
			yield return rp;

			for (var i = 0; i < 3; i++)
			{
				rp = rp.Select(p => p.RotateCWY()).ToArray();
				yield return rp;
			}
		}
	}
}
