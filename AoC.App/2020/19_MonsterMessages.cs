using Ujeby.AoC.Common;
using Ujeby.Extensions;

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
			Debug.Line($"{msgs.Length} messages");

			var allRules = input
				.Take(Array.IndexOf(input, string.Empty))
				.ToDictionary(x => x.ToNumArray()[0], x => x.EndsWith('\"') ?
					new Rule(Value: x[^2]) :
					new Rule(Child: x[x.IndexOf(':')..].Split('|').Select(a => a.ToNumArray()).ToArray())
				);
			Debug.Line($"{allRules.Count} rules");

			// part1
			Debug.Line();
			var answer1 = msgs.Count(x => Match(allRules, x).Result);

			//Debug.Line();
			//foreach (var rule in _cache)
			//	Debug.Line($"{rule.Key}: {string.Join(", ", rule.Value)}");
			//Debug.Line();

			// part2
			// TODO 2020/19 p2
			string answer2 = null;
			//allRules.Remove(8);
			//allRules.Add(8, new Rule(Child: new long[][]
			//{
			//	new long[] { 42 },
			//	new long[] { 42, 8 }
			//}));

			//allRules.Remove(11);
			//allRules.Add(11, new Rule(Child: new long[][]
			//{
			//	new long[] { 42, 31 },
			//	new long[] { 42, 11, 31 }
			//}));

			//Debug.Line();
			//var answer2 = msgs.Count(x => Match(allRules, x).Result);

			return (answer1.ToString(), answer2?.ToString());
		}

		static Dictionary<long, HashSet<string>> _cache = new();

		private static (bool Result, int Offset) Match(Dictionary<long, Rule> rules, string msg,
			int index = 0, long ruleId = 0, int recursion = 0)
		{
			if (recursion > msg.Length)
				return (false, 0);

			var rule = rules[ruleId];
			if (rule.Value.HasValue)
				return (index < msg.Length && msg[index] == rule.Value, 1);

			var offsetOrOk = 0;
			var orOk = false;
			for (var ior = 0; ior < rule.Child.Length; ior++)
			{
				var offset = 0;
				var andOk = true;
				for (var iand = 0; iand < rule.Child[ior].Length; iand++)
				{
					var match = Match(rules, msg, index: index + offset, ruleId: rule.Child[ior][iand], recursion: recursion + 1);
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
					offsetOrOk = offset;
					//break;
				}
			}

			if (ruleId == 0 && orOk)
			{
				if (offsetOrOk < msg.Length)
					orOk = false;
				else
					Debug.Line($"{msg}");
			}

			//if (orOk)
			//{
			//	if (!_cache.ContainsKey(ruleId))
			//		_cache.Add(ruleId, new HashSet<string>(new string[] { msg.Substring(index, offsetOrOk) }));
			//	else
			//		_cache[ruleId].Add(msg.Substring(index, offsetOrOk));
			//}

			return (orOk, offsetOrOk);
		}
	}
}
