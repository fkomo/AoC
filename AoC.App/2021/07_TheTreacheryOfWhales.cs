using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2021_07
{
	internal class TheTreacheryOfWhales : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var inputN = input.First().Split(',').Select(s => int.Parse(s))
				.OrderBy(i => i)
				.ToArray();

			var max = inputN.Max();

			// part1
			long answer1 = long.MaxValue;
			for (var m = 0; m < max; m++)
			{
				long fuel = 0;
				for (var i = 0; i < inputN.Length; i++)
				{
					fuel += Math.Abs(inputN[i] - m);
					if (fuel > answer1)
						break;
				}
				if (fuel < answer1)
					answer1 = fuel;
			}

			// part2
			long answer2 = long.MaxValue;
			for (var m = 0; m < max; m++)
			{
				long fuel = 0;
				for (var i = 0; i < inputN.Length; i++)
				{
					for (var d = Math.Abs(inputN[i] - m); d > 0; d--)
					{
						fuel += d;
						if (fuel > answer2)
							break;
					}
					if (fuel > answer2)
						break;
				}
				if (fuel < answer2)
					answer2 = fuel;
			}

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
