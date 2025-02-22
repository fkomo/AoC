using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2018_09;

[AoCPuzzle(Year = 2018, Day = 09, Answer1 = "398242", Answer2 = "3273842452", Skip = true)]
public class MarbleMania : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var gameParams = input[0].ToNumArray();

		// part1
		var answer1 = GetHighScore((int)gameParams[_playerCount], gameParams[_lastMarble]);

		// part2
		// TODO OPTIMIZE 2018/09 p2 (2s)
		var answer2 = GetHighScore((int)gameParams[_playerCount], gameParams[_lastMarble] * 100);

		return (answer1.ToString(), answer2.ToString());
	}

	static int _playerCount = 0;
	static int _lastMarble = 1;

	static long GetHighScore(int players, long lastMarble)
	{
		var circle = new LinkedList<long>();

		var current = circle.AddFirst(0);
		current = circle.AddAfter(current, 1);
		current = circle.AddBefore(current, 2);

		var score = Enumerable.Range(0, players).ToDictionary(x => x, x => 0L);

		var player = 2;
		var currentMarble = 2L;
		while (currentMarble++ != lastMarble)
		{
			player = (player + 1) % players;

			if (currentMarble % 23 == 0)
			{
				score[player] += currentMarble;

				var toRemove = current;
				for (var i = 0; i < 7; i++)
				{
					if (toRemove.Previous == null)
						toRemove = circle.Last;
					else
						toRemove = toRemove.Previous;
				}
				current = toRemove.Next ?? circle.First;

				score[player] += toRemove.Value;
				circle.Remove(toRemove);
			}
			else
			{
				if (current.Next == null)
					current = circle.AddAfter(circle.First, currentMarble);
				else
					current = circle.AddAfter(current.Next, currentMarble);
			}
		}

		return score.Values.Max();
	}
}