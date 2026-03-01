using System.Data;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2018_12;

[AoCPuzzle(Year = 2018, Day = 12, Answer1 = "2166", Answer2 = "2100000000061", Skip = false)]
public class SubterraneanSustainability : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var plants = input[0].Skip("initial state: ".Length).ToArray();
		var rules = input.Skip(2).Select(x => new Rule(x[..5], x[^1] == '#')).ToArray();

		// part1
		var plantsAfter = Grow([.. plants], rules, out long left);
		var answer1 = GetSum(plantsAfter, left);

		// part2
		plantsAfter = Grow([.. plants], rules, out left, generations: 5_000);
		
		// generations 5_000, 50_000, 500_000, ... produces the same plant configuration
		// only thery are moved further to the right - left is increasing by 10x (45_000, 450_000, 4_500_000, ...)
		var gen = 5_000L;
		var leftInc = 4_500L;
		while (gen != 50_000_000_000)
		{
			gen *= 10;
			leftInc *= 10;
			left += leftInc;
		}

		var answer2 = GetSum(plantsAfter, left);

		return (answer1.ToString(), answer2.ToString());
	}

	static string Grow(char[] plants, Rule[] rules, out long left, long generations = 20)
	{
		left = 0;

		var extPlants = Enumerable.Repeat('.', 5);
		for (var g = 0; g < generations; g++)
		{
			// extend to left and right
			plants = extPlants.Concat(plants).Concat(extPlants).ToArray();
			left -= extPlants.Count();

			// grow next generation
			var plants2 = Enumerable.Repeat('.', plants.Length).ToArray();
			for (var p = 2; p < plants.Length - 2; p++)
			{
				var rule = rules.SingleOrDefault(x => MatchPatternAt(plants, p, x.Pattern));
				if (rule != null)
					plants2[p] = rule.Grow ? '#' : '.';
			}

			// trim left
			var first = Array.IndexOf(plants2, '#');
			left += first;
			plants = plants2[first..];

			// trim right
			var last = Array.LastIndexOf(plants, '#');
			plants = plants[..(last + 1)];
		}

		Debug.Line($"gen={generations} left={left}");
		Debug.Line(string.Join("", plants));

		return string.Join("", plants);
	}

	static long GetSum(string plants, long left)
	{
		var sum = 0L;
		for (var i = 0; i < plants.Length; i++)
			if (plants[i] == '#')
				sum += left + i;

		return sum;
	}

	static bool MatchPatternAt(char[] plants, int p, string pattern)
	{
		for (var i = 0; i < pattern.Length; i++)
			if (plants[p - 2 + i] != pattern[i])
				return false;

		return true;
	}
}

record class Rule(string Pattern, bool Grow);