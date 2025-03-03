using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;
using Instruction = System.Func<Ujeby.Vectors.v4i, Ujeby.Vectors.v4i, Ujeby.Vectors.v4i>;

namespace Ujeby.AoC.App._2018_16;

[AoCPuzzle(Year = 2018, Day = 16, Answer1 = "624", Answer2 = "584", Skip = false)]
public class ChronalClassification : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var tests = input.Split(string.Empty).Where(x => x.Length == 3)
			.Select(x => x.Select(xx => new v4i(xx.ToNumArray())).ToArray())
			.ToArray();

		// part1
		var answer1 = tests.Count(x => x.MatchPossibleInstructions().Count >= 3);

		// part2
		var opCodeMap = tests
			.ToDictionary(x => x, x => x.MatchPossibleInstructions())
			// iteratively remove tests that have only 1 possible instruction and fill opCodeMap with these instructions
			.MacroDataRefinement_Step1(out Dictionary<long, Instruction> tmpOpCodeMap)
			// iteratively find instructions that have only 1 opCode in test data and move it to opCodeMap
			.MacroDataRefinement_Step2(tmpOpCodeMap);

		var reg = v4i.Zero;
		var program = input.Split(string.Empty)[^1].Select(x => new v4i(x.ToNumArray()));
		foreach (var line in program)
			reg = opCodeMap[line[0]](reg, line);

		var answer2 = reg[0];

		return (answer1.ToString(), answer2.ToString());
	}
}
static class Instructions
{
	const int iA = 1;
	const int iB = 2;
	const int iC = 3;

	public readonly static Instruction[] All =
	[
		addr, addi,
		mulr, muli,
		banr, bani,
		borr, bori,
		setr, seti,
		gtir, gtri, gtrr,
		eqir, eqri, eqrr
	];

#pragma warning disable IDE1006 // Naming Styles
	// addition
	public static v4i addr(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] + reg[instr[iB]]);
	public static v4i addi(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] + instr[iB]);

	// multiplication
	public static v4i mulr(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] * reg[instr[iB]]);
	public static v4i muli(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] * instr[iB]);

	// bitwise AND
	public static v4i banr(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] & reg[instr[iB]]);
	public static v4i bani(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] & instr[iB]);

	// bitwise OR
	public static v4i borr(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] | reg[instr[iB]]);
	public static v4i bori(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] | instr[iB]);

	// assigment
	public static v4i setr(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]]);
	public static v4i seti(v4i reg, v4i instr) => reg.Set(instr[iC], instr[iA]);

	// greather-than testing
	public static v4i gtir(v4i reg, v4i instr) => reg.Set(instr[iC], instr[iA] > reg[instr[iB]] ? 1 : 0);
	public static v4i gtri(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] > instr[iB] ? 1 : 0);
	public static v4i gtrr(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] > reg[instr[iB]] ? 1 : 0);

	// equality testing
	public static v4i eqir(v4i reg, v4i instr) => reg.Set(instr[iC], instr[iA] == reg[instr[iB]] ? 1 : 0);
	public static v4i eqri(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] == instr[iB] ? 1 : 0);
	public static v4i eqrr(v4i reg, v4i instr) => reg.Set(instr[iC], reg[instr[iA]] == reg[instr[iB]] ? 1 : 0);
#pragma warning restore IDE1006 // Naming Styles
}

static class Extensions
{
	const int iOpCode = 0;

	const int _regBefore = 0;
	const int _instr = 1;
	const int _regAfter = 2;

	public static List<Instruction> MatchPossibleInstructions(this v4i[] test)
	{
		var instructions = new List<Instruction>();

		foreach (var instruction in Instructions.All)
		{
			try
			{
				if (instruction(test[_regBefore], test[_instr]) != test[_regAfter])
					continue;

				instructions.Add(instruction);
			}
			catch (Exception)
			{
				// reg / immediate value mismatch
			}
		}

		return instructions;
	}

	public static Dictionary<v4i[], List<Instruction>> MacroDataRefinement_Step1(this Dictionary<v4i[], List<Instruction>> tests, out Dictionary<long, Instruction> result)
	{
		result = [];

		while (true)
		{
			// find tests with exact match
			var matchesWithOneInstr = tests.Where(x => x.Value.Count == 1).DistinctBy(x => x.Key[_instr][iOpCode])
				.Select(x => (opCode: x.Key[_instr][iOpCode], instr: x.Value.Single()))
				.ToArray();

			if (matchesWithOneInstr.Length == 0)
				break;

			foreach (var (opCode, instr) in matchesWithOneInstr)
			{
				// instruction matched to opcode
				result.Add(opCode, instr);

				// remove tests with exact match
				tests = tests.Where(x => x.Value.Count > 1)
					.Where(x => x.Key[_instr][iOpCode] != opCode)
					.ToDictionary(x => x.Key, x => x.Value);

				// remove this instruction from other matches
				foreach (var m in tests.Values)
					m.Remove(instr);
			}

			PrintInstructionsMap(result, tests.Count);
		}

		return tests;
	}

	public static Dictionary<long, Instruction> MacroDataRefinement_Step2(this Dictionary<v4i[], List<Instruction>> tests, Dictionary<long, Instruction> opCodeMap)
	{
		while (true)
		{
			var opCodeToPossibleInstr = tests.GroupBy(x => x.Key[_instr][iOpCode])
				.ToDictionary(x => x.Key, x => x.SelectMany(xx => xx.Value.ToArray()).Distinct().ToList());

			var instrToPossibleOpCode = Instructions.All.ToDictionary(x => x, x => opCodeToPossibleInstr.Where(xx => xx.Value.Contains(x)).Select(xx => xx.Key).ToArray());

			var instrWithOneOpCode = instrToPossibleOpCode.Where(x => x.Value.Length == 1).Select(x => (opCode: x.Value[0], instr: x.Key)).ToArray();
			if (instrWithOneOpCode.Length == 0)
				break;

			foreach (var (opCode, instr) in instrWithOneOpCode)
			{
				// instruction matched to opcode
				opCodeMap.Add(opCode, instr);

				// remove tests with exact match
				tests = tests
					.Where(x => x.Key[_instr][iOpCode] != opCode)
					.ToDictionary(x => x.Key, x => x.Value);

				// remove this instruction from other matches
				foreach (var m in tests.Values)
					m.Remove(instr);
			}

			PrintInstructionsMap(opCodeMap, tests.Count);
		}

		if (opCodeMap.Count == Instructions.All.Length - 1)
		{
			// 1 instruction is missing
			opCodeMap.Add(Enumerable.Range(0, 16).Single(x => !opCodeMap.ContainsKey(x)), Instructions.All.Single(x => !opCodeMap.ContainsValue(x)));
			PrintInstructionsMap(opCodeMap, tests.Count);
		}

		return opCodeMap;
	}

	static void PrintInstructionsMap(Dictionary<long, Instruction> map, int testsLeft)
	{
#if DEBUG
		Debug.Line($"{nameof(map)}[{map.Count}/{Instructions.All.Length}] ({testsLeft} tests left):");
		foreach (var oi in map)
			Debug.Line($"  {oi.Key,2}:{oi.Value.Method.Name}");
		Debug.Line();
#endif
	}
}