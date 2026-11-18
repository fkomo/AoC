using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2019_02;

[AoCPuzzle(Year = 2019, Day = 02, Answer1 = "4945026", Answer2 = "5296", Skip = false)]
public class _1202ProgramAlarm : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
    {
        var intcode = input.Single().ToNumArray();

        // part1
        var answer1 = Run([.. intcode], 12, 2);

        // part2
        var answer2 = FindNounAndVerb([.. intcode], 19690720);

        return (answer1.ToString(), answer2.ToString());
    }

    static long FindNounAndVerb(long[] intcode, int desiredOutput)
    {
        for (var n = 0; n <= 99; n++)
            for (var v = 0; v <= 99; v++)
            {
                var output = Run([.. intcode], n, v);
                if (output != desiredOutput)
                    continue;

                return 100 * n + v;
            }

        throw new Exception("should not happen");
    }

    static long Run(long[] intcode, long noun, long verb)
    {
        intcode[1] = noun;
        intcode[2] = verb;

        var i = 0;
        while (i < intcode.Length && intcode[i] != 99)
        {
            switch(intcode[i])
            {
                case 1: intcode[intcode[i + 3]] = intcode[intcode[i + 1]] + intcode[intcode[i + 2]]; break;
                case 2: intcode[intcode[i + 3]] = intcode[intcode[i + 1]] * intcode[intcode[i + 2]]; break;
                default: return -1;
            };

            i += 4;
        }

        return intcode[0];
    }
}