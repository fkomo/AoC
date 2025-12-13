using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2025_09;

[AoCPuzzle(Year = 2025, Day = 09, Answer1 = "4735222687", Answer2 = null, Skip = false)]
public class MovieTheater : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var red = input.Select(x => new v2i([.. x.Split(',').Select(x => long.Parse(x))])).ToArray();

		// part1
		var answer1 = 0L;
		for (var r1 = 0; r1 < red.Length; r1++)
			for (var r2 = r1 + 1; r2 < red.Length; r2++)
			{
				var area = new aab2i([red[r1], red[r2]]).Size.Area();
				if (area > answer1) 
					answer1 = area;
			}

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}