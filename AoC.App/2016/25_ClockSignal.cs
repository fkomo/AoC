using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_25;

[AoCPuzzle(Year = 2016, Day = 25, Answer1 = "175", Answer2 = "*", Skip = false)]
public class ClockSignal : PuzzleBase
{
	public class AssembunnyV3 : _2016_23.SafeCracking.AssembunnyV2
	{
		private long _lastOut = 1;

		public AssembunnyV3(string[][] instructions) : base(instructions)
		{
		}

		public override Instruction ParseInstruction(string[] instruction)
		{
			if (instruction[0][0] != 'o')
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

		public override v4i Exec(v4i reg)
		{
			_lastOut = 1;
			return base.Exec(reg);
		}

		public override bool BeforeExecInstruction(int i)
		{
			if (i == _instructions.Length - 1)
				return false;

			return true;
		}

		public override void ExecInstruction(int i, v4i reg)
		{
			var toOut = _instructions[i].Op(reg, 0);

			Debug.Text($"{toOut} ", indent: 0);
			if (_lastOut == toOut || (toOut != 0 && toOut != 1))
				throw new Exception("invalid clock signal");

			_lastOut = toOut;
		}
	}

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var instructions = input.Select(x => x.Split(' ')).ToArray();

		var ass = new AssembunnyV3(instructions);

		// part1
		long? answer1 = null;
		for (var i = 0; i < int.MaxValue; i++)
		{
			Debug.Text($"{i}: ");
			try
			{
				ass.Exec(new v4i(i, 0, 0, 0));
				answer1 = i;
				break;
			}
			catch (Exception)
			{
			}
			Debug.Line();
		}

		// part2
		string answer2 = "*";

		return (answer1.ToString(), answer2?.ToString());
	}
}