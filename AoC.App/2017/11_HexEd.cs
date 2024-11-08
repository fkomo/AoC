using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2017_11;

[AoCPuzzle(Year = 2017, Day = 11, Answer1 = "685", Answer2 = "1457", Skip = false)]
public class HexEd : PuzzleBase
{
	readonly static Dictionary<string, v3i> _hexaSteps = new()
	{
		{ "n", v3i.N },
		{ "ne", v3i.NE },
		{ "se", v3i.SE },
		{ "s", v3i.S },
		{ "sw", v3i.SW },
		{ "nw", v3i.NW },
	};

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var path = input[0].Split(',').Select(x => _hexaSteps[x]);

		// part1
		var answer1 = v3i.HexGridDistance(v3i.Zero, path.Aggregate((x, y) => x + y));

		// part2
		var current = v3i.Zero;
		var maxDistance = long.MinValue;
		foreach (var step in path)
		{
			current += step;
			var dist = v3i.HexGridDistance(v3i.Zero, current);
			maxDistance = System.Math.Max(maxDistance, dist);
		}

		var answer2 = maxDistance;

		return (answer1.ToString(), answer2.ToString());
	}
}