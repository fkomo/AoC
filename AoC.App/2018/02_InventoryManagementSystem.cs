using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2018_02;

[AoCPuzzle(Year = 2018, Day = 02, Answer1 = "6175", Answer2 = "asgwjcmzredihqoutcylvzinx", Skip = false)]
public class InventoryManagementSystem : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var grpd = input.Select(x => x.GroupBy(xx => xx)).ToArray();

		// part1
		var answer1 = grpd.Count(x => x.Any(xx => xx.Count() == 2)) * grpd.Count(x => x.Any(xx => xx.Count() == 3));

		// part2
		var hs = new HashSet<string>();
		for (var s1 = 0; s1 < input.Length; s1++)
		{
			for (var s2 = 0; s2 < s1; s2++)
			{
				if (CountDiff(input[s1], input[s2]) == 1)
				{
					hs.Add(input[s1]);
					hs.Add(input[s2]);
				}
			}
		}

		var final = hs.ToArray();
		var answer2 = string.Empty;
		for (var i = 0; i < final[0].Length; i++)
		{
			if (final[0][i] == final[1][i])
				answer2 += final[0][i];
		}

		return (answer1.ToString(), answer2.ToString());
	}

	static int CountDiff(string a, string b)
	{
		if (a.Length != b.Length)
			return -1;

		var cnt = 0;
		for (var i = 0; i < a.Length; i++)
		{
			if (a[i] != b[i])
				cnt++;
		}

		return cnt;
	}
}