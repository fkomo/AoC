using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_22;

[AoCPuzzle(Year = 2016, Day = 22, Answer1 = "987", Answer2 = null, Skip = false)]
public class GridComputing : PuzzleBase
{
	private const int _size = 2;
	private const int _used = 3;

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var nodes = input
			.Skip(2)
			.Select(x => new v4i(x.ToNumArray().Take(4).ToArray()))
			.ToArray();

		// part1
		var pairs = new HashSet<v2i>();
		for (var ai = 0; ai < nodes.Length - 1; ai++)
			for (var bi = ai + 1; bi < nodes.Length; bi++)
			{
				if ((nodes[ai][_used] > 0 && nodes[bi][_used] + nodes[ai][_used] < nodes[bi][_size]) || 
					(nodes[bi][_used] > 0 && nodes[ai][_used] + nodes[bi][_used] < nodes[ai][_size]))
					pairs.Add(new v2i(ai, bi));
			}
		var answer1 = pairs.Count;

		// part2
		//var gridSize = new v2i(nodes.Max(x => x.X), nodes.Max(x => x.Y));
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}