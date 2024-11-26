using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_06;

[AoCPuzzle(Year = 2018, Day = 06, Answer1 = "5333", Answer2 = null, Skip = false)]
public class ChronalCoordinates : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var coords = input.Select((x,i) => (id: (char)('A' + i), cc: new v2i(x.ToNumArray()))).ToArray();
		var view = new aab2i(
			new v2i(coords.Min(x => x.cc.X), coords.Min(x => x.cc.Y)), 
			new v2i(coords.Max(x => x.cc.X), coords.Max(x => x.cc.Y)));

		// part1
		var possibleCoords = coords
			.Where(x => view.Min.X != x.cc.X && view.Max.X != x.cc.X && view.Min.Y != x.cc.Y && view.Max.Y != x.cc.Y)
			.Select(x => x.id)
			.ToArray();

		var areas = coords.ToDictionary(x => x.id, x => 1);
		foreach (var p in view.EnumPoints())
		{
			if (coords.Any(x => x.cc == p))
				continue;

			var distances = coords.ToDictionary(x => x.id, x => v2i.ManhDistance(p, x.cc)).OrderBy(x => x.Value).ToArray();
			if (distances[0].Value != distances[1].Value)
				areas[distances[0].Key]++;
		}

		var answer1 = areas.Where(x => possibleCoords.Contains(x.Key)).Max(x => x.Value);

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}