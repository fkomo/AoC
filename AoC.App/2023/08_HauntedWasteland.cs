using System.Xml.Linq;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2023_08;

[AoCPuzzle(Year = 2023, Day = 08, Answer1 = "17621", Answer2 = "20685524831999", Skip = false)]
public class HauntedWasteland : PuzzleBase
{
	readonly Dictionary<char, int> _dir = new()
	{
		{ 'L', 0 },
		{ 'R', 1 }
	};

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var instructions = input.First();
		var network = input.Skip(2).ToDictionary(x => x[..3], x => new string[] { x.Substring(7, 3), x.Substring(12, 3) });

		// part1
		long answer1 = 0;
		if (network.ContainsKey("AAA"))
		{
			string node = "AAA";
			for (int i = 0; node != "ZZZ"; i = (i + 1) % instructions.Length, answer1++)
				node = network[node][_dir[instructions[i]]];
		}

		// part2
		long answer2 = 1;
		var nodes = network.Keys.Where(x => x.EndsWith("A")).ToArray();
		foreach (var n in nodes)
		{
			var i = 0;
			var node = n;
			while (!node.EndsWith("Z"))
				node = network[node][_dir[instructions[i++ % instructions.Length]]];

			answer2 = Math.LeastCommonMultiple(answer2, i);
		}

		return (answer1.ToString(), answer2.ToString());
	}
}