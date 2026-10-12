using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_15;

[AoCPuzzle(Year = 2016, Day = 15, Answer1 = "122318", Answer2 = "3208583", Skip = false)]
public class TimingIsEverything : PuzzleBase
{
	private const int _positionsCount = 0;
	private const int _time = 1;
	private const int _startPosition = 2;

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var discs = input.Select(x => new v3i(x.ToNumArray().Skip(1).ToArray())).ToArray();

		var start = discs[0][_positionsCount] - (discs[0][_startPosition] + 1);
		var step = discs[0][_positionsCount];

		// part1
		long answer1 = start;
		while (!CheckOffset(answer1, discs))
			answer1 += step;

		// part2
		long answer2 = start;
		while (!CheckOffset(answer2, discs.Concat(new v3i[] { new(11, 0, 0) }).ToArray()))
			answer2 += step;

		return (answer1.ToString(), answer2.ToString());
	}

	public static bool CheckOffset(long offset, v3i[] discs)
	{
		for (var i = 0; i < discs.Length; i++)
			if ((discs[i][_startPosition] + offset + i + 1) % discs[i][_positionsCount] != 0)
				return false;

		return true;
	}
}