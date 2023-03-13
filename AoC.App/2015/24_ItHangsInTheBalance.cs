using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_24
{
	[AoCPuzzle(Year = 2015, Day = 24, Answer1 = "11846773891", Answer2 = null)]
	public class ItHangsInTheBalance : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			string answer2 = null;

			var packages = input.Select(i => long.Parse(i)).ToArray();

			// part1
			var answer1 = long.MaxValue;
			var minG1Count = long.MaxValue;

			var gWeight = packages.Sum() / 3;
			Debug.Line($"gWeight={gWeight}");
			for (var g1Length = 2; g1Length < packages.Length - 2; g1Length++)
			{
				if (g1Length > minG1Count)
					break;

				foreach (var g1 in Alg.Combinatorics.Combinations(packages, g1Length)
					.Where(g => g.Sum() == gWeight))
				{
					var g23 = packages.Except(g1);
					if (!DivideTo2Groups(Array.Empty<long>(), g23.ToArray(), gWeight))
						continue;

					var qe = QuantumEntanglement(g1.ToArray());
					if (qe < answer1)
					{
						answer1 = qe;
						minG1Count = g1Length;
						Debug.Line($"g1:[{string.Join(", ", g1)}] qe={qe} ... g2/3:[{string.Join(", ", g23)}]");
					}
				}
			}

			// part2

			return (answer1.ToString(), answer2);
		}

		private static bool DivideTo2Groups(long[] g, long[] all, long gWeight)
		{
			var gSum = g.Sum();
			if (gSum == gWeight)
				return true;

			for (var i = 0; i < all.Length; i++)
			{
				if (g.Contains(all[i]) || gSum + all[i] > gWeight)
					continue;

				if (DivideTo2Groups(new[] { all[i] }.Concat(g).ToArray(), all, gWeight))
					return true;
			}

			return false;
		}

		private static long QuantumEntanglement(long[] a)
			=> a.Aggregate((a, b) => a * b);
	}
}
