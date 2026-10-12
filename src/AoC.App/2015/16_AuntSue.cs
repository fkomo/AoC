using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2015_16
{
	[AoCPuzzle(Year = 2015, Day = 16, Answer1 = "213", Answer2 = "323")]
	public class AuntSue : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var sues = input.ToDictionary(
				i => i.ToNumArray()[0], 
				i => i[(i.IndexOf(": ") + 2)..].Split(", ")
					.ToDictionary(y => y[..y.IndexOf(":")], y => int.Parse(y[(y.IndexOf(' ') + 1)..])));

			var secretSue = new Dictionary<string, (int Count, bool? GtThan)>()
			{
				{ "cats", (7, true) },
				{ "trees", (3, true) },
				{ "pomeranians", (3, false) },
				{ "goldfish", (5, false) },
				{ "children", (3, null) },
				{ "samoyeds", (2, null) },
				{ "akitas", (0, null) },
				{ "vizslas", (0, null) },
				{ "cars", (2, null) },
				{ "perfumes", (1, null) },
			};

			// part1
			var answer1 = sues.Single(s => s.Value.All(t => secretSue[t.Key].Count == t.Value)).Key;

			// part2
			var ex = secretSue.Where(t => t.Value.GtThan == null).ToArray();
			var gt = secretSue.Where(t => t.Value.GtThan == true).ToArray();
			var ft = secretSue.Where(t => t.Value.GtThan == false).ToArray();
			var answer2 = sues.Single(s =>
			{
				if (ex.Any(t => s.Value.ContainsKey(t.Key) && s.Value[t.Key] != t.Value.Count) ||	// exact
					gt.Any(t => s.Value.ContainsKey(t.Key) && s.Value[t.Key] <= t.Value.Count) ||	// greater than
					ft.Any(t => s.Value.ContainsKey(t.Key) && s.Value[t.Key] >= t.Value.Count))		// fewer than
					return false;

				return true;
			}).Key;

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
