using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2021_04
{
	[AoCPuzzle(Year = 2021, Day = 04, Answer1 = "10680", Answer2 = "31892")]
	public class GiantSquid : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var numOrder = input.First().Split(',').Select(s => int.Parse(s)).ToArray();

			input = input.Skip(2).ToArray();
			var bingoSize = 5;
			var bingoSets = CreateBingoSets(input, bingoSize);

			// part1
			long answer1 = -1;
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
						answer1 = n * bingoSets[bs].Where(n => n > 0).Sum();
						break;
					}
				}

				if (answer1 > 0)
					break;
			}

			// part2
			bingoSets = CreateBingoSets(input, bingoSize);
			long answer2 = -1;
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
						answer2 = n * bingoSets[bs].Where(n => n > 0).Sum();
						bingoSets[bs] = null;
					}
				}
			}

			return (answer1.ToString(), answer2.ToString());
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
