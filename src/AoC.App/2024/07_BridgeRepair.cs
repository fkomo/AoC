using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2024_07;

[AoCPuzzle(Year = 2024, Day = 07, Answer1 = "5030892084481", Answer2 = "91377448644679", Skip = false)]
public class BridgeRepair : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var eqs = input.Select(x => x.ToNumArray()).ToArray();

		// part1
		var answer1 = eqs.Where(ValidateFor2Ops).Sum(x => x[0]);

		// part2
		// all operator permutations for all number of operators
		var opCache = eqs.Select(x => x.Length - 2).Distinct()
			.ToDictionary(x => x, x => Ujeby.Alg.Combinatorics.PermutationsWithRep("abc", x).Select(x => x.ToArray()).ToArray());

		var answer2 = eqs.Where(x => ValidateFor2Ops(x) || ValidateFor3Ops(x, opCache)).Sum(x => x[0]);

		return (answer1.ToString(), answer2.ToString());
	}

	static bool ValidateFor2Ops(long[] values)
	{
		var target = values[0];
		var opCount = values.Length - 2;

		for (var ops = 0; ops < (long)System.Math.Pow(2, opCount); ops++)
		{
			var iOp = 1;
			var sum = values[1];
			for (var i = 1; i <= opCount && sum < target; i++, iOp *= 2)
			{
				if ((ops & iOp) > 0)
					sum += values[i + 1];
				else
					sum *= values[i + 1];
			}

			if (sum == target)
				return true;
		}

		return false;
	}

	static bool ValidateFor3Ops(long[] values, Dictionary<int, char[][]> opCache)
	{
		var target = values[0];
		var opCount = values.Length - 2;

		foreach (var op in opCache[opCount])
		{
			var i = 1;
			var sum = values[i];
			foreach (var o in op)
			{
				if (o == 'a')
					sum += values[i + 1];
				else if (o == 'b')
					sum *= values[i + 1];
				else // concatenation
					sum = sum * (long)System.Math.Pow(10, values[i + 1].ToString().Length) + values[i + 1];

				if (sum > target)
					break;

				i++;
			}

			if (sum == target)
				return true;
		}

		return false;
	}
}