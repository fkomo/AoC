using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day07
{
	internal class TheTreacheryOfWhales : ProblemBase
	{
		protected override (long?, long?) SolveProblem(string[] input)
		{
			var inputN = input.First().Split(',').Select(s => int.Parse(s))
				.OrderBy(i => i)
				.ToArray();

			var max = inputN.Max();

			// part1
			long result1 = long.MaxValue;
			for (var m = 0; m < max; m++)
			{
				long fuel = 0;
				for (var i = 0; i < inputN.Length; i++)
				{
					fuel += Math.Abs(inputN[i] - m);
					if (fuel > result1)
						break;
				}
				if (fuel < result1)
					result1 = fuel;
			}

			// part2
			long result2 = long.MaxValue;
			for (var m = 0; m < max; m++)
			{
				long fuel = 0;
				for (var i = 0; i < inputN.Length; i++)
				{
					for (var d = Math.Abs(inputN[i] - m); d > 0; d--)
					{
						fuel += d;
						if (fuel > result2)
							break;
					}
					if (fuel > result2)
						break;
				}
				if (fuel < result2)
					result2 = fuel;
			}

			return (result1, result2);
		}
	}
}
