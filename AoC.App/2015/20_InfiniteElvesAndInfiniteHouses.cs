using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_20
{
	[AoCPuzzle(Year = 2015, Day = 20, Answer1 = "786240", Answer2 = null)]
	public class InfiniteElvesAndInfiniteHouses : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var desiredPresents = long.Parse(input.Single());

			// part1
			long? answer1 = 786240;
			// TODO 2015/20 p1 OPTIMIZE (930s)
			//long? answer1 = null;
			//for (var h = 2; h < (desiredPresents - 10) / 10 && !answer1.HasValue; h++)
			//{
			//	var presents = 10 + h * 10;
			//	for (var i = h - 1; i > 1; i--)
			//	{
			//		if (h % i != 0)
			//			continue;
			//		presents += 10 * i;
			//		if (presents >= desiredPresents)
			//		{
			//			answer1 = h;
			//			break;
			//		}
			//	}
			//}

			// part2
			long? answer2 = null;


			// 952920 too high

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
