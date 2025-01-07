using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_08;

[AoCPuzzle(Year = 2024, Day = 08, Answer1 = "344", Answer2 = "1182", Skip = false)]
public class ResonantCollinearity : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var city = new aab2i(v2i.Zero, new v2i(input.Length - 1));

		var antennaPairs = city
			.EnumPoints()
			.Where(x => input[x.Y][(int)x.X] != '.')
			.Select(x => (Name:input[x.Y][(int)x.X], Position:x))
			.GroupBy(x => x.Name)
			.Where(x => x.Count() > 1)
			.Select(x => x.Select(xx => xx.Position).ToArray())
			.EnumPairs()
			.ToArray();

		// part1
		var antinodesInCity = new HashSet<v2i>();
		foreach (var ap in antennaPairs)
		{
			var antinode = ap[1] + ap[1] - ap[0];
			if (city.Contains(antinode))
				antinodesInCity.Add(antinode);
		}
		var answer1 = antinodesInCity.Count;

		// part2
		foreach (var ap in antennaPairs)
		{
			var dist = ap[1] - ap[0];

			var antinode = ap[0] + dist;
			while (city.Contains(antinode))
			{
				antinodesInCity.Add(antinode);
				antinode += dist;
			}
		}
		var answer2 = antinodesInCity.Count;

		return (answer1.ToString(), answer2.ToString());
	}
}

public static class Extensions
{
	public static IEnumerable<v2i[]> EnumPairs(this IEnumerable<v2i[]> antennas)
	{
		foreach (var antennaPositions in antennas)
		{
			for (var a1 = 0; a1 < antennaPositions.Length; a1++)
				for (var a2 = 0; a2 < antennaPositions.Length; a2++)
				{
					if (a1 == a2)
						continue;

					yield return [antennaPositions[a1], antennaPositions[a2]];
				}
		}
	}
}