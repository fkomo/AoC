using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2020_07
{
	public class HandyHaversacks : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var rules = input
				.Where(line => !line.Contains("no other"))
				.Select(line => line.Replace(" bags", null).Replace(" bag", null).Replace(".", null))
				.ToDictionary(
					x => x.Split(" contain ")[0],
					x => x[(x.IndexOf(" contain ") + " contain ".Length)..]
						.Split(", ")
							.Select(y => new KeyValuePair<string, int>(y[(y.IndexOf(' ') + 1)..], int.Parse(y[..y.IndexOf(' ')])))
							.ToDictionary(b => b.Key, b => b.Value));

			// part1
			long? answer1 = rules
				.Where(r => r.Key != "shiny gold")
				.Count(r => ContainsBagRec(rules, r.Key, "shiny gold"));

			// part2
			long? answer2 = CountBagsRec(rules, "shiny gold");

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static bool ContainsBagRec(Dictionary<string, Dictionary<string, int>> rules, string outterBag, string innerBag)
			=> (outterBag == innerBag) || 
				(rules.ContainsKey(outterBag) && rules[outterBag].Keys.Any(b => ContainsBagRec(rules, b, innerBag)));

		private static long CountBagsRec(Dictionary<string, Dictionary<string, int>> rules, string outterBag)
		{
			var result = 0L;
			foreach (var b in rules[outterBag])
			{
				var inner = 1L;
				if (rules.ContainsKey(b.Key))
				{
					inner = CountBagsRec(rules, b.Key);
					result += b.Value;
				}

				result += b.Value * inner;
			}

			return result;
		}
	}
}
