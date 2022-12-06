using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day06
{
	internal class TuningTrouble : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			// part1
			long? answer1 = 4;
			for (; answer1 < input[0].Length; answer1++)
				if (input[0].Skip((int)answer1 - 4).Take(4).Distinct().Count() == 4)
					break;

			// part2
			long? answer2 = 14;
			for (; answer2 < input[0].Length; answer2++)
				if (input[0].Skip((int)answer2 - 14).Take(14).Distinct().Count() == 14)
					break;

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
