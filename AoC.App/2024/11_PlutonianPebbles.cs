using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2024_11;

[AoCPuzzle(Year = 2024, Day = 11, Answer1 = "186203", Answer2 = null, Skip = false)]
public class PlutonianPebbles : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var stones = input.Single().ToNumArray();

		// part1
		// TODO 2024/11 p1 OPTIMIZE (1min)
		for (var i = 0; i < 25; i++)
		{
			Ujeby.Tools.Timer.Start("p1");
			
			stones = stones.Blink();
			
			Debug.Line($"{i}:" + Ujeby.Tools.Timer.Stop("p1").ToString());
		}

		var answer1 = stones.Length;

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}

static class Extensions
{
	public static long[] Blink(this long[] stones)
	{
		for (var s = stones.Length - 1; s >= 0; s--)
		{
			if (stones[s] == 0)
				stones[s] = 1;

			else if (stones[s].ToString().Length % 2 == 0)
			{
				var str = stones[s].ToString();

				var s1 = long.Parse(str[..(str.Length / 2)]);
				var s2 = long.Parse(str[(str.Length / 2)..]);

				stones[s] = s1;
				stones = stones.Take(s + 1).Concat([s2]).Concat(stones.Skip(s + 1)).ToArray();
			}
			else
				stones[s] *= 2024;
		}

		return stones;
	}
}