using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_06;

[AoCPuzzle(Year = 2018, Day = 06, Answer1 = "5333", Answer2 = "35334", Skip = false)]
public class ChronalCoordinates : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var coords = input.Select((x,i) => (Name: (char)('A' + i), Pos: new v2i(x.ToNumArray()))).ToArray();
		var view = new aab2i(
			new v2i(coords.Min(x => x.Pos.X), coords.Min(x => x.Pos.Y)), 
			new v2i(coords.Max(x => x.Pos.X), coords.Max(x => x.Pos.Y)));
		var pointsInView = view.EnumPoints().ToArray();

		// part1
		var areas = coords.ToDictionary(x => x.Name, x => 1);
		foreach (var p in pointsInView)
		{
			if (coords.Any(x => x.Pos == p))
				continue;

			var distances = coords.ToDictionary(x => x.Name, x => v2i.ManhDistance(p, x.Pos)).OrderBy(x => x.Value).ToArray();
			if (distances[0].Value != distances[1].Value)
				areas[distances[0].Key]++;
		}
		
		var possibleCoords = coords
			.Where(x => view.Min.X != x.Pos.X && view.Max.X != x.Pos.X && view.Min.Y != x.Pos.Y && view.Max.Y != x.Pos.Y)
			.Select(x => x.Name)
			.ToArray(); 
		
		var answer1 = areas.Where(x => possibleCoords.Contains(x.Key)).Max(x => x.Value);

		// part2
		var answer2 = pointsInView.Count(x => coords.Sum(xx => v2i.ManhDistance(x, xx.Pos)) < 10_000);

		return (answer1.ToString(), answer2.ToString());
	}
}