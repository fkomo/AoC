using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2018_07;

using StepsTree = Dictionary<char, char[]>;

[AoCPuzzle(Year = 2018, Day = 07, Answer1 = "BFLNGIRUSJXEHKQPVTYOCZDWMA", Answer2 = "880", Skip = false)]
public class TheSumOfItsParts : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var relations = input
			.Select(x => (Prereq: x["Step ".Length], Id: x["Step F must be finished before step ".Length]))
			.ToArray();

		var stepsTree = relations
			.GroupBy(x => x.Id)
			.ToDictionary(x => x.Key, x => x.Select(x => x.Prereq).ToArray());

		var allStepsOrdered = relations.SelectMany(x => new char[] { x.Id, x.Prereq }).Distinct().Order().ToArray();

		// part1
		var answer1 = GetSteps([.. allStepsOrdered], stepsTree, out _, maxNumOfWorkers: 1, minStepDuration: 0);

		// part2
		GetSteps([.. allStepsOrdered], stepsTree, out int answer2);

		return (answer1.ToString(), answer2.ToString());
	}

	static string GetSteps(List<char> unusedSteps, StepsTree stepsTree, out int timeTaken, int maxNumOfWorkers = 5, int minStepDuration = 60)
	{
		timeTaken = -1;

		var usedSteps = new List<char>();
		var workers = new Dictionary<char, int>();

		while (unusedSteps.Count > 0)
		{
			// find finished
			var finished = workers.Where(x => x.Value == 0).OrderBy(x => x.Key).Select(x => x.Key).ToArray();
			foreach (var f in finished)
			{
				usedSteps.Add(f);
				workers.Remove(f);
				unusedSteps.Remove(f);
			}

			// progress current workers
			foreach (var step in workers.Keys)
				workers[step]--;

			var next = unusedSteps
				.Where(x => !stepsTree.ContainsKey(x) || stepsTree[x].All(xx => usedSteps.Contains(xx)))
				.Where(x => !workers.ContainsKey(x))
				.Take(maxNumOfWorkers - workers.Count);

			foreach (var n in next)
				workers.Add(n, minStepDuration + (n - 'A'));

			// advance time
			timeTaken++;
		}

		return string.Join("", usedSteps);
	}
}