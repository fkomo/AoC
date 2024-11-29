using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2018_07;

[AoCPuzzle(Year = 2018, Day = 07, Answer1 = null, Answer2 = null, Skip = false)]
public class TheSumOfItsParts : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var relations = input
			.Select(x => (Prereq: x["Step ".Length], Id: x["Step F must be finished before step ".Length]))
			.ToArray();

		var steps = relations
			.GroupBy(x => x.Id)
			.ToDictionary(x => x.Key, x => x.Select(x => x.Prereq).ToArray());

		var stepsWitNoPrereq = relations.Select(x => x.Prereq).Distinct().Where(x => !steps.ContainsKey(x));
		foreach (var step in stepsWitNoPrereq)
			steps.Add(step, []);

		// TODO 2018/07

		// part1
		string answer1 = null;

		// part2
		string answer2 = null;

		return (answer1?.ToString(), answer2?.ToString());
	}
}