using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2017_03;

[AoCPuzzle(Year = 2017, Day = 03, Answer1 = "480", Answer2 = "349975", Skip = false)]
public class SpiralMemory : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var answer1 = GetSpiralLocation(long.Parse(input.Single())).ManhLength();

		// part2
		var answer2 = GetFirstSpiral8SumAfter(long.Parse(input.Single()));

		return (answer1.ToString(), answer2.ToString());
	}

	public static IEnumerable<v2i> EnumSpiral()
	{
		var dir = 0;
		var axis = 0;
		var border = new long[] { 0, 0, 0, 0 };

		var position = new v2i(0, 0);
		yield return position;

		for (var i = 1; ; dir = ++dir % v2i.RightDownLeftUp.Length, axis = ++axis % 2)
		{
			var rdlu = v2i.RightDownLeftUp[dir];

			border[dir] += rdlu[axis];
			var steps = System.Math.Abs(border[dir] - position[axis]);
			for (var step = 0; step < steps; step++, i++)
			{
				position += rdlu;
				yield return position;
			}
		}
	}

	private static v2i GetSpiralLocation(long spiralLength)
	{
		var spiralEnum = EnumSpiral().GetEnumerator();
		for (var i = 0; i < spiralLength; i++)
			spiralEnum.MoveNext();

		return spiralEnum.Current;
	}

	private static long GetFirstSpiral8SumAfter(long spiralSum)
	{
		var sumDirs = v2i.RightDownLeftUp.Concat(v2i.Corners).ToArray();

		var sums = new Dictionary<v2i, long>();
		foreach (var p in EnumSpiral())
		{
			var sum = sums.Where(kv => sumDirs.Select(d => d + kv.Key).Contains(p))
				.Sum(kv => kv.Value);

			if (sum == 0)
				sum = 1;

			sums.Add(p, sum);

			if (sum > spiralSum)
				break;
		}

		return sums.Last().Value;
	}
}