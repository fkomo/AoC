using System.Collections.Concurrent;
using Ujeby.AoC.Common;
using Ujeby.Tools;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2021.Day19
{
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

			// part1
			var matchedScanners = new ConcurrentDictionary<string, (int s1, int s2, int rs1, int rs2, int bc)>();
			//Parallel.For(0, scanners.Length, (is1) =>
			for (var is1 = 0; is1 < scanners.Length; is1++)
			{
				var match = false;
				for (var is2 = is1 + 1; is2 < scanners.Length && !match; is2++)
				{
					var pairKey = GetPairKey(is1, is2);
					if (matchedScanners.ContainsKey(pairKey))
						continue;

					for (var irs1 = 0; irs1 < scanners[is1].Length && !matchedScanners.ContainsKey(pairKey); irs1++)
						for (var irs2 = 0; irs2 < scanners[is2].Length && !matchedScanners.ContainsKey(pairKey); irs2++)
						{
							var overlapping = FindOverlapping(scanners[is1][irs1], scanners[is2][irs2]);
							if (overlapping != null)
								matchedScanners.TryAdd(pairKey, (is1, is2, irs1, irs2, overlapping.Length));
						}
				}
			}
			//);

			foreach (var ms in matchedScanners)
				Log.Line($"#{ms.Key}: scanners[{ms.Value.s1}/{ms.Value.s2}] with rotation[{ms.Value.rs1}/{ms.Value.rs2}] overlaps {ms.Value.bc} beacons");

			long? answer1 = null;

			// part2
			long? answer2 = null;

			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static string GetPairKey(int is1, int is2) => is1 < is2 ? $"{is1}{is2}" : $"{is2}{is1}";

		private static v3i[] FindOverlapping(v3i[] pa1, v3i[] pa2)
		{
			for (var ip1 = 0; ip1 < pa1.Length; ip1++)
				for (var ip2 = 0; ip2 < pa2.Length; ip2++)
				{
					var offset = pa2[ip2] - pa1[ip1];
					var overlapping = pa1.Intersect(pa2.Select(p => p - offset));
					if (overlapping.Count() >= 12)
						return overlapping.ToArray();
				}

			return null;
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
