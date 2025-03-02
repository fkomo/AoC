using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;
using Operation = System.Func<Ujeby.Vectors.v4i, Ujeby.Vectors.v4i, Ujeby.Vectors.v4i>;

namespace Ujeby.AoC.App._2018_16;

[AoCPuzzle(Year = 2018, Day = 16, Answer1 = "624", Answer2 = null, Skip = false)]
public class ChronalClassification : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var tests = input.Split(string.Empty).Where(x => x.Length == 3)
			.Select(x => x.Select(xx => new v4i(xx.ToNumArray())).ToArray())
			.ToArray();

		// part1
		var operations = new Operation[]
		{
			addr, addi,
			mulr, muli,
			banr, bani,
			borr, bori,
			setr, seti,
			gtir, gtri, gtrr,
			eqir, eqri, eqrr
		};

		var answer1 = tests.Count(x => Has3OrMoreOpCodes(operations, x));

		// part2
		var reg = v4i.Zero;

		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static bool Has3OrMoreOpCodes(Operation[] operations, v4i[] test)
	{
		var count = 0;
		foreach (var op in operations)
		{
			try
			{
				if (op(test[_regBefore], test[_instr]) != test[_regAfter])
					continue;

				count++;
				if (count == 3)
					return true;
			}
			catch (Exception)
			{
				// reg / immediate value mismatch
			}
		}

		return false;
	}

	const int _regBefore = 0;
	const int _instr = 1;
	const int _regAfter = 2;

	const int rA = 0;
	const int rB = 1;
	const int rC = 2;
	const int rD = 3;

	const int iOpCode = 0;
	const int iA = 1;
	const int iB = 2;
	const int iC = 3;

	// addition
	static v4i addr(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] + reg[instr[iB]]);
	static v4i addi(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] + instr[iB]);

	// multiplication
	static v4i mulr(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] * reg[instr[iB]]);
	static v4i muli(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] * instr[iB]);

	// bitwise AND
	static v4i banr(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] & reg[instr[iB]]);
	static v4i bani(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] & instr[iB]);

	// bitwise OR
	static v4i borr(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] | reg[instr[iB]]);
	static v4i bori(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] | instr[iB]);

	// assigment
	static v4i setr(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]]);
	static v4i seti(v4i reg, v4i instr) => reg.Set(instr[iC], instr[iA]);

	// greather-than testing
	static v4i gtir(v4i reg, v4i instr) => reg.Set(instr[iC], instr[iA] > reg[instr[iB]] ? 1 : 0);
	static v4i gtri(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] > instr[iB] ? 1 : 0);
	static v4i gtrr(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] > reg[instr[iB]] ? 1 : 0);

	// equality testing
	static v4i eqir(v4i reg, v4i instr) => reg.Set(instr[iC], instr[iA] == reg[instr[iB]] ? 1 : 0);
	static v4i eqri(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] == instr[iB] ? 1 : 0);
	static v4i eqrr(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] == reg[instr[iB]] ? 1 : 0);
}