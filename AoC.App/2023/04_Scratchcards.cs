using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2023_04;

[AoCPuzzle(Year = 2023, Day = 04, Answer1 = "21558", Answer2 = "10425665", Skip = false)]
public class Scratchcards : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var matches = input
			.Select(x =>
			{
				var card = x[x.IndexOf(':')..].Split('|');
				var winning = card[0].ToNumArray();
				var mine = card[1].ToNumArray();
				return winning.Count(x => mine.Contains(x));
			})
			.ToArray();

		// part1
		var answer1 = matches
			.Select(x => x > 0 ? System.Math.Pow(2, x - 1) : 0)
			.Sum();

		// part2
		var cards = new long[matches.Length];
		for (var i = 0; i < matches.Length; i++)
		{
			cards[i]++;
			if (matches[i] > 0)
			{
				for (var w = i + 1; w <= i + matches[i]; w++)
					cards[w] += cards[i];
			}

			Debug.Line(cards[i].ToString());
		}
		var answer2 = cards.Sum();

		return (answer1.ToString(), answer2.ToString());
	}
}