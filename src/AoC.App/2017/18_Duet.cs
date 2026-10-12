using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2017_18;

[AoCPuzzle(Year = 2017, Day = 18, Answer1 = "1187", Answer2 = "5969", Skip = false)]
public class Duet : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var instructions = input.Select(x =>
			{
				var opStr = x.Split(' ')[1..];
				return new Instruction(x, opStr, opStr.Select(x => { return int.TryParse(x, out int i) ? i : int.MinValue; }).ToArray());
			})
			.ToArray();

		var registers = CreateRegisters(input);

		// part1
		var answer1 = GetLastPlayedFreq(instructions, registers);

		// part2
		Dictionary<char, long>[] pRegs =
		[
			CreateRegisters(input),
			CreateRegisters(input)
		];
		pRegs[0]['p'] = 0;
		pRegs[1]['p'] = 1;

		int[] pInstr = [0, 0];
		long[] pSent = [0, 0];
		bool[] pWait = [false, false];
		Queue<long>[] pQueues =
		[
			new(),
			new()
		];

		bool IsValidInstr(int p) => pInstr[p] >= 0 && pInstr[p] < instructions.Length;

		var p = 0;
		while (true)
		{
			var reg = pRegs[p];
			while (IsValidInstr(p))
			{
				pWait[p] = false;

				var instr = instructions[pInstr[p]];
				if (instr.Raw.StartsWith("sn"))
				{
					var op0 = instr.OpStr[0][0];
					var toSend = char.IsLetter(op0) ? reg[op0] : instr.OpNum[0];
					pQueues[(p + 1) % 2].Enqueue(toSend);
					pSent[p]++;
				}
				else if (instr.Raw[0] == 'r')
				{
					if (pQueues[p].Count == 0)
					{
						pWait[p] = true;
						break;
					}
					else
					{
						var received = pQueues[p].Dequeue();
						pRegs[p][instr.OpStr[0][0]] = received;
					}
				}
				else if (instr.Raw[0] == 'j')
				{
					if (Process_Jgz(instr, reg, out int jump))
					{
						pInstr[p] += jump;
						continue;
					}
				}
				else
					Process_SetAddMulMod(instr, reg);             
			
				pInstr[p]++;
			}

			// both out of bounds
			if (!IsValidInstr(0) && !IsValidInstr(1))
				break;

			// deadlock
			if (pQueues[0].Count == 0 && pQueues[1].Count == 0 && pWait[0] && pWait[1])
				break;

			// switch
			p = (p + 1) % 2;
		}
		var answer2 = pSent[1];

		return (answer1.ToString(), answer2.ToString());
	}

	static long GetLastPlayedFreq(Instruction[] instructions, Dictionary<char, long> registers)
	{
		int i = 0;
		var lastPlayedFreq = long.MinValue;
		while (i >= 0 && i < instructions.Length)
		{
			var instr = instructions[i];
			if (instr.Raw.StartsWith("sn"))
			{
				var op0 = instr.OpStr[0][0];
				lastPlayedFreq = char.IsLetter(op0) ? registers[op0] : instr.OpNum[0];
			}
			else if (instr.Raw[0] == 'r')
			{
				if (registers[instr.OpStr[0][0]] != 0)
					break;
			}
			else if (instr.Raw[0] == 'j')
			{
				if (Process_Jgz(instr, registers, out int jump))
				{
					i += jump;
					continue;
				}
			}
			else
				Process_SetAddMulMod(instr, registers);

			i++;
		}

		return lastPlayedFreq;
	}

	record class Instruction(string Raw, string[] OpStr, int[] OpNum);

	static Dictionary<char, long> CreateRegisters(string[] input)
		=> input
			.Select(x => x.Split(' ')[1][0])
			.Where(char.IsLetter)
			.Distinct()
			.ToDictionary(x => x, x => 0L);

	static void Process_SetAddMulMod(Instruction instr, Dictionary<char, long>  registers)
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
	}

	static bool Process_Jgz(Instruction instr, Dictionary<char, long> registers, out int i)
	{
		i = 0;

		var op0 = instr.OpStr[0][0];
		var x = char.IsLetter(op0) ? registers[op0] : instr.OpNum[0];
		if (x > 0)
		{
			var op1 = instr.OpStr[1][0];
			i += char.IsLetter(op1) ? (int)registers[op1] : instr.OpNum[1];
			return true;
		}

		return false;
	}
}