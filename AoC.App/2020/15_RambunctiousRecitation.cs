using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

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
				x => new v2i(Array.IndexOf(spoken, x) + 1, -1));

			// part1
			var answer1 = MemoryGame(spoken.Last(), lastSpokenPosition, spoken.Length + 1, 2020);

			// part2
			var answer2 =  MemoryGame(answer1, lastSpokenPosition, 2020 + 1, 30_000_000);

			return (answer1.ToString(), answer2.ToString());
		}

		public static long MemoryGame(long last, Dictionary<long, v2i> spoken, long from, long to)
		{
			for (var i = from; i <= to; i++)
			{
				var spokenBefore = spoken.TryGetValue(last, out v2i recentPositions);
				if (!spokenBefore || recentPositions.Y == -1)
					last = 0;
				else
					last = i - 1 - recentPositions.X;

				if (spoken.TryGetValue(last, out v2i recentPositions2))
					spoken[last] = new v2i(recentPositions2.Y == -1 ? recentPositions2.X : recentPositions2.Y, i);
				else
					spoken.Add(last, new v2i(i, -1));
			}

			return last;
		}
	}
}
