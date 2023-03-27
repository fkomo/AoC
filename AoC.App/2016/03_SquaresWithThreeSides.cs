using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_03
{
	[AoCPuzzle(Year = 2016, Day = 03, Answer1 = "993", Answer2 = "1849")]
	public class SquaresWithThreeSides : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			// part1
			var answer1 = input.Select(x => new v3i(x.ToNumArray()))
				.Count(t => IsValidTriangle(t));

			// part2
			var answer2 = 0;
			for (var i = 0; i < input.Length; i += 3)
			{
				var r1 = input[i + 0].ToNumArray();
				var r2 = input[i + 1].ToNumArray();
				var r3 = input[i + 2].ToNumArray();

				if (IsValidTriangle(new v3i(r1[0], r2[0], r3[0])))
					answer2++;
				if (IsValidTriangle(new v3i(r1[1], r2[1], r3[1])))
					answer2++;
				if (IsValidTriangle(new v3i(r1[2], r2[2], r3[2])))
					answer2++;
			}

			return (answer1.ToString(), answer2.ToString());
		}

		private static bool IsValidTriangle(v3i t)
			=> (t.X < t.Y + t.Z && t.Y < t.X + t.Z && t.Z < t.Y + t.X);
	}
}
