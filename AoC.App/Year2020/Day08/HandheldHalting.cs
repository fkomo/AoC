using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2020.Day08
{
	public class HandheldHalting : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			long? answer1 = 0;
			var linesExecuted = new List<int>();
			for (var i = 0; !linesExecuted.Contains(i);)
			{
				linesExecuted.Add(i);

				var split = input[i].Split(' ');
				switch (split[0])
				{
					case "nop": 
						i++;
						continue;
					
					case "acc":
						answer1 += int.Parse(split[1]);
						i++;
						break;

					case "jmp": 
						i += int.Parse(split[1]);
						break;
				}
			}

			// part2
			long? answer2 = null;
			for (var i = 0; i < input.Length; i++)
			{
				if (input[i].StartsWith("nop"))
					input[i] = input[i].Replace("nop", "jmp");
				else if (input[i].StartsWith("jmp"))
					input[i] = input[i].Replace("jmp", "nop");
				else
					continue;

				answer2 = ComputeAcc(input);
				if (answer2.HasValue)
					break;

				if (input[i].StartsWith("nop"))
					input[i] = input[i].Replace("nop", "jmp");
				else if (input[i].StartsWith("jmp"))
					input[i] = input[i].Replace("jmp", "nop");
			}

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long? ComputeAcc(string[] input)
		{
			long? acc = 0;
			var linesExecuted = new List<int>();
			for (var i = 0; !linesExecuted.Contains(i);)
			{
				if (i >= input.Length)
					return acc;

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

			return null;
		}
	}
}
