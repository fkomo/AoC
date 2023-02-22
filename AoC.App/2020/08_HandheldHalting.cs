using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2020_08
{
	[AoCPuzzle(Year = 2020, Day = 08, Answer1 = "2034", Answer2 = "672")]
	public class HandheldHalting : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			ComputeAcc(input, out long answer1);

			// part2
			long answer2 = 0;
			for (var i = 0; i < input.Length; i++)
			{
				var prev = input[i];
				if (input[i].StartsWith("nop"))
					input[i] = input[i].Replace("nop", "jmp");
				else if (input[i].StartsWith("jmp"))
					input[i] = input[i].Replace("jmp", "nop");
				else
					continue;

				if (ComputeAcc(input, out answer2))
					break;

				input[i] = prev;
			}

			return (answer1.ToString(), answer2.ToString());
		}

		private static bool ComputeAcc(string[] input, out long acc)
		{
			acc = 0;
			var linesExecuted = new List<int>();
			for (var i = 0; !linesExecuted.Contains(i);)
			{
				if (i >= input.Length)
					return true;

				linesExecuted.Add(i);

				var split = input[i].Split(' ');
				switch (split[0])
				{
					case "nop":
						i++;
						break;

					case "acc":
						acc += int.Parse(split[1]);
						i++;
						break;

					case "jmp":
						i += int.Parse(split[1]);
						break;
				}
			}

			return false;
		}
	}
}
