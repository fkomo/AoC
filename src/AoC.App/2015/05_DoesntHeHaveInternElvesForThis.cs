using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_05
{
	[AoCPuzzle(Year = 2015, Day = 05, Answer1 = "255", Answer2 = "55")]
	public class DoesntHeHaveInternElvesForThis : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			// part1
			var vowels = "aeiou".ToArray();
			var doubleLetters = Enumerable.Range(0, 'z' - 'a' + 1).Select(i => $"{(char)('a' + i)}{(char)('a' + i)}").ToArray();
			var naughty = new string[]
			{
				"ab", "cd", "pq", "xy"
			};
			var answer1 = input.Count(s =>
				s.Count(c => vowels.Contains(c)) >= 3 && naughty.All(ns => !s.Contains(ns)) && doubleLetters.Any(dl => s.Contains(dl)));

			// part2
			var pairs = Alg.Combinatorics.PermutationsWithRep(Enumerable.Range(0, 'z' - 'a' + 1).Select(i => (char)('a' + i)), 2)
				.Select(p => new string(p.ToArray()))
				.ToArray();
			var answer2 = input.Count(s =>
			{
				var rule1 = false;
				foreach (var p in pairs)
				{
					var i1 = s.IndexOf(p);
					if (i1 == -1)
						continue;

					var i2 = s.LastIndexOf(p);
					if (i2 > i1 + 1)
					{
						rule1 = true;
						break;
					}
				}

				if (!rule1)
					return false;

				var rule2 = false;
				for (var i = 1; i < s.Length - 1; i++)
				{
					if (s[i - 1] == s[i + 1])
					{
						rule2 = true;
						break;
					}
				}

				if (!rule2)
					return false;

				return true;
			});

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
