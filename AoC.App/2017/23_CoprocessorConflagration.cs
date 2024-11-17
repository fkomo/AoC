using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2017_23;

[AoCPuzzle(Year = 2017, Day = 23, Answer1 = "8281", Answer2 = null, Skip = false)]
public class CoprocessorConflagration : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var instructions = input.Select(x =>
			{
				var opStr = x.Split(' ')[1..];
				return new Instruction(x, opStr, opStr.Select(x => { return int.TryParse(x, out int i) ? i : int.MinValue; }).ToArray());
			})
			.ToArray();

		var registers = "abcdefgh".ToDictionary(x => x, x => 0L);

		// part1
		var answer1 = 0L;
		int i = 0;
		while (i >= 0 && i < instructions.Length)
		{
			var instr = instructions[i];
			if (instr.Raw[0] == 'j')
			{
				if (Process_Jnz(instr, registers, out int jump))
				{
					i += jump;
					continue;
				}
			}
			else
			{
				var op1 = instr.OpStr[1][0];
				var y = char.IsLetter(op1) ? registers[op1] : instr.OpNum[1];

				var op0 = instr.OpStr[0][0];

				if (instr.Raw[1] == 'e') // set
					registers[op0] = y;

				else if (instr.Raw[0] == 's') // sub
					registers[op0] -= y;

				else if (instr.Raw[0] == 'm') // mul
				{
					registers[op0] *= y;
					answer1++;
				}
			}

			i++;
		}

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	record class Instruction(string Raw, string[] OpStr, int[] OpNum);

	static bool Process_Jnz(Instruction instr, Dictionary<char, long> registers, out int i)
	{
		i = 0;

		var op0 = instr.OpStr[0][0];
		var x = char.IsLetter(op0) ? registers[op0] : instr.OpNum[0];
		if (x != 0)
		{
			var op1 = instr.OpStr[1][0];
			i += char.IsLetter(op1) ? (int)registers[op1] : instr.OpNum[1];
			return true;
		}

		return false;
	}
}