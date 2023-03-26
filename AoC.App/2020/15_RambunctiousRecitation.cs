using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2020_15
{
	[AoCPuzzle(Year = 2020, Day = 15, Answer1 = "1238", Answer2 = "3745954")]
	public class RambunctiousRecitation : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var spoken = input.Single().ToNumArray();
			var lastSpokenPosition = spoken.ToDictionary(
				x => x,
				x => new int[] { Array.IndexOf(spoken, x) + 1 });

			// part1
			var answer1 = MemoryGame(spoken.Last(), lastSpokenPosition, spoken.Length + 1, 2020);

			// part2
			var answer2 =  MemoryGame(answer1, lastSpokenPosition, 2020 + 1, 30_000_000);

			return (answer1.ToString(), answer2.ToString());
		}

		public static long MemoryGame(long last, Dictionary<long, int[]> spoken, int from, int to)
		{
			for (var i = from; i <= to; i++)
			{
				var spokenBefore = spoken.TryGetValue(last, out int[] recentPositions);
				if (!spokenBefore || recentPositions.Length == 1)
					last = 0;
				else
					last = i - 1 - recentPositions.First();

				//Debug.Line($"turn #{i}: {last}");

				if (spoken.ContainsKey(last))
					spoken[last] = new int[] { spoken[last].Last(), i };
				else
					spoken.Add(last, new int[] { i });
			}

			return last;
		}
	}
}
