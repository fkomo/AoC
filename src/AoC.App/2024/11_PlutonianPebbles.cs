using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2024_11;

[AoCPuzzle(Year = 2024, Day = 11, Answer1 = "186203", Answer2 = "221291560078593", Skip = false)]
public class PlutonianPebbles : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var stones = input.Single().ToNumArray().ToDictionary(x => x, x => 1L);

		// part1
		for (var i = 0; i < 25; i++)
			stones = stones.Blink();
		var answer1 = stones.Sum(x => x.Value);

		// part2
		for (var i = 25; i < 75; i++)
			stones = stones.Blink();
		var answer2 = stones.Sum(x => x.Value);

		return (answer1.ToString(), answer2.ToString());
	}
}

static class Extensions
{
	public static Dictionary<long, long> Blink(this Dictionary<long, long> stones)
	{
		var stonesAfter = new Dictionary<long, long>();

		void AddStoneAfter(long newStone, long count)
		{
			if (!stonesAfter.TryAdd(newStone, count))
				stonesAfter[newStone] += count;
		}

		foreach (var stone in stones.Keys)
		{
			var stoneCount = stones[stone];

			if (stone == 0)
				AddStoneAfter(1, stoneCount);

			else if (stone.ToString().Length % 2 == 0)
			{
				var stoneStr = stone.ToString();

				AddStoneAfter(long.Parse(stoneStr[..(stoneStr.Length / 2)]), stoneCount);
				AddStoneAfter(long.Parse(stoneStr[(stoneStr.Length / 2)..]), stoneCount);
			}
			else
				AddStoneAfter(stone * 2024, stoneCount);
		}

		return stonesAfter;
	}
}