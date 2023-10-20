using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2020_19
{
	public record Rule(long[][] Child = null, char? Value = null);

	[AoCPuzzle(Year = 2020, Day = 19, Answer1 = "109", Answer2 = null, Skip = false)]
	public class MonsterMessages : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var msgs = input
				.Skip(Array.IndexOf(input, string.Empty) + 1)
				.ToArray();

			var allRules = input
				.Take(Array.IndexOf(input, string.Empty))
				.ToDictionary(x => x.ToNumArray()[0], x => x.EndsWith('\"') ?
					new Rule(Value: x[^2]) :
					new Rule(Child: x[x.IndexOf(':')..].Split('|').Select(a => a.ToNumArray()).ToArray())
				);

			// part1
			Debug.Line();
			var answer1 = msgs.Count(x => Match(allRules, x).Result);

			Debug.Line();
			foreach (var rule in _cache)
			{
				Debug.Line($"{rule.Key}: {string.Join(", ", rule.Value)}");
				//foreach (var match in rule.Value)
				//	Debug.Line($"{match}", indent: 6);
			}
			Debug.Line();

			// part2
			allRules.Remove(8);
			allRules.Add(8, new Rule(Child: new long[][]
			{
				new long[] { 42 },
				new long[] { 42, 8 }
			}));

			allRules.Remove(11);
			allRules.Add(11, new Rule(Child: new long[][]
			{
				new long[] { 42, 31 },
				new long[] { 42, 11, 31 }
			}));

			var answer2 = 0;// msgs.Count(x => Match(allRules, x).Result);

			return (answer1.ToString(), answer2.ToString());
		}

		static Dictionary<long, HashSet<string>> _cache = new();

		private static (bool Result, int Offset) Match(Dictionary<long, Rule> rules, string msg,
			int index = 0, long ruleId = 0)
		{
			var rule = rules[ruleId];
			if (rule.Value.HasValue)
				return (msg[index] == rule.Value, 1);

			var offset = 0;
			var orOk = false;
			for (var ior = 0; ior < rule.Child.Length; ior++)
			{
				offset = 0;
				var andOk = true;
				for (var iand = 0; iand < rule.Child[ior].Length; iand++)
				{
					var match = Match(rules, msg, index: index + offset, ruleId: rule.Child[ior][iand]);
					if (!match.Result)
					{
						andOk = false;
						break;
					}
					offset += match.Offset;
				}

				if (andOk)
				{
					orOk = true;
					break;
				}
			}

			if (ruleId == 0)
			{
				if (offset < msg.Length)
					orOk = false;
				else
					Debug.Line($"{msg}");
			}

			if (orOk)
			{
				if (!_cache.ContainsKey(ruleId))
					_cache.Add(ruleId, new HashSet<string>(new string[] { msg.Substring(index, offset) }));
				else
					_cache[ruleId].Add(msg.Substring(index, offset));
			}

			return (orOk, offset);
		}
	}
}
