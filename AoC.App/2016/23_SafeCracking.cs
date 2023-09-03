using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_23;

[AoCPuzzle(Year = 2016, Day = 23, Answer1 = "10440", Answer2 = "479007000", Skip = true)]
public class SafeCracking : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
#if _DEBUG_SAMPLE
		var reg = new v4i(0, 0, 0, 0);
#else
		var reg = new v4i(7, 0, 0, 0);
#endif

		// part1
		var instrRaw = input.Select(x => x.Split(' ')).ToArray();
		var answer1 = _2016_12.LeonardosMonorail.ExecPasswordCheckingLogic(instrRaw, reg, ToggleInstr).X;

		// part2
		instrRaw = input.Select(x => x.Split(' ')).ToArray();
		var answer2 = _2016_12.LeonardosMonorail.ExecPasswordCheckingLogic(instrRaw, new v4i(12, 0, 0, 0)).X;

		return (answer1.ToString(), answer2.ToString());
	}

	private static (v4i Reg, _2016_12.LeonardosMonorail.Instruction[] Instr) ToggleInstr(
		_2016_12.LeonardosMonorail.Instruction[] instr, int i, v4i reg)
	{
		if (instr[i].Type != _2016_12.LeonardosMonorail.Instruction.TypeEnum.Tgl)
			return (reg, instr);

		var tgl = instr[i].Op(reg, 0);

		// outside of program
		if ((tgl + i >= instr.Length) || (tgl + i < 0))
			return (reg, instr);

		var iiToTgl = instr[i + tgl];

		// one-arg
		if (iiToTgl.OpValue.Length == 1)
			iiToTgl.Type = (iiToTgl.Type == _2016_12.LeonardosMonorail.Instruction.TypeEnum.Inc) ?
				_2016_12.LeonardosMonorail.Instruction.TypeEnum.Dec : _2016_12.LeonardosMonorail.Instruction.TypeEnum.Inc;

		// two-arg
		else if (iiToTgl.OpValue.Length == 2)
			iiToTgl.Type = (iiToTgl.Type == _2016_12.LeonardosMonorail.Instruction.TypeEnum.Jnz) ?
				_2016_12.LeonardosMonorail.Instruction.TypeEnum.Cpy : _2016_12.LeonardosMonorail.Instruction.TypeEnum.Jnz;

		return (reg, instr);
	}
}