using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_22;

[AoCPuzzle(Year = 2018, Day = 22, Answer1 = "8681", Answer2 = null, Skip = false)]
public class ModeMaze : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var depth = input[0].ToNumArray()[0];
		var target = new v2i(input[1].ToNumArray());
		var area = new aab2i(v2i.Zero, target);

		// part1
		var erosionLevels = new long[target.Y + 1][];
		for (var i = 0; i < erosionLevels.Length; i++)
			erosionLevels[i] = new long[target.X + 1];

		foreach (var a in area.EnumPoints())
			erosionLevels.Set(a, GetErosionLevelAt(a, target, depth, erosionLevels));

		var answer1 = erosionLevels.Sum(x => x.Sum(xx => (depth + xx) % 3));

		// part2
		var map = CreateMap(erosionLevels, depth);

		foreach (var line in map)
			Debug.Line(new string(line));
		Debug.Line();

		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static long GetErosionLevelAt(v2i a, v2i target, long depth, long[][] erosionLevels)
	{
		var geoIdx = 0L;
		if (a == v2i.Zero || a == target)
			geoIdx = 0;
		else if (a.Y == 0)
			geoIdx = a.X * 16807;
		else if (a.X == 0)
			geoIdx = a.Y * 48271;
		else
			geoIdx = erosionLevels.Get(a + v2i.Left) * erosionLevels.Get(a + v2i.Down);

		return (geoIdx + depth) % 20183;
	}

	static char GetRegionType(long t) => t switch
	{
		0 => '.',
		1 => '=',
		2 => '|',
		_ => '\0',
	};

	static char[][] CreateMap(long[][] erosionLevels, long depth) => [.. erosionLevels.Select(x => x.Select(xx => GetRegionType((depth + xx) % 3)).ToArray())];
}