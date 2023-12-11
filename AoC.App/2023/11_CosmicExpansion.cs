using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_11;

[AoCPuzzle(Year = 2023, Day = 11, Answer1 = "10494813", Answer2 = "840988812853", Skip = false)]
public class CosmicExpansion : PuzzleBase
{
	const int _origin = 0;
	const int _exp = 1;

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var galaxies = new Dictionary<int, v2i[]>();
		var emptyRows = new List<int>();
		for (var y = 0; y < input.Length; y++)
		{
			var empty = true;
			for (var x = 0; x < input[y].Length; x++)
			{
				if (input[y][x] == '#')
				{
					galaxies.Add(galaxies.Count + 1, new v2i[] { new v2i(x, y), new v2i(x, y) });
					empty = false;
				}
			}
			if (empty)
				emptyRows.Add(y);
		}

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
		Debug.Line($"{emptyRows.Count} empty rows");
		Debug.Line($"{emptyColumns.Count} empty columns");

		var pairs = Ujeby.Alg.Combinatorics.Combinations(galaxies.Keys, 2);
		Debug.Line($"{pairs.Length} pairs");

		// part1
		var agedGalaxies = ExpandUniverse(galaxies, emptyRows.ToArray(), emptyColumns.ToArray());
		var answer1 = pairs.Sum(x => v2i.ManhDistance(agedGalaxies[x.First()][_exp], agedGalaxies[x.Last()][_exp]));

		// part2
		agedGalaxies = ExpandUniverse(galaxies, emptyRows.ToArray(), emptyColumns.ToArray(), 
			age: 1000000);
		var answer2 = pairs.Sum(x => v2i.ManhDistance(agedGalaxies[x.First()][_exp], agedGalaxies[x.Last()][_exp]));

		return (answer1.ToString(), answer2.ToString());
	}

	private static Dictionary<int, v2i[]> ExpandUniverse(Dictionary<int, v2i[]> galaxies, int[] emptyRows, int[] emptyColumns, 
		int age = 2)
	{
		var aged = galaxies.ToDictionary(x => x.Key, x => x.Value.ToArray());

		foreach (var row in emptyRows)
			foreach (var g in aged.Keys)
				if (aged[g][_origin].Y > row)
					aged[g][_exp].Y += age - 1;
		foreach (var column in emptyColumns)
			foreach (var g in aged.Keys)
				if (aged[g][_origin].X > column)
					aged[g][_exp].X += age - 1;

		return aged;
	}
}