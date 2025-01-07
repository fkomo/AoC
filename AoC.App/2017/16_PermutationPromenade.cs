using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2017_16;

[AoCPuzzle(Year = 2017, Day = 16, Answer1 = "fnloekigdmpajchb", Answer2 = "amkjepdhifolgncb", Skip = false)]
public class PermutationPromenade : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var programs = "abcdefghijklmnop".ToArray();
		var moves = input[0].Split(',').Select(x => new Move(x, x.ToNumArray())).ToArray();

		// part1
		programs = Dance(programs, moves);
		var answer1 = new string(programs);

		// part2
		string answer2 = null;
		var dhs = new HashSet<string>();
		var dances = new List<string>();

		programs = [.. "abcdefghijklmnop"];

		var danceRounds = 1_000_000_000L;
		for (long i = 0; i < danceRounds; i++)
		{
			programs = Dance(programs, moves);
			var s = new string(programs);

			Debug.Line($"{i,4}: {s}");

			if (dhs.Add(s))
			{
				dances.Add(s);
				continue;
			}
		
			answer2 = dances[(int)(danceRounds % i) - 1];
			break;
		}

		return (answer1.ToString(), answer2?.ToString());
	}

	record class Move(string Raw, long[] Parsed);

	static char[] Dance(char[] programs, Move[] moves)
	{
		var tmp = new char[programs.Length];

		foreach (var move in moves)
		{
			if (move.Raw[0] == 's')
			{
				var len = move.Parsed[0];

				// spin
				for (var i = 0; i < len; i++)
					tmp[i] = programs[programs.Length - len + i];
				for (var i = 0; i < programs.Length - len; i++)
					tmp[i + len] = programs[i];

				// copy
				for (var i = 0; i < programs.Length ; i++)
					programs[i] = tmp[i];
			}
			else
			{
				var swap = move.Parsed;
				if (move.Raw[0] == 'p')
					swap = [Array.IndexOf(programs, move.Raw[1]), Array.IndexOf(programs, move.Raw[3])];

				// swap
				(programs[swap[0]], programs[swap[1]]) = (programs[swap[1]], programs[swap[0]]);
			}
		}

		return programs;
	}
}