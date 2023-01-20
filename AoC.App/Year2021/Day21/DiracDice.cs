using Ujeby.AoC.Common;
using Ujeby.Tools;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2021.Day21
{
	internal struct GameState
	{
		public int Turn;
		public v2i Position;
		public v2i Score;

		public GameState(GameState prevState)
		{
			Turn = prevState.Turn + 1;
			Position = new();
			Score = new();
		}
	}

	public class DiracDice : PuzzleBase
	{
		private static readonly Dictionary<int, int> _allRolls = 
			Alg.Combinatorics.PermutationsWithRep(new int[] { 1, 2, 3 }, 3)
				.GroupBy(r => r.Sum())
				.ToDictionary(g => g.Key, g => g.Count());

		private static readonly int[] _allRollsList =
			Alg.Combinatorics.PermutationsWithRep(new int[] { 1, 2, 3 }, 3)
				.Select(r => r.Sum())
				//.Distinct()
				.ToArray();

		protected override (string, string) SolvePuzzle(string[] input)
		{
			var position = new v2i(input.Select(p => p.ToNumArray().Last()).ToArray());
			var score = new v2i();

			// part1
			var diceRolls = 0L;
			var lastDiceRoll = 0;
			long? answer1 = null;
			while (!answer1.HasValue)
			{
				for (var p = 0; p < 2; p++)
				{
					var roll = 0;
					for (var i = 0; i < 3; i++, diceRolls++)
					{
						var diceRoll = DeterministicDiceRoll(lastDiceRoll);
						lastDiceRoll = diceRoll;
						roll += diceRoll;
					}

					position[p] = MovePlayer(position[p], roll);
					score[p] += position[p];

					//Debug.Line($"{diceRolls}: player#{ip + 1} pos={position[ip]}, score={score[ip]}");
					if (score[p] < 1000)
						continue;

					answer1 = score[(p + 1) % 2] * diceRolls;
					break;
				}
			}

			// part2
			var wins = GameTurn(
				new GameState
				{
					Turn = 0,
					Position = new v2i(input.Select(p => p.ToNumArray().Last()).ToArray()),
					Score = new(),
				});
			long? answer2 = Math.Max(wins.X, wins.Y);

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long MovePlayer(long currentPosition, int step)
		{
			if (step % 10 > 0)
			{
				currentPosition += step;
				if (currentPosition % 10 == 0)
					currentPosition = 10;
				else if (currentPosition > 10)
					currentPosition %= 10;
			}
			return currentPosition;
		}

		private static int DeterministicDiceRoll(int prevRoll)
		{
			var diceRoll = prevRoll + 1;
			if (diceRoll == 101)
				diceRoll = 1;

			return diceRoll;
		}

		private static v2i GameTurn(GameState state)
		{
			var wins = new v2i();
			foreach (var p1Roll in _allRolls)
			{
				var nextState = new GameState(state);

				nextState.Position.X = MovePlayer(state.Position.X, p1Roll.Key);
				nextState.Score.X = state.Score.X + state.Position.X;
				if (nextState.Score.X > 20)
				{
					wins.X += p1Roll.Value;
					continue;
				}

				foreach (var p2Roll in _allRolls)
				{
					nextState.Position.Y = MovePlayer(state.Position.Y, p2Roll.Key);
					nextState.Score.Y = state.Score.Y + state.Position.Y;
					if (nextState.Score.Y > 20)
					{
						wins.Y += p2Roll.Value;
						continue;
					}

					wins += GameTurn(nextState) * p1Roll.Value * p2Roll.Value;
				}
			}

			return wins;
		}
	}
}
