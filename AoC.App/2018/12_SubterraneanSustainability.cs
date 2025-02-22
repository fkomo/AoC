using System.Data;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2018_12;

[AoCPuzzle(Year = 2018, Day = 12, Answer1 = "2166", Answer2 = null, Skip = false)]
public class SubterraneanSustainability : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var plants = input[0].Skip("initial state: ".Length).ToArray();
		var rules = input.Skip(2).Select(x => new Rule(x[..5], x[^1] == '#')).ToArray();

		// part1
		var answer1 = Grow([.. plants], rules, out _);

		// part2
		// 50 000 000 000 ?
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static long Grow(char[] plants, Rule[] rules, out int left, long generations = 20)
	{
		left = 0;

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

		//Debug.Line(string.Join("", plants));

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