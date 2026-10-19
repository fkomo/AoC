using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2022_10
{
	[AoCPuzzle(Year = 2022, Day = 10, Answer1 = "14540", Answer2 = "EHZFZHCZ")]
	public class CathodeRayTube : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
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
				output[outputy, outputx] = System.Math.Abs(outputx - x) <= 1;
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

				if ((cycle) % 40 == 0)
				{
					outputy++;
					outputx = 0;
				}
			}

			var answer2 = CharCodes.ToString(output.GetLength(1), output.GetLength(0), output);

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
