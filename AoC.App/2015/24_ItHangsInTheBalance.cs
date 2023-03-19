using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_24
{
	[AoCPuzzle(Year = 2015, Day = 24, Answer1 = "11846773891", Answer2 = "80393059")]
	public class ItHangsInTheBalance : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var packages = input.Select(i => long.Parse(i)).ToArray();

			// part1
			var answer1 = DividePackages(packages, 3);

			// part2
			var answer2 = DividePackages(packages, 4);

			return (answer1.ToString(), answer2.ToString());
		}

		private static long DividePackages(long[] packages, int gCount)
		{
			var minQe = long.MaxValue;
			var minGroupLength = long.MaxValue;

			var weight = packages.Sum() / gCount;
			Debug.Line($"weight={weight}");

			for (var gLength = 1; gLength < packages.Length - (gCount - 1); gLength++)
			{
				if (gLength > minGroupLength)
					break;

				foreach (var g in Alg.Combinatorics.Combinations(packages, gLength)
					.Where(g => g.Sum() == weight))
				{
					var gRest = packages.Except(g);
					if (!IsDivisibleToGroups(Array.Empty<long>(), gRest.ToArray(), weight, gCount - 1))
						continue;

					var qe = QuantumEntanglement(g.ToArray());
					if (qe < minQe)
					{
						minQe = qe;
						minGroupLength = gLength;
						Debug.Line($"g1:[{string.Join(", ", g)}] qe={qe} ... gRest:[{string.Join(", ", gRest)}]");

						return minQe;
					}
				}
			}

			return minQe;
		}

		private static bool IsDivisibleToGroups(long[] g, long[] rest, long gWeight, int gCount)
		{
			var weight = g.Sum();
			if (gCount == 1 && weight == gWeight)
				return true;

			for (var i = 0; i < rest.Length; i++)
			{
				if (g.Contains(rest[i]))
					continue;

				var newWeight = weight + rest[i];
				if (newWeight > gWeight)
					continue;

				var tmp = new[] { rest[i] }.Concat(g).ToArray();
				if (newWeight == gWeight && IsDivisibleToGroups(Array.Empty<long>(), rest.Except(tmp).ToArray(), gWeight, gCount - 1))
					return true;

				// newWeight < gWeight
				else if (IsDivisibleToGroups(tmp, rest, gWeight, gCount))
					return true;
			}

			return false;
		}

		private static long QuantumEntanglement(long[] a)
			=> a.Aggregate((a, b) => a * b);
	}
}
