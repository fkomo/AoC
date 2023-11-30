using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_12;

[AoCPuzzle(Year = 2016, Day = 12, Answer1 = "318003", Answer2 = "9227657", Skip = false)]
public class LeonardosMonorail : PuzzleBase
{
	public class Assembunny
	{
		public class Instruction
		{
			public enum TypeEnum
			{
				Unsupported,
				Inc,
				Dec,
				Jnz,
				Cpy,
			}

			public TypeEnum Type;
			public string Name;

			public long[] OpValue;
			public bool[] IsOpConst;

			public Instruction(TypeEnum type, int opCount, 
				string name = null)
			{
				Type = type;
				Name = name ?? type.ToString();

				OpValue = new long[opCount];
				IsOpConst = new bool[opCount];
			}

			public long Op(v4i reg, int i) => IsOpConst[i] ? OpValue[i] : reg[(int)OpValue[i]];
		}

		protected Instruction[] _instructions;

		public Assembunny(string[][] instructions)
		{
			ParseInstructions(instructions);
		}

		private void ParseInstructions(string[][] instructions)
		{
			var tmp = new List<Instruction>();
			for (int x = 0; x < instructions.Length; x++)
			{
				var i = instructions[x];
				Instruction instruction;

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
				else
					instruction = ParseInstruction(i);

				if (instruction != null)
					tmp.Add(instruction);
			}

			_instructions = tmp.ToArray();
		}

		public virtual v4i Exec(v4i reg)
		{
			for (var i = 0; i < _instructions.Length; i++)
			{
				if (!BeforeExecInstruction(i))
					break;

				var ii = _instructions[i];
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
						ExecInstruction(i, reg);
						break;
				}
			}

			return reg;
		}

		public virtual void ExecInstruction(int i, v4i reg)
		{
		}

		public virtual bool BeforeExecInstruction(int i) => true;

		public virtual Instruction ParseInstruction(string[] instr) => null;
	}

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var instructions = input.Select(x => x.Split(' ')).ToArray();

		// part1
		var answer1 = new Assembunny(instructions).Exec(new v4i(0)).X;

		// part2
		var answer2 = new Assembunny(instructions).Exec(new v4i(0, 0, 1, 0)).X;

		return (answer1.ToString(), answer2.ToString());
	}
}