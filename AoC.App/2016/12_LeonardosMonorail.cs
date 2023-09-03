using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_12;

[AoCPuzzle(Year = 2016, Day = 12, Answer1 = "318003", Answer2 = "9227657", Skip = false)]
public class LeonardosMonorail : PuzzleBase
{
	public class Instruction
	{
		public enum TypeEnum
		{
			Inc,
			Dec,
			Jnz,
			Cpy,
			Tgl
		}

		public TypeEnum Type;

		public long[] OpValue;
		public bool[] IsOpConst;

		public Instruction(TypeEnum type, int opCount)
		{
			Type = type;
			OpValue = new long[opCount];
			IsOpConst = new bool[opCount];
		}

		public long Op(v4i reg, int i) => IsOpConst[i] ? OpValue[i] : reg[(int)OpValue[i]];
	}

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var instr = input.Select(x => x.Split(' ')).ToArray();

		// part1
		var answer1 = ExecPasswordCheckingLogic(instr, new v4i(0)).X;

		// part2
		var answer2 = ExecPasswordCheckingLogic(instr, new v4i(0, 0, 1, 0)).X;

		return (answer1.ToString(), answer2.ToString());
	}

	public static Instruction[] ParseInstructions(string[][] instrRaw)
	{
		return instrRaw.Select(i =>
		{
			Instruction instruction = null;

			if (i[0][0] == 'c')
			{
				instruction = new Instruction(Instruction.TypeEnum.Cpy, 2);

				for (var op = 0; op < 2; op++)
				{
					if (long.TryParse(i[op + 1], out long opValue))
					{
						instruction.OpValue[op] = opValue;
						instruction.IsOpConst[op] = true;
					}
					else
						instruction.OpValue[op] = i[op + 1][0] - 'a';
				}
			}
			else if (i[0][0] == 'i')
			{
				instruction = new Instruction(Instruction.TypeEnum.Inc, 1);
				if (long.TryParse(i[1], out long opValue))
				{
					instruction.OpValue[0] = opValue;
					instruction.IsOpConst[0] = true;
				}
				else
					instruction.OpValue[0] = i[1][0] - 'a';
			}
			else if (i[0][0] == 'd')
			{
				instruction = new Instruction(Instruction.TypeEnum.Dec, 1);
				if (long.TryParse(i[1], out long opValue))
				{
					instruction.OpValue[0] = opValue;
					instruction.IsOpConst[0] = true;
				}
				else
					instruction.OpValue[0] = i[1][0] - 'a';
			}
			else if (i[0][0] == 'j')
			{
				instruction = new Instruction(Instruction.TypeEnum.Jnz, 2);

				for (var op = 0; op < 2; op++)
				{
					if (long.TryParse(i[op + 1], out long opValue))
					{
						instruction.OpValue[op] = opValue;
						instruction.IsOpConst[op] = true;
					}
					else
						instruction.OpValue[op] = i[op + 1][0] - 'a';
				}
			}
			else if (i[0][0] == 't')
			{
				instruction = new Instruction(Instruction.TypeEnum.Tgl, 1);
				if (long.TryParse(i[1], out long opValue))
				{
					instruction.OpValue[0] = opValue;
					instruction.IsOpConst[0] = true;
				}
				else
					instruction.OpValue[0] = i[1][0] - 'a';
			}

			return instruction;

		}).ToArray();
	}

	public static v4i ExecPasswordCheckingLogic(string[][] instrRaw, v4i reg, 
		Func<Instruction[], int, v4i, (v4i Reg, Instruction[] Instr)> unknownInstr = null)
	{
		var instr = ParseInstructions(instrRaw);
		for (var i = 0; i < instr.Length; i++)
		{
			var ii = instr[i];
			switch (ii.Type)
			{
				case Instruction.TypeEnum.Cpy:
					{
						// skip invalid instruction (cpy ? n)
						if (ii.IsOpConst[1])
							break;

						reg[(int)ii.OpValue[1]] = ii.Op(reg, 0);
					}
					break;

				case Instruction.TypeEnum.Inc:
					{
						// skip invalid instruction (inc 1)
						if (ii.IsOpConst[0])
							break;

						reg[(int)ii.OpValue[0]]++;
					}
					break;

				case Instruction.TypeEnum.Dec:
					{
						// skip invalid instruction (dec 1)
						if (ii.IsOpConst[0])
							break;

						reg[(int)ii.OpValue[0]]--;
					}
					break;

				case Instruction.TypeEnum.Jnz:
					{
						if (ii.Op(reg, 0) > 0)
							i += (int)ii.Op(reg, 1) - 1;
					}
					break;

				default:
					{
						if (unknownInstr != null)
							(reg, instr) = unknownInstr(instr, i, reg);
					}
					break;
			}
		}

		return reg;
	}
}