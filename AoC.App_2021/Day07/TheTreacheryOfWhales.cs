using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day07
{
	internal class TheTreacheryOfWhales : ProblemBase
	{
		protected override (long, long) SolveProblem()
		{
			var input = ReadInputLine().Split(',').Select(s => int.Parse(s)).ToArray();
			DebugLine($"{ input.Length } crab submarines");

			// part1
			var fuel = new List<int>();
			for (var m = 0; m < input.Max(); m++)
				fuel.Add(input.Sum(p => Math.Abs(p - m)));
			long result1 = fuel.Min();

			// part2
			fuel.Clear();
			for (var m = 0; m < input.Max(); m++)
				fuel.Add(input.Sum(p =>
				{
					var d = Math.Abs(p - m);

					var f = 0;
					for (; d > 0; d--)
						f += d;

					return f;
				}));
			long result2 = fuel.Min();

			return (result1, result2);
		}
	}
}
