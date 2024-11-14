using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2017_18;

[AoCPuzzle(Year = 2017, Day = 18, Answer1 = "1187", Answer2 = null, Skip = false)]
public class Duet : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var instructions = input.Select(x =>
			{
				var opStr = x.Split(' ')[1..];
				return (Raw: x, OpStr: opStr, OpNum: opStr.Select(x => { return int.TryParse(x, out int i) ? i : int.MinValue; }).ToArray());
			})
			.ToArray();

		var registers = input
			.Select(x => x.Split(' ')[1][0])
			.Where(char.IsLetter)
			.Distinct()
			.ToDictionary(x => x, x => 0L);

		// part1
		int i = 0;
		var lastPlayedFreq = long.MinValue;
		var answer1 = lastPlayedFreq;
		while (i >= 0 && i < instructions.Length)
		{
			var instr = instructions[i];
			if (instr.Raw.StartsWith("sn"))
			{
				var op0 = instr.OpStr[0][0];
				lastPlayedFreq = char.IsLetter(op0) ? registers[op0] : instr.OpNum[0];
				Debug.Line($"{instr.Raw}: {lastPlayedFreq}");
			}
			else if (instr.Raw[0] == 'r')
			{
				if (registers[instr.OpStr[0][0]] != 0)
				{
					answer1 = lastPlayedFreq;
					break;
				}
			}
			else if (instr.Raw[0] == 'j')
			{
				var op0 = instr.OpStr[0][0];
				var x = char.IsLetter(op0) ? registers[op0] : instr.OpNum[0];
				if (x > 0)
				{
					var op1 = instr.OpStr[1][0];
					var y = char.IsLetter(op1) ? registers[op1] : instr.OpNum[1];

					i += (int)y;
					continue;
				}
			}
			else
			{
				var op1 = instr.OpStr[1][0];
				var y = char.IsLetter(op1) ? registers[op1] : instr.OpNum[1];

				var op0 = instr.OpStr[0][0];
				if (instr.Raw[0] == 's')
					registers[op0] = y;

				else if (instr.Raw[0] == 'a')
					registers[op0] += y;

				else if (instr.Raw[1] == 'u')
					registers[op0] *= y;

				else if (instr.Raw[1] == 'o')
					registers[op0] %= y;

				else
					throw new NotImplementedException();
			}

			i++;
		}

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}