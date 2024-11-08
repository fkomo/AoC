using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2017_11;

[AoCPuzzle(Year = 2017, Day = 11, Answer1 = "685", Answer2 = "1457", Skip = false)]
public class HexEd : PuzzleBase
{
	public readonly static v3i N = new(1, 0, -1);
	public readonly static v3i NE = new(0, 1, -1);
	public readonly static v3i SE = new(-1, 1, 0);
	public readonly static v3i S = new(-1, 0, 1);
	public readonly static v3i SW = new(0, -1, 1);
	public readonly static v3i NW = new(1, -1, 0);

	public readonly static v3i[] HexaGrid =
	[
		N,
		NE,
		SE,
		S,
		SW,
		NW
	];

	readonly static Dictionary<string, v3i> _hexaDirs = new Dictionary<string, v3i>()
	{
		{ "n", N },
		{ "ne", NE },
		{ "se", SE },
		{ "s", S },
		{ "sw", SW },
		{ "nw", NW },
	};

	public static long HexGridDistance(v3i a, v3i b) => v3i.ManhDistance(a, b) / 2;

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var path = input[0].Split(',').Select(x => _hexaDirs[x]);

		// part1
		var answer1 = HexGridDistance(v3i.Zero, path.Aggregate((x, y) => x + y));

		// part2
		var current = v3i.Zero;
		var maxDistance = long.MinValue;
		foreach (var step in path)
		{
			current += step;
			var dist = HexGridDistance(v3i.Zero, current);
			maxDistance = System.Math.Max(maxDistance, dist);
		}

		var answer2 = maxDistance;

		return (answer1.ToString(), answer2.ToString());
	}
}