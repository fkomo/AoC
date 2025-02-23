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
		var answer1 = Grow2([.. plants], rules);

		// part2
		var answer2 = Grow2([.. plants], rules, generations: 50_000);

		// 000000000
		// 0000000000
		// 640ms

		return (answer1.ToString(), answer2.ToString());
	}

	static long Grow2(char[] plants, Rule[] rules, long generations = 20)
	{
		var grown = new HashSet<long>();
		for (var i = 0; i < plants.Length; i++)
			if (plants[i] == '#')
				grown.Add(i);

		var emptyToPlant = rules.Where(x => x.Grow && x.Pattern[2] == '.').ToArray();
		var plantToPlant = rules.Where(x => x.Grow && x.Pattern[2] == '#').ToArray();

		var possiblePlants = new int[] { -2, -1, 1, 2 };

		for (var g = 0; g < generations; g++)
		{
			var grown2 = new List<long>();

			foreach (var p in grown)
			{
				foreach (var r in plantToPlant)
					if (MatchPattern(r.Pattern, grown, p))
						grown2.Add(p);
			}

			var empty = new HashSet<long>();
			foreach (var p in grown)
			{
				foreach (var pp in possiblePlants)
					if (!grown.Contains(pp + p))
						empty.Add(pp + p);
			}

			foreach (var p in empty)
			{
				foreach (var r in emptyToPlant)
					if (MatchPattern(r.Pattern, grown, p))
						grown2.Add(p);
			}

			grown = [.. grown2];
		}

		return grown.Sum();
	}

	static bool MatchPattern(string pattern, HashSet<long> grown, long p)
	{
		for (var i = 0; i < pattern.Length; i++)
		{
			if (pattern[i] == '#' && !grown.Contains(p - 2 + i))
				return false;

			if (pattern[i] == '.' && grown.Contains(p - 2 + i))
				return false;
		}

		return true;
	}

	static long Grow(char[] plants, Rule[] rules, long generations = 20)
	{
		var left = 0;

		var extPlants = Enumerable.Repeat('.', 5);
		for (var g = 0; g < generations; g++)
		{
			//Debug.Line($"{g}: {string.Join("", plants)}");

			// extend to left and right
			plants = extPlants.Concat(plants).Concat(extPlants).ToArray();
			left -= extPlants.Count();

			// grow next generation
			var plants2 = Enumerable.Repeat('.', plants.Length).ToArray();
			for (var p = 2; p < plants.Length - 2; p++)
			{
				foreach (var rule in rules)
				{
					if (MatchPatternAt(plants, p, rule.Pattern))
					{
						plants2[p] = rule.Grow ? '#' : '.';
						break;
					}
				}
			}

			// trim left
			var first = Array.IndexOf(plants2, '#');
			left += first;
			plants = plants2[first..];

			// trim right
			var last = Array.LastIndexOf(plants, '#');
			plants = plants[..(last + 1)];
		}

		Log.Line(string.Join("", plants));

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

record struct Rule(string Pattern, bool Grow);