using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2025_06;

[AoCPuzzle(Year = 2025, Day = 06, Answer1 = "6635273135233", Answer2 = null, Skip = false)]
public class TrashCompactor : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var ops = input.Last().Replace(" ", string.Empty);

		// part1
		var problems = new List<long>[ops.Length];
		for (var i = 0; i < problems.Length; i++)
			problems[i] = [];

		foreach (var operands in input.Take(input.Length - 1).Select(x => x.ToNumArray()))
			for (var pId = 0; pId < operands.Length; pId++)
				problems[pId].Add(operands[pId]); 
		
		var answer1 = problems
			.Select((x, i) => 
				ops[i] == '+' ?
					x.Sum() :
					x.Aggregate((a, b) => a * b))
			.Sum();

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}