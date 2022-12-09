using System.Data;
using System.Text;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2021.Day14
{
	internal class ExtendedPolymerization : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var template = input[0];
			var rules = input.Skip(2).Select(s => s.Split(" -> "))
				.ToDictionary(s => s[0], s => s[1][0]);

			// part1
			long? answer1 = ProcessTemplate_Naive(template, 10, rules);

			// part2
			long? answer2 = ProcessTemplate_Turbo(template, 40, rules, input);

			return (answer1?.ToString(), answer2?.ToString());
		}

		/// <summary>
		/// System.OutOfMemoryException at steps >= 25
		/// </summary>
		/// <param name="template"></param>
		/// <param name="steps"></param>
		/// <param name="rules"></param>
		/// <returns></returns>
		private long ProcessTemplate_Naive(string template, int steps, Dictionary<string, char> rules)
		{
			Debug.Line();

			var sb = new StringBuilder();

			var result = template;

			for (var step = 0; step < steps; step++)
			{
				sb.Clear();
				for (var i = 0; i < result.Length - 1; i++)
				{
					var seq = result.Substring(i, 2);
					sb.Append(seq[0]);

					if (rules.TryGetValue(seq, out char mid))
						sb.Append(mid);
				}
				sb.Append(result.Last());
				result = sb.ToString();

				Debug.Line($"{step}: {result}", indent: 6);
			}
			Debug.Line();

			var quant = result.Distinct().ToDictionary(c => c, c => result.LongCount(rc => rc == c)).OrderByDescending(d => d.Value);
			return quant.First().Value - quant.Last().Value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="template"></param>
		/// <param name="steps"></param>
		/// <param name="rules"></param>
		/// <returns></returns>
		private long ProcessTemplate_Turbo(string template, int steps, Dictionary<string, char> rules, string[] input)
		{
			var quant = string.Join("", input).Replace(" -> ", string.Empty)
				.Distinct().ToDictionary(c => c, c => template.LongCount(tc => tc == c));

			var pairs = new Dictionary<string, long>();
			for (var i = 0; i < template.Length - 1; i++)
			{
				var seq = template.Substring(i, 2);
				if (!pairs.ContainsKey(seq))
					pairs.Add(seq, 1);
				else
					pairs[seq]++;
			}

			for (var step = 0; step < 40; step++)
			{
				var newPairs = pairs.ToDictionary(d => d.Key, d => d.Value);
				for (var p = 0; p < pairs.Count; p++)
				{
					var pair = pairs.ElementAt(p);

					// find rule for existing pair
					if (pair.Value == 0 || !rules.TryGetValue(pair.Key, out char mid))
						continue;

					// update/add 1st new pair
					var p1 = $"{pair.Key[0]}{mid}";
					if (!newPairs.ContainsKey(p1))
						newPairs.Add(p1, 0);
					newPairs[p1] += pair.Value;

					// update/add 2nd new pair
					var p2 = $"{mid}{pair.Key[1]}";
					if (!newPairs.ContainsKey(p2))
						newPairs.Add(p2, 0);
					newPairs[p2] += pair.Value;

					// increase new item count
					quant[mid] += pair.Value;

					// remove old pair
					newPairs[pair.Key] -= pair.Value;
				}

				pairs = newPairs.ToDictionary(d => d.Key, d => d.Value);

				Debug.Text($"{step}: ", indent: 6);
				foreach (var q in quant.OrderByDescending(d => d.Value))
					Debug.Text($"{q.Key}={q.Value} ");
				Debug.Line();
			}
			Debug.Line();

			var orderedQuant = quant.OrderByDescending(d => d.Value);
			return orderedQuant.First().Value - orderedQuant.Last().Value;
		}
	}
}
