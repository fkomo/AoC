using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_19;

[AoCPuzzle(Year = 2016, Day = 19, Answer1 = "1841611", Answer2 = null, Skip = false)]
public class AnElephantNamedJoseph : PuzzleBase
{
	private const int _id = 0;
	private const int _presents = 1;

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var numOfElves = int.Parse(input.Single());

		var elves = Enumerable.Range(1, numOfElves).Select(x => new v2i(x, 1)).ToArray();
		while (elves.Length > 1)
		{
			if (elves.Length % 2 == 0)
			{
				var last = elves.Last();
				elves = elves.Where((x, i) => i % 2 == 0).ToArray();
				elves[^1][_presents] += last[_presents];
			}
			else
			{
				var first = elves.First();
				elves = elves.Where((x, i) => i > 0 && i % 2 == 0).ToArray();
				elves[^1][_presents] += first[_presents] * 2;
			}

			if (elves.Length == 1)
				break;

			for (var i = 0; i < elves.Length - 1; i++)
				elves[i][_presents] *= 2;
		}
		var answer1 = elves.Single()[_id];

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}