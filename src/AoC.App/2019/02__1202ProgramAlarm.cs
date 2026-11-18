using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2019_02;

[AoCPuzzle(Year = 2019, Day = 02, Answer1 = "4945026", Answer2 = null, Skip = false)]
public class _1202ProgramAlarm : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var intcode = input.Single().ToNumArray();

		// part1
		intcode[1] = 12;
		intcode[2] = 2;

		var i = 0;
		while (i < intcode.Length && intcode[i] != 99)
		{
            intcode[intcode[i + 3]] = intcode[i] switch
            {
                1 => intcode[intcode[i + 1]] + intcode[intcode[i + 2]],
                2 => intcode[intcode[i + 1]] * intcode[intcode[i + 2]],
                _ => throw new NotImplementedException(intcode[i].ToString()),
            };

            i += 4;
		}

		var answer1 = intcode[0];

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}