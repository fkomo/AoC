using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_19;

[AoCPuzzle(Year = 2023, Day = 19, Answer1 = "389114", Answer2 = null, Skip = false)]
public class Aplenty : PuzzleBase
{
	const int _x = 0;
	const int _m = 1;
	const int _a = 2;
	const int _s = 3;

	static readonly Dictionary<char, int> _xmas = new()
	{
		{ 'x', _x },
		{ 'm', _m },
		{ 'a', _a },
		{ 's', _s },
	};

	const string Accepted = "A";
	const string Rejected = "R";

	public class Rule
	{
		public Rule(string result, int prop = -1, char cmp = '=', long value = -1)
		{
			Result = result;
			Prop = prop;
			Cmp = cmp;
			Value = value;
		}

		public string Result { get; set; }
		public int Prop { get; set; } = -1;
		public char Cmp { get; set; } = '=';
		public long Value { get; set; } = -1;
	}

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var workflows = CreateWorkflows(input);

		var ratings = input
			.Skip(Array.IndexOf(input, string.Empty) + 1)
			.Select(x => new v4i(x.ToNumArray()))
			.ToArray();
		Debug.Line($"{ratings.Length} ratings");

		// part1
		var answer1 = ratings
			.Where(x => IsAccepted(x, workflows))
			.Sum(x => x.X + x.Y + x.Z + x.W);

		// part2
		long answer2 = 0;
		var wf2 = OptimizeWorkflows(workflows);

		// TODO 2023/19 p2

		return (answer1.ToString(), answer2.ToString());
	}

	public static Dictionary<string, Rule[]> CreateWorkflows(string[] input)
	{
		var workflows = input
			.Take(Array.IndexOf(input, string.Empty))
			.ToDictionary(
				x => x[..x.IndexOf('{')],
				x => x[(x.IndexOf('{') + 1)..][..^1]
					.Split(',')
					.Select(r =>
					{
						if (r.Contains(':'))
							return new Rule(r[(r.IndexOf(':') + 1)..], _xmas[r[0]], r[1], r.ToNumArray()[0]);

						return new Rule(r);
					})
					.ToArray());
		Debug.Line($"{workflows.Count} workflows");

		return workflows;
	}

	public static Dictionary<string, Rule[]> OptimizeWorkflows(Dictionary<string, Rule[]> workflows)
	{
		var wf = workflows.ToDictionary(x => x.Key, x => x.Value.ToArray());

		var finished = false;
		while (!finished)
		{
			finished = true;
			foreach (var key in wf.Keys)
			{
				// combine multiple results into 1 (if they are all the same)
				var result = wf[key][0].Result;
				if (wf[key].All(x => x.Result == result))
				{
					wf[key] = new Rule[] { new Rule(result) };

					Debug.Line($"{key}: combine rules to {result}");
					finished = false;
				}

				if (wf[key].Length == 1 && wf[key][0].Cmp == '=')
				{
					var keyResult = wf[key][0].Result;
					foreach (var key2 in wf.Keys)
					{
						if (wf[key2].Any(r => r.Result == key))
						{
							for (var i = 0; i < wf[key2].Length; i++)
								if (wf[key2][i].Result == key)
								{
									Debug.Line($"{key2}: update {wf[key2][i].Result} -> {keyResult}");
									wf[key2][i].Result = keyResult;
									finished = false;
								}
						}
					}

					Debug.Line($"{key}: remove");
					wf.Remove(key);
					finished = false;
					break;
				}
			}
		}

		Debug.Line($"{wf.Count} optimized workflows!");
		return wf;
	}

	static bool IsAccepted(v4i rating, Dictionary<string, Rule[]> workflows)
	{
		var w = workflows["in"];
		while (w != null)
		{
			foreach (var rule in w)
			{
				if (rule.Cmp == '=')
				{
					if (rule.Result == Accepted)
						return true;
					else if (rule.Result == Rejected)
						return false;

					w = workflows[rule.Result];
					break;
				}
				else if (rule.Cmp == '<' && rating[rule.Prop] < rule.Value)
				{
					if (rule.Result == Accepted)
						return true;
					else if (rule.Result == Rejected)
						return false;

					w = workflows[rule.Result];
					break;
				}
				else if (rule.Cmp == '>' && rating[rule.Prop] > rule.Value)
				{
					if (rule.Result == Accepted)
						return true;
					else if (rule.Result == Rejected)
						return false;

					w = workflows[rule.Result];
					break;
				}
			}
		}

		return false;
    }
}