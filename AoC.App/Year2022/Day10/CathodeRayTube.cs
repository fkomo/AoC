using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day10
{
	public class CathodeRayTube : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			// part1
			var i = 0;
			var x = 1;
			string exec = null;
			long? answer1 = 0;
			for (var cycle = 1; i < input.Length; cycle++)
			{
				if (cycle == 20 || (cycle - 20) % 40 == 0)
					answer1 += cycle * x;

				if (exec != null)
				{
					x += int.Parse(exec["addx ".Length..]);
					exec = null;
					i++;
				}
				else
				{
					var instr = input[i];
					if (instr == "noop")
						i++;
					else
						exec = instr;
				}
			}

			// part2
			i = 0;
			x = 1;
			exec = null;
			var output = new bool[6, 40];
			var outputx = 0;
			var outputy = 0;
			for (var cycle = 1; i < input.Length; cycle++)
			{
				output[outputy, outputx] = Math.Abs(x + 1 - cycle % 40) <= 1;
				outputx++;

				if (exec != null)
				{
					x += int.Parse(exec["addx ".Length..]);
					exec = null;
					i++;
				}
				else
				{
					var instr = input[i];
					if (instr == "noop")
						i++;
					else
						exec = instr;
				}

				if (cycle % 40 == 0)
				{
					outputy++;
					outputx = 0;
				}
			}

			var answer2 = Year2021.Day13.CharCodes.ToString(output.GetLength(1), output.GetLength(0), output);

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
