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

		// part1
		var elves = Enumerable.Range(1, numOfElves).Select(x => new v2i(x, 1)).ToArray();
		while (elves.Length > 1)
		{
			if (elves.Length % 2 == 0)
			{
				var lastPresents = elves[^1][_presents];
				elves = elves.Where((x, i) => i % 2 == 0).ToArray();
				elves[^1][_presents] += lastPresents;
			}
			else
			{
				var firstPresents = elves[0][_presents];
				elves = elves.Where((x, i) => i > 0 && i % 2 == 0).ToArray();
				elves[^1][_presents] += firstPresents * 2;
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