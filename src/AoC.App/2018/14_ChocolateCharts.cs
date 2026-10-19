using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2018_14;

[AoCPuzzle(Year = 2018, Day = 14, Answer1 = "1041411104", Answer2 = "20174745", Skip = false)]
public class ChocolateCharts : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var plan = int.Parse(input[0]);

		int[] elves = [0, 1];
		var scoreboard = new List<int>();
		scoreboard.AddRange([3, 7]);

		// part1
		var scoreLength = 10;
		while (scoreboard.Count < plan + scoreLength)
		{
			var newRecipes = CreateNewRecipes(scoreboard, elves, out elves);
			foreach (var r in newRecipes)
				scoreboard.Add(r);
		}

		var answer1 = new string([.. scoreboard.Skip(plan).Take(scoreLength).Select(x => (char)('0' + x))]);

		// part2
		elves = [0, 1];
		scoreboard = [3, 7];
		var scoreToFind = input[0].Select(x => x - '0').Reverse().ToArray();

		var scoreFound = false;
		while (!scoreFound)
		{
			foreach (var r in CreateNewRecipes(scoreboard, elves, out elves))
			{
				scoreboard.Add(r);
				if (CheckForScore(scoreboard, scoreToFind))
				{
					scoreFound = true;
					break;
				}
			}
		}

		var answer2 = scoreboard.Count - scoreToFind.Length;

		return (answer1?.ToString(), answer2.ToString());
	}

	static int[] CreateNewRecipes(List<int> scoreboard, int[] elves, out int[] newElves)
	{
		var newRecipe = scoreboard[elves[0]] + scoreboard[elves[1]];
		var newScoreboardCount = (scoreboard.Count + (newRecipe >= 10 ? 2 : 1));

		newElves = [
			(elves[0] + 1 + scoreboard[elves[0]]) % newScoreboardCount,
			(elves[1] + 1 + scoreboard[elves[1]]) % newScoreboardCount];

		if (newRecipe >= 10)
			return [newRecipe / 10, newRecipe % 10];

		return [newRecipe % 10];
	}

	static bool CheckForScore(List<int> scoreboard, int[] scoreToFindReversed)
	{
		if (scoreboard.Count < scoreToFindReversed.Length)
			return false;

		for (var i = 0; i < scoreToFindReversed.Length; i++)
		{
			if (scoreboard[scoreboard.Count - 1 - i] != scoreToFindReversed[i])
				return false;
		}

		return true;
	}
}