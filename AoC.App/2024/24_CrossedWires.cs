using System.Text;
using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2024_24;

[AoCPuzzle(Year = 2024, Day = 24, Answer1 = "52728619468518", Answer2 = null, Skip = false)]
public class CrossedWires : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var inputSplit = input.Split(string.Empty);

		var inputs = inputSplit[0].ToDictionary(x => x[..3], x => x[^1] == '1');
		var gates = inputSplit[1].ToDictionary(x => x[^3..], x => x.Split(' ')[..3]);

		// part1
		var zBuilder = new StringBuilder();
		foreach (var z in gates.Keys.Where(x => x[0] == 'z').OrderByDescending(x => x))
			zBuilder.Append(GetGateOutput(z, inputs, gates) ? "1" : "0");

		var answer1 = Ujeby.Math.BaseToDec(zBuilder.ToString());

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static bool GetGateOutput(string gate, Dictionary<string, bool> inputs, Dictionary<string, string[]> gates)
	{
		if (inputs.TryGetValue(gate, out bool value))
			return value;

		var ops = gates[gate];
		return ops[1] switch
		{
			"OR" => GetGateOutput(ops[0], inputs, gates) || GetGateOutput(ops[2], inputs, gates),
			"XOR" => GetGateOutput(ops[0], inputs, gates) ^ GetGateOutput(ops[2], inputs, gates),
			"AND" => GetGateOutput(ops[0], inputs, gates) && GetGateOutput(ops[2], inputs, gates),
			_ => throw new NotImplementedException(ops[1]),
		};
	}
}