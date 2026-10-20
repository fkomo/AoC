using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2019_01;

[AoCPuzzle(Year = 2019, Day = 01, Answer1 = "3386686", Answer2 = "5077155", Skip = false)]
public class TheTyrannyOfTheRocketEquation : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var masses = input
			.Select(x => int.Parse(x))
			.ToArray();

		// part1
		var answer1 = masses.Sum(GetFuel);

		// part2
		var answer2 = masses.Sum(GetFuelRec);

        return (answer1.ToString(), answer2.ToString());
	}

	static int GetFuel(int mass) => mass / 3 - 2;

	static int GetFuelRec(int mass)
	{
        var fuel = GetFuel(mass);
        if (fuel <= 0)
			return 0;

		return fuel + GetFuelRec(fuel);
	}
}