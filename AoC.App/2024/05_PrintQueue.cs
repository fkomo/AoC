using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2024_05;

[AoCPuzzle(Year = 2024, Day = 05, Answer1 = "5166", Answer2 = "4679", Skip = false)]
public class PrintQueue : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var inputSplit = input.Split(string.Empty);

		var rawRules = inputSplit[0].Select(x => x.ToNumArray()).ToArray();
		var rules = rawRules.SelectMany(x => x).ToArray().Distinct()
			.ToDictionary(x => x, x => new Rule(
				rawRules.Where(xx => xx[1] == x).Select(xx => xx[0]).Distinct().ToArray(),
				rawRules.Where(xx => xx[0] == x).Select(xx => xx[1]).Distinct().ToArray()));

		var manuals = inputSplit[1].Select(x => x.ToNumArray()).ToArray();

		// part1
		var correctlyOrdered = manuals
			.Where(x => x.VerifyPageOrder(rules))
			.ToArray();

		var answer1 = correctlyOrdered
			.Select(x => x[x.Length / 2])
			.Sum();

		// part2
		var pageOrderComparer = new PageOrderComparer(rules);

		var answer2 = manuals.Except(correctlyOrdered)
			.Select(x => x.OrderBy(x => x, pageOrderComparer).ToArray())
			.Select(x => x[x.Length / 2])
			.Sum();

		return (answer1.ToString(), answer2.ToString());
	}
}

record class Rule(long[] Before, long[] After);

static class Extensions
{
	public static bool VerifyPageOrder(this long[] pages, Dictionary<long, Rule> rules)
	{
		for (var i = 0; i < pages.Length; i++)
		{
			if (!rules.TryGetValue(pages[i], out Rule rule))
				return false;

			if (rule.After.Any(x => pages.Contains(x) && Array.IndexOf(pages, x) < i))
				return false;

			if (rule.Before.Any(x => pages.Contains(x) && Array.IndexOf(pages, x) > i))
				return false;
		}

		return true;
	}
}

class PageOrderComparer(Dictionary<long, Rule> rules) : IComparer<long>
{
	public int Compare(long x, long y)
	{
		if (x == y)
			return 0;

		if (!rules.TryGetValue(x, out var xRule) || !rules.TryGetValue(y, out var yRule))
			return 0;

		if (xRule.After.Contains(y) || yRule.Before.Contains(x))
			return -1;

		if (xRule.Before.Contains(y) || yRule.After.Contains(x))
			return 1;

		return 0;
	}
}