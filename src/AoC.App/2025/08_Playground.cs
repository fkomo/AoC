using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2025_08;

[AoCPuzzle(Year = 2025, Day = 08, Answer1 = "32103", Answer2 = "8133642976", Skip = false)]
public class Playground : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// sample
		//var top = 10;
		var top = 1000;

		var junctions = input.Select(x => new v3i(x.ToNumArray())).ToArray();

		var distances = new Dictionary<(int, int), long>();
		for (var j1 = 0; j1 < junctions.Length; j1++)
			for (var j2 = j1 + 1; j2 < junctions.Length; j2++)
				distances.Add((j1, j2), (junctions[j2] - junctions[j1]).Length2());

		// part1
		var ordered = distances.OrderBy(x => x.Value).ToArray();

		var circuits = new List<HashSet<int>>();
		foreach (var d in ordered.Take(top))
		{
			var c1 = circuits.SingleOrDefault(x => x.Contains(d.Key.Item1));
			var c2 = circuits.SingleOrDefault(x => x.Contains(d.Key.Item2));

			if (c1 != null && c2 == null)
				c1.Add(d.Key.Item2);
			else if (c1 == null && c2 != null)
				c2.Add(d.Key.Item1);
			else if (c1 == null && c2 == null)
				circuits.Add([d.Key.Item1, d.Key.Item2]);

			else if (c1 != c2)
			{
				foreach (var j in c2)
					c1.Add(j);

				circuits.Remove(c2);
			}
		}

		var answer1 = circuits
			.OrderByDescending(x => x.Count).Select(x => (long)x.Count)
			.Take(3)
			.Aggregate((a, b) => a * b);

		// part2
		var answer2 = 0L;
		foreach (var d in ordered.Skip(top))
		{
			var c1 = circuits.SingleOrDefault(x => x.Contains(d.Key.Item1));
			var c2 = circuits.SingleOrDefault(x => x.Contains(d.Key.Item2));

			if (c1 != null && c2 == null)
				c1.Add(d.Key.Item2);
			else if (c1 == null && c2 != null)
				c2.Add(d.Key.Item1);
			else if (c1 == null && c2 == null)
				circuits.Add([d.Key.Item1, d.Key.Item2]);

			else if (c1 != c2)
			{
				foreach (var j in c2)
					c1.Add(j);

				circuits.Remove(c2);
			}

			if (circuits.Count == 1 && circuits[0].Count == junctions.Length)
			{
				answer2 = junctions[d.Key.Item1].X * junctions[d.Key.Item2].X;
				break;
			}
		}

		return (answer1.ToString(), answer2.ToString());
	}
}