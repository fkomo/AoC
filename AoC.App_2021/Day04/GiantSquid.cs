using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day04
{
	internal class GiantSquid : ProblemBase
	{
		protected override (long, long) SolveProblem(string[] input)
		{
			var numOrder = input.First().Split(',').Select(s => int.Parse(s)).ToArray();
			DebugLine($"{ numOrder.Length } numbers drawn");

			input = input.Skip(2).ToArray();
			var bingoSize = 5;
			var bingoSets = CreateBingoSets(input, bingoSize);
			DebugLine($"{ bingoSets.Count } bingo sets");

			// part1
			long result1 = -1;
			foreach (var n in numOrder)
			{
				for (var bs = 0; bs < bingoSets.Count; bs++)
				{
					for (var bsn = 0; bsn < bingoSets[bs].Length; bsn++)
					{
						if (bingoSets[bs][bsn] == n)
							bingoSets[bs][bsn] = -1;
					}

					if (CheckBingo(bingoSets[bs]))
					{
						result1 = n * bingoSets[bs].Where(n => n > 0).Sum();
						break;
					}
				}

				if (result1 > 0)
					break;
			}

			// part2
			bingoSets = CreateBingoSets(input, bingoSize);
			long result2 = -1;
			foreach (var n in numOrder)
			{
				for (var bs = 0; bs < bingoSets.Count; bs++)
				{
					if (bingoSets[bs] == null)
						continue;

					for (var bsn = 0; bsn < bingoSets[bs].Length; bsn++)
					{
						if (bingoSets[bs][bsn] == n)
							bingoSets[bs][bsn] = -1;
					}

					if (CheckBingo(bingoSets[bs]))
					{
						result2 = n * bingoSets[bs].Where(n => n > 0).Sum();
						bingoSets[bs] = null;
					}
				}
			}

			return (result1, result2);
		}

		private static List<int[]> CreateBingoSets(string[] input, int bingoSize)
		{
			var bingoSets = new List<int[]>();
			for (var b = 0; b < input.Length; b += bingoSize + 1)
			{
				var set = string.Empty;
				for (var i = 0; i < bingoSize; i++)
					set += input[b + i] + " ";

				var bingoSet = set
					.Split(' ', StringSplitOptions.RemoveEmptyEntries)
					.Select(i => int.Parse(i))
					.ToArray();

				bingoSets.Add(bingoSet);
			}

			return bingoSets;
		}

		private static bool CheckBingo(int[] bingoSet)
		{
			if (bingoSet.Count(n => n == -1) < 5)
				return false;

			var bingoSize = (int)Math.Sqrt(bingoSet.Length);

			for (var i = 0; i < bingoSize; i++)
			{
				// row
				if (bingoSet.Skip(i * bingoSize).Take(bingoSize).Sum() == -5)
					return true;

				// column
				if (bingoSet[i + 0*5] == -1 && 
					bingoSet[i + 1*5] == -1 &&
					bingoSet[i + 2*5] == -1 &&
					bingoSet[i + 3*5] == -1 &&
					bingoSet[i + 4*5] == -1)
					return true;
			}

			return false;
		}
	}
}
