using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2025_04;

[AoCPuzzle(Year = 2025, Day = 04, Answer1 = "1424", Answer2 = "8727", Skip = false)]
public class PrintingDepartment : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToArray()).ToArray();
		var allPapers = map.EnumAll('@').ToArray();

		// part1
		var answer1 = allPapers
			.Count(x => v2i.PlusMinusOne.Count(xx => map.TryGet(xx + x, out char neighbour) && neighbour == '@') < 4);

		// part2
		var papers = allPapers
			.ToDictionary(x => x, x => v2i.PlusMinusOne.Count(xx => map.TryGet(xx + x, out char neighbour) && neighbour == '@'));
		
		var initialCount = papers.Count;

		var toRemove = papers.Where(x => x.Value < 4).ToArray();
		while (toRemove.Length > 0)
		{
			foreach (var rem in toRemove)
			{
				papers.Remove(rem.Key);

				// update neighbours
				foreach (var n in v2i.PlusMinusOne.Select(x => x + rem.Key))
				{
					if (!papers.ContainsKey(n))
						continue;

					papers[n]--;
					if (papers[n] <= 0)
						papers.Remove(n);
				}
			}

			toRemove = [.. papers.Where(x => x.Value < 4)];
		}

		var answer2 = initialCount - papers.Count;

		return (answer1.ToString(), answer2.ToString());
	}
}