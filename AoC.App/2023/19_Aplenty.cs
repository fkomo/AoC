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

	record class Rule(string Result, int Prop = -1, char Cmp = '=', long Value = -1);

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
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
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
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