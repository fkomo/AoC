using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2023_08;

[AoCPuzzle(Year = 2023, Day = 08, Answer1 = "17621", Answer2 = null, Skip = false)]
public class HauntedWasteland : PuzzleBase
{
	Dictionary<char, int> _dir = new()
	{
		{ 'L', 0 },
		{ 'R', 1 }
	};

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var instructions = input.First();
		var network = input.Skip(2).ToDictionary(x => x[..3], x => new string[] { x.Substring(7, 3), x.Substring(12, 3) });

		// part1
		var nodeId = "AAA";
		long answer1 = 0;
		for (var i = 0; i < instructions.Length; i++)
		{
			nodeId = network[nodeId][_dir[instructions[i]]];
			answer1++;
			if (nodeId == "ZZZ")
				break;

			if (i == instructions.Length - 1)
				i = -1;
		}

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}