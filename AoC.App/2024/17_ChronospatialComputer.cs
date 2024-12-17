using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_17;

[AoCPuzzle(Year = 2024, Day = 17, Answer1 = "2,7,4,7,2,1,7,5,1", Answer2 = null, Skip = false)]
public class ChronospatialComputer : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var program = input[4].ToNumArray();

		// part1
		var answer1 = GetOutput(new v3i(input[0].ToNumArray()[0], input[1].ToNumArray()[0], input[2].ToNumArray()[0]), program);

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	const int _A = 0;
	const int _B = 1;
	const int _C = 2;

	static string GetOutput(v3i reg, long[] program)
	{
		long AsCombo(long operand) => operand > 3 ? reg[(int)operand - 4] : operand;

		var output = new List<long>();
		var i = 0;
		while (i >= 0 && i < program.Length - 1)
		{
			var opcode = program[i];
			var operand = program[i + 1];

			switch (opcode)
			{
				// adv
				case 0: reg[_A] /= (long)System.Math.Pow(2, AsCombo(operand)); break;
				// bxl
				case 1: reg[_B] ^= operand; break;
				// bst
				case 2: reg[_B] = AsCombo(operand) % 8; break;
				// bxc
				case 4: reg[_B] ^= reg[_C]; break;
				// out
				case 5: output.Add(AsCombo(operand) % 8); break;
				// bdv
				case 6: reg[_B] = reg[_A] / (long)System.Math.Pow(2, AsCombo(operand)); break;
				// cdv
				case 7: reg[_C] = reg[_A] / (long)System.Math.Pow(2, AsCombo(operand)); break;
				// jnz
				case 3:
					if (reg[_A] != 0)
					{
						i = (int)operand;
						continue;
					}
					break;
			}

			i += 2;
		}

		return string.Join(',', output);
	}

	static string GetOutput(v3i reg)
	{
		var output = new List<long>();

		while (true)
		{
			// 2,4
			reg[_B] = reg[_A] % 8;
			// 1,2
			reg[_B] ^= 2;
			// 7,5
			reg[_C] = reg[_A] / (long)System.Math.Pow(2, reg[_B]);
			// 4,7
			reg[_B] ^= reg[_C];
			// 1,3
			reg[_B] ^= 3;
			// 5,5
			output.Add(reg[_B] % 8);
			// 0,3
			reg[_A] /= 8;
			// 3,0
			if (reg[_A] == 0)
				break;
		}

		return string.Join(',', output);
	}

	static string GetOutputSample(v3i reg)
	{
		var output = new List<long>();

		while (true)
		{
			// 0,1
			reg[_A] /= 2;
			// 5,4
			output.Add(reg[_A] % 8);
			// 3,0
			if (reg[_A] == 0)
				break;
		}

		return string.Join(',', output);
	}
}