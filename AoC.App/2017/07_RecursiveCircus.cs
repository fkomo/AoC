using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2017_07;

[AoCPuzzle(Year = 2017, Day = 07, Answer1 = "hlqnsbe", Answer2 = "1993", Skip = false)]
public class RecursiveCircus : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var circus = input.ToDictionary(
			x => x.Split(' ')[0], 
			x =>
			{
				var s = x.Split(" -> ");
				return new Disc(long.Parse(x.Split(' ')[1][1..^1]), (s.Length == 1) ? [] : s[1].Split(", "));
			});

		Debug.Line($"nodes {circus.Keys.Count}");
		Debug.Line($"leafs {circus.Keys.Count(x => circus[x].Above.Length == 0)}");

		// part1
		var root = circus.Keys
			.Where(x => circus[x].Above.Length != 0)
			.Single(x => !circus.Values.Any(v => v.Above.Contains(x)));

		var answer1 = root;

		// part2
		_ = circus.FindCorrectWeight(root, out long answer2);

		return (answer1?.ToString(), answer2.ToString());
	}
}

record struct Disc(long Weight, string[] Above);

internal static class CircusExtensions
{
	public static long FindCorrectWeight(this Dictionary<string, Disc> circus, string current, out long correctWeight)
	{
		var w = circus[current].Weight;
		correctWeight = w;

		if (circus[current].Above.Length == 0)
			return w;

		var above = circus[current].Above.Select(x => (Tower: x, Weight: (long)0)).ToArray();
		for (var i = 0; i < above.Length; i++)
		{
			above[i].Weight = circus.FindCorrectWeight(above[i].Tower, out correctWeight);
			if (correctWeight != above[i].Weight)
				return -1;
		}

		if (above.Any(x => x.Weight != above[0].Weight))
		{
			var (Tower, Weight) = above.GroupBy(x => x.Weight).Single(x => x.Count() == 1).Single();
			var diff = above.First(x => x.Tower != Tower).Weight - Weight;

			correctWeight = circus[Tower].Weight + diff;
			return -1;
		}

		correctWeight = w + above.Sum(x => x.Weight);
		return correctWeight;
	}
}