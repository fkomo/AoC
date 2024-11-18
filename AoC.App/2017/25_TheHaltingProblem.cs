using Ujeby.AoC.Common;
using Ujeby.Tools.ArrayExtensions;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2017_25;

[AoCPuzzle(Year = 2017, Day = 25, Answer1 = "4769", Answer2 = "*", Skip = false)]
public class TheHaltingProblem : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var state = input[0][^2];
		var steps = input[1].ToNumArray()[0];

		var states = input.Skip(3).ToArray().Split(string.Empty)
			.ToDictionary(
				x => x[0][^2],
				x =>
				{
					var value = new byte[] { byte.Parse(x[2][^2].ToString()), byte.Parse(x[6][^2].ToString()) };
					var step = new int[] { x[3].EndsWith("right.") ? 1 : -1, x[7].EndsWith("right.") ? 1 : -1 };
					var state = new char[] { x[4][^2], x[8][^2] };
					return (value, step, state);
				});

		// part1
		var pos = 0L;
		var values = new Dictionary<long, byte>();
		while (steps-- > 0)
		{
			var s = states[state];

			values.TryGetValue(pos, out var value);

			// set new value - only store 1
			if (s.value[value] == 1)
				values[pos] = 1;
			else
				// remove if value is 0
				values.Remove(pos);

			// move
			pos += s.step[value];

			// change state
			state = s.state[value];
		}
		var answer1 = values.LongCount();

		// part2
		var answer2 = "*";

		return (answer1.ToString(), answer2?.ToString());
	}
}