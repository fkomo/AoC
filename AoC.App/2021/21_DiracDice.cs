using Ujeby.AoC.Common;
using Ujeby.Tools;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2021_21
{
	public class DiracDice : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// 0 - 9 instead of 1 - 10
			var startPosition = new v2i(input.Select(p => p.ToNumArray().Last()).ToArray()) - 1;

			// part1
			var diceRolls = 0L;
			var lastDiceRoll = 100;

			var score = new v2i();
			var position = startPosition;

			long? answer1 = null;
			while (!answer1.HasValue)
			{
				for (var p = 0; p < 2; p++)
				{
					var roll = 0;
					for (var i = 0; i < 3; i++, diceRolls++)
					{
						var diceRoll = RollDeterministicDice(lastDiceRoll);
						lastDiceRoll = diceRoll;
						roll += diceRoll;
					}

					position[p] = MovePlayer(position[p], roll);
					score[p] += position[p] + 1;

					//Debug.Line($"{diceRolls}: player#{ip + 1} pos={position[ip]}, score={score[ip]}");
					if (score[p] < 1000)
						continue;

					answer1 = score[(p + 1) % 2] * diceRolls;
					break;
				}
			}

			// part2
			var wins = PlayerTurn(startPosition, new());
			long? answer2 = Math.Max(wins.X, wins.Y);

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long MovePlayer(long currentPosition, int step)
			=> (currentPosition + step) % 10;

		private static int RollDeterministicDice(int prevRoll)
			=> (prevRoll % 100) + 1;

		private static Dictionary<string, v2i> _turnCache = new();

		private static readonly Dictionary<int, int> _allRolls =
			Alg.Combinatorics.PermutationsWithRep(new int[] { 1, 2, 3 }, 3)
				.GroupBy(r => r.Sum())
				.ToDictionary(g => g.Key, g => g.Count());

		private static v2i PlayerTurn(v2i position, v2i score, 
			long paths = 1, int pIdx = 0)
		{
			var cacheKey = $"{position}:{score}:{paths}:{pIdx}";
			if (_turnCache.TryGetValue(cacheKey, out v2i cachedWins))
				return cachedWins;

			var wins = new v2i();
			foreach (var roll in _allRolls)
			{
				var newScore = score;
				var newPosition = position;

				newPosition[pIdx] = MovePlayer(position[pIdx], roll.Key);
				newScore[pIdx] = score[pIdx] + newPosition[pIdx] + 1;
				
				if (newScore[pIdx] > 20)
					wins[pIdx] += paths * roll.Value;
				else
					wins += PlayerTurn(newPosition, newScore, paths * roll.Value, (pIdx + 1) % 2);
			}

			_turnCache.Add(cacheKey, wins);
			return wins;
		}
	}
}
