using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day04
{
	internal class GiantSquid : ProblemBase
	{
		protected override (long, long) SolveProblem()
		{
			long result1 = 0;
			long result2 = 0;

			var numOrder = ReadInputLine(0).Split(',').ToArray();
			DebugLine($"{ numOrder.Length } numbers drawn");

			var input = ReadInputLines().Skip(2).ToArray();
			var bingoSets = new List<int[]>();
			for (var b = 0; b < input.Length; b += 6)
			{
				var set = (
					input[b + 0] + " " +
					input[b + 1] + " " +
					input[b + 2] + " " +
					input[b + 3] + " " +
					input[b + 4]
				)
				.Split(' ', StringSplitOptions.RemoveEmptyEntries)
				.Select(i => int.Parse(i))
				.ToArray();

				bingoSets.Add(set);
			}
			DebugLine($"{ bingoSets.Count } bingo sets");

			// part1

			// part2

			return (result1, result2);
		}
	}
}
