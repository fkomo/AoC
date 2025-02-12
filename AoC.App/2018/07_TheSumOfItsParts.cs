using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2018_07;

[AoCPuzzle(Year = 2018, Day = 07, Answer1 = "BFLNGIRUSJXEHKQPVTYOCZDWMA", Answer2 = null, Skip = false)]
public class TheSumOfItsParts : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var relations = input
			.Select(x => (Prereq: x["Step ".Length], Id: x["Step F must be finished before step ".Length]))
			.ToArray();

		var stepsWithPrev = relations
			.GroupBy(x => x.Id)
			.ToDictionary(x => x.Key, x => x.Select(x => x.Prereq).ToArray());

		var usedSteps = new List<char>();
		var unusedSteps = relations.SelectMany(x => new char[] { x.Id, x.Prereq }).Distinct().Order().ToList();

		while (unusedSteps.Count > 0)
		{
			var next = unusedSteps.First(x => !stepsWithPrev.ContainsKey(x) || stepsWithPrev[x].All(xx => usedSteps.Contains(xx)));

			unusedSteps.Remove(next);
			usedSteps.Add(next);
		}

		// part1
		var answer1 = string.Join("", usedSteps);

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}