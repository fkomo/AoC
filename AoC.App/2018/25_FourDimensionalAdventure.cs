using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_25;

[AoCPuzzle(Year = 2018, Day = 25, Answer1 = "388", Answer2 = "*", Skip = false)]
public class FourDimensionalAdventure : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var constellations = input
			.Select(x => new List<v4i> { new(x.ToNumArray()) })
			.ToArray();

		// part1
		var noChange = false;
		while (!noChange)
		{
			noChange = true;

			for (var i = 0; i < constellations.Length; i++)
			{
				if (constellations[i].Count == 0)
					continue;

				foreach (var nIdx in constellations
					.Select((x, i) => (x, i))
					.Where(x => x.i != i && x.x.Any(xx => constellations[i].Any(xxx => v4i.ManhDistance(xx, xxx) <= 3)))
					.Select(x => x.i))
				{
					constellations[i].AddRange(constellations[nIdx]);
					constellations[nIdx].Clear();

					noChange = false;
				}
			}

			constellations = [.. constellations.Where(x => x.Count > 0)];
		}

		var answer1 = constellations.Length;

		// part2
		var answer2 = "*";

		return (answer1.ToString(), answer2.ToString());
	}
}