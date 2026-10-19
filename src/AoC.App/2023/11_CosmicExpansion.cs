using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_11;

[AoCPuzzle(Year = 2023, Day = 11, Answer1 = "10494813", Answer2 = "840988812853", Skip = false)]
public class CosmicExpansion : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var galaxies = new Dictionary<int, v2i>();
		for (var y = 0; y < input.Length; y++)
			for (var x = 0; x < input[y].Length; x++)
				if (input[y][x] == '#')
					galaxies.Add(galaxies.Count + 1, new v2i(x, y) );

		var emptyRows = input.Select((x, i) => x.Any(c => c != '.') ? -1 : i).Where(x => x != -1).ToArray();

		var emptyColumns = new List<int>();
		for (var x = 0; x < input[0].Length; x++)
		{
			var empty = true;
			for (var y = 0; y < input.Length; y++)
			{
				if (input[y][x] == '#')
				{
					empty = false;
					break;
				}
			}
			if (empty)
				emptyColumns.Add(x);
		}

		Debug.Line($"{input[0].Length}x{input.Length}: {galaxies.Count} galaxies");
		Debug.Line($"{emptyRows.Length} empty rows");
		Debug.Line($"{emptyColumns.Count} empty columns");

		var pairs = Ujeby.Alg.Combinatorics.Combinations(galaxies.Keys, 2)
			.Select(x => x.ToArray())
			.ToArray();
		Debug.Line($"{pairs.Length} pairs");

		// part1
		var agedGalaxies = ExpandUniverse(galaxies, emptyRows, emptyColumns.ToArray());
		var answer1 = pairs.Sum(x => v2i.ManhDistance(agedGalaxies[x[0]], agedGalaxies[x[1]]));

		// part2
		var evenMoreAgedGalaxies = ExpandUniverse(galaxies, emptyRows, emptyColumns.ToArray(), 
			age: 1000000);
		var answer2 = pairs.Sum(x => v2i.ManhDistance(evenMoreAgedGalaxies[x[0]], evenMoreAgedGalaxies[x[1]]));

		return (answer1.ToString(), answer2.ToString());
	}

	private static Dictionary<int, v2i> ExpandUniverse(Dictionary<int, v2i> galaxies, int[] emptyRows, int[] emptyColumns, 
		int age = 2)
		=> galaxies.ToDictionary(
			x => x.Key,
			x => x.Value + new v2i(emptyColumns.Count(e => e < x.Value.X), emptyRows.Count(e => e < x.Value.Y)) * (age - 1));
}