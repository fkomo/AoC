using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2020_01
{
	[AoCPuzzle(Year = 2020, Day = 01, Answer1 = "927684", Answer2 = "292093004")]
	public class ReportRepair : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var n = input.Select(l => long.Parse(l)).OrderBy(n => n).ToArray();

			// part1
			long? answer1 = null;
			for (var n1 = 0; n1 < n.Length && !answer1.HasValue; n1++)
			{
				for (var n2 = 0; n2 < n.Length && !answer1.HasValue; n2++)
				{
					if (n1 == n2)
						continue;

					if (n[n1] + n[n2] == 2020)
						answer1 = n[n1] * n[n2];
				}
			}

			// part2
			long? answer2 = null;
			for (var n1 = 0; n1 < n.Length && !answer2.HasValue; n1++)
			{
				for (var n2 = 0; n2 < n.Length && !answer2.HasValue; n2++)
				{
					if (n1 == n2)
						continue;

					for (var n3 = 0; n3 < n.Length && !answer2.HasValue; n3++)
					{
						if (n1 == n3 || n2 == n3)
							continue;

						if (n[n1] + n[n2] + n[n3] == 2020)
							answer2 = n[n1] * n[n2] * n[n3];
					}
				}
			}

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
