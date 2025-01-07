using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2023_09;

[AoCPuzzle(Year = 2023, Day = 09, Answer1 = "2005352194", Answer2 = "1077", Skip = false)]
public class MirageMaintenance : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var datasets = input.Select(x => x.ToNumArray())
			.ToArray();

		// part1 & part2
		long answer1 = 0;
		long answer2 = 0;

		for (var d = 0; d < datasets.Length; d++)
		{
			var diffs = new List<long[]>();

			var dataset = datasets[d];
			//Debug.Line(string.Join(" ", dataset.Select(x => x.ToString())));

			diffs.Add(dataset);
			while (dataset.Any(x => x != 0))
			{
				var newSet = new long[dataset.Length - 1];
				for (var i = 0; i < dataset.Length - 1; i++)
					newSet[i] = dataset[i + 1] - dataset[i];
				diffs.Add(newSet);
				dataset = newSet;
				//Debug.Line(string.Join(" ", dataset.Select(x => x.ToString())));
			}

			long prediction = 0;
			for (var i = diffs.Count - 2; i >= 0; i--)
				prediction = diffs[i].Last() + prediction;
			//Debug.Line($"frwrd: {prediction}");
			answer1 += prediction;

			prediction = 0;
			for (var i = diffs.Count - 2; i >= 0; i--)
				prediction = diffs[i][0] - prediction;
			//Debug.Line($"bckwrd: {prediction}");
			answer2 += prediction;
		}

		return (answer1.ToString(), answer2.ToString());
	}
}