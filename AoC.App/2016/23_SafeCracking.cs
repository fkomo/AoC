using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_23;

[AoCPuzzle(Year = 2016, Day = 23, Answer1 = "10440", Answer2 = "479007000", Skip = true)]
public class SafeCracking : PuzzleBase
{
	public class AssembunnyV2 : _2016_12.LeonardosMonorail.Assembunny
	{
		public AssembunnyV2(string[][] instructions) : base(instructions)
		{
		}

		public override Instruction ParseInstruction(string[] instruction)
		{
			if (instruction[0][0] != 't')
				return null;

			var newInstruction = new Instruction(Instruction.TypeEnum.Unsupported, 1, 
				name: instruction[0]);

			if (long.TryParse(instruction[1], out long opValue))
			{
				newInstruction.OpValue[0] = opValue;
				newInstruction.IsOpConst[0] = true;
			}
			else
				newInstruction.OpValue[0] = instruction[1][0] - 'a';

			return newInstruction;
		}

		public override void ExecInstruction(int i, v4i reg)
		{
			var tgl = _instructions[i].Op(reg, 0);

			// outside of program
			if ((tgl + i >= _instructions.Length) || (tgl + i < 0))
				return;

			var iiToTgl = _instructions[i + tgl];

			// one-arg
			if (iiToTgl.OpValue.Length == 1)
				iiToTgl.Type = (iiToTgl.Type == Instruction.TypeEnum.Inc) ? Instruction.TypeEnum.Dec : Instruction.TypeEnum.Inc;

			// two-arg
			else if (iiToTgl.OpValue.Length == 2)
				iiToTgl.Type = (iiToTgl.Type == Instruction.TypeEnum.Jnz) ? Instruction.TypeEnum.Cpy : Instruction.TypeEnum.Jnz;
		}
	}

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
#if _DEBUG_SAMPLE
		var reg = new v4i(0, 0, 0, 0);
#else
		var reg = new v4i(7, 0, 0, 0);
#endif
		var instrRaw = input.Select(x => x.Split(' ')).ToArray();

		// part1
		var answer1 = new AssembunnyV2(instrRaw).Exec(reg).X;

		// part2
		// TODO 2016/23 OPTIMIZE p2 (?)
		var answer2 = new AssembunnyV2(instrRaw).Exec(new v4i(12, 0, 0, 0)).X;

		return (answer1.ToString(), answer2.ToString());
	}
}