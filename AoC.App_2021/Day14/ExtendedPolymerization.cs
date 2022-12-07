using System.Data;
using System.Text;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day14
{
	internal class ExtendedPolymerization : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var template = input[0];

			var rules = input.Skip(2).Select(s => s.Split(" -> "))
				.ToDictionary(s => s[0], s => s[1]);

			// part1
			long? answer1 = ProcessTemplate(template, 10, rules);

			// part2
			// TODO 2021/14 part2
			long? answer2 = null; // ProcessTemplate(template, 40, rules);

			return (answer1?.ToString(), answer2?.ToString());
		}

		/// <summary>
		/// System.OutOfMemoryException at steps >= 25
		/// </summary>
		/// <param name="template"></param>
		/// <param name="steps"></param>
		/// <param name="rules"></param>
		/// <returns></returns>
		private long ProcessTemplate(string template, int steps, Dictionary<string, string> rules)
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

					if (rules.TryGetValue(seq, out string mid))
						sb.Append(mid);
				}
				sb.Append(result.Last());
				result = sb.ToString();

				Debug.Line($"{step}: polymer length={result.Length}", indent: 6);
			}
			Debug.Line();

			var quant = result.Distinct().ToDictionary(c => c, c => result.LongCount(rc => rc == c)).OrderByDescending(d => d.Value);
			return quant.First().Value - quant.Last().Value;
		}
	}
}
