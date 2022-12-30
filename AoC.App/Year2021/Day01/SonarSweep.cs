using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2021.Day01
{
	internal class SonarSweep : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			long answer1 = 0;
			for (var i = 1; i < input.Length; i++)
				if (int.Parse(input[i - 1]) < int.Parse(input[i]))
					answer1++;

			// part2
			long answer2 = 0;
			for (var i = 0; i < input.Length - 3; i++)
			{
				var sum1 = 
					int.Parse(input[i + 0]) + 
					int.Parse(input[i + 1]) + 
					int.Parse(input[i + 2]);

				var sum2 =
					int.Parse(input[i + 1]) +
					int.Parse(input[i + 2]) +
					int.Parse(input[i + 3]);

				if (sum2 > sum1)
					answer2++;
			}

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
