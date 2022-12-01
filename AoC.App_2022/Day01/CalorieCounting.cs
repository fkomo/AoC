using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day01
{
	internal class CalorieCounting : ProblemBase
	{
		protected override (long, long) SolveProblem()
		{
			var input = ReadInputLines();
			DebugLine($"{ input.Length } lines");

			// part1
			var elfCalories = new List<int>();
			var calList = new List<int>();
			foreach (var cal in input)
			{
				if (cal == string.Empty && calList.Any())
				{
					elfCalories.Add(calList.Sum());
					calList.Clear();
				}
				else
					calList.Add(int.Parse(cal));
			}
			long result1 = elfCalories.Max();

			// part2
			long result2 = elfCalories.OrderByDescending(c => c).Take(3).Sum();

			return (result1, result2);
		}
	}
}
