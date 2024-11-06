using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2017_08;

[AoCPuzzle(Year = 2017, Day = 08, Answer1 = "3880", Answer2 = "5035", Skip = false)]
public class IHeardYouLikeRegisters : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var register = new Dictionary<string, long>();

		// part1
		var tmpMax = long.MinValue;
		foreach (var instr in input)
		{
			var parts = instr.Split(' ');

			var src = parts[4];
			if (!register.ContainsKey(src))
				register.Add(src, 0);

			var right = long.Parse(parts[6]);

			switch (parts[5])
			{
				case ">": if (register[src] <= right) continue; break;
				case "<": if (register[src] >= right) continue; break;
				case ">=": if (register[src] < right) continue; break;
				case "<=": if (register[src] > right) continue; break;
				case "==": if (register[src] != right) continue; break;
				case "!=": if (register[src] == right) continue; break;
			}

			var dest = parts[0];
			if (!register.ContainsKey(dest))
				register.Add(dest, 0);

			var left = long.Parse(parts[2]);

			switch (parts[1])
			{
				case "inc":
					register[dest] = register[dest] + left;
					break;

				case "dec":
					register[dest] = register[dest] - left;
					break;
			}

			tmpMax = System.Math.Max(register.Max(x => x.Value), tmpMax);
		}
		var answer1 = register.Max(x => x.Value);

		// part2
		var answer2 = tmpMax;

		return (answer1.ToString(), answer2.ToString());
	}
}