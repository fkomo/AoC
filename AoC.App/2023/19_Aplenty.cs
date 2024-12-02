using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_19;

[AoCPuzzle(Year = 2023, Day = 19, Answer1 = "389114", Answer2 = "125051049836302", Skip = false)]
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

	public record class Rule(string Result, int Prop = -1, char Cmp = '=', long Value = -1);

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var wf = CreateWorkflows(input);
		Debug.Line($"{wf.Count} workflows");

		wf = OptimizeWorkflows(wf);
		Debug.Line($"{wf.Count} workflows after optimization");

		// part1
		var ratings = input
			.Skip(Array.IndexOf(input, string.Empty) + 1)
			.Select(x => new v4i(x.ToNumArray()))
			.ToArray();
		Debug.Line($"{ratings.Length} ratings");

		var answer1 = ratings
			.Where(x => IsAccepted(x, wf))
			.Sum(x => x.X + x.Y + x.Z + x.W);

		// part2
		var answer2 = AllPossibleCombinations(wf);

		return (answer1.ToString(), answer2.ToString());
	}

	static long AllPossibleCombinations(Dictionary<string, Rule[]> wf)
	{
		var count = 0L;

		var queue = new Queue<(string Node, v4i From, v4i To)>();
		queue.Enqueue(("in", new v4i(1), new v4i(4000)));

		void AddToQueue(string node, v4i from, v4i to)
		{
			if (node == Accepted)
				count +=
					(to[_x] - from[_x] + 1) *
					(to[_m] - from[_m] + 1) *
					(to[_a] - from[_a] + 1) *
					(to[_s] - from[_s] + 1);

			else if (node != Rejected)
				queue.Enqueue((node, from, to));
		}

		while (queue.Any())
		{
			var (Node, From, To) = queue.Dequeue();
			foreach (var r in wf[Node])
			{
				switch (r.Cmp)
				{
					case '<':
						{
							var to = To;
							to[r.Prop] = r.Value - 1;

							AddToQueue(r.Result, From, to);
							From[r.Prop] = r.Value;
						}
						break;

					case '>':
						{
							var from = From;
							from[r.Prop] = r.Value + 1;

							AddToQueue(r.Result, from, To);
							To[r.Prop] = r.Value;
						}
						break;

					default:
						AddToQueue(r.Result, From, To);
						break;
				}
			}
		}

		return count;
	}

	public static Dictionary<string, Rule[]> CreateWorkflows(string[] input)
		=> input
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
									wf[key2][i] = wf[key2][i] with { Result = keyResult };
									finished = false;
								}
						}
					}

					wf.Remove(key);
					finished = false;
					break;
				}
			}
		}

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