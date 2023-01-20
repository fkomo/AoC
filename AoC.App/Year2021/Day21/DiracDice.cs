using Ujeby.AoC.Common;
using Ujeby.Tools;

namespace Ujeby.AoC.App.Year2021.Day21
{
	public class DiracDice : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var position = input.Select(p => p.ToNumArray().Last()).ToArray();
			var score = new long[position.Length];

			// part1
			var diceRolls = 0L;
			var lastDiceRoll = 0;
			var winner = false;
			long? answer1 = null;
			while (!winner)
			{
				for (var ip = 0; ip < position.Length; ip++)
				{
					var roll = 0;
					for (var i = 0; i < 3; i++, diceRolls++)
					{
						var diceRoll = DeterministicDiceRoll(lastDiceRoll);
						lastDiceRoll = diceRoll;

						roll += diceRoll;
					}

					if (roll % 10 > 0)
					{
						position[ip] += roll;
						if (position[ip] % 10 == 0)
							position[ip] = 10;
						else if (position[ip] > 10)
							position[ip] = position[ip] % 10;
					}
					score[ip] += position[ip];

					//Debug.Line($"{diceRolls}: player#{ip + 1} pos={position[ip]}, score={score[ip]}");
					if (score[ip] >= 1000)
					{
						answer1 = score[(ip + 1) % position.Length] * diceRolls;
						winner = true;
						break;
					}
				}
			}

			// part2
			long? answer2 = null;

			return (answer1?.ToString(), answer2?.ToString());
		}

		protected static int DeterministicDiceRoll(int prevRoll)
		{
			var diceRoll = prevRoll + 1;
			if (diceRoll == 101)
				diceRoll = 1;

			return diceRoll;
		}
	}
}
