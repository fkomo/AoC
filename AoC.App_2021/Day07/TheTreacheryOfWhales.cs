using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day07
{
	internal class TheTreacheryOfWhales : ProblemBase
	{
		protected override (long, long) SolveProblem(string[] input)
		{
			var inputN = input.First().Split(',').Select(s => int.Parse(s)).ToArray();
			DebugLine($"{inputN.Length } crab submarines");

			// part1
			var fuel = new List<int>();
			for (var m = 0; m < inputN.Max(); m++)
				fuel.Add(inputN.Sum(p => Math.Abs(p - m)));
			long result1 = fuel.Min();

			// part2
			fuel.Clear();
			for (var m = 0; m < inputN.Max(); m++)
				fuel.Add(inputN.Sum(p =>
				{
					var f = 0;
					for (var d = Math.Abs(p - m); d > 0; d--)
						f += d;

					return f;
				}));
			long result2 = fuel.Min();

			return (result1, result2);
		}
	}
}
