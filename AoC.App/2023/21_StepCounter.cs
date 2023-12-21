using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_21;

[AoCPuzzle(Year = 2023, Day = 21, Answer1 = "3751", Answer2 = null, Skip = false)]
public class StepCounter : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var start = GetStart(input);

		// part1
		var steps = 64;
		var plots0 = new HashSet<v2i>() { start };
		var plots1 = new HashSet<v2i>();
		for (var i = 1; i < steps + 1; i++)
		{
			plots1.Clear();
			foreach (var p0 in plots0)
			{
				foreach (var dir in v2i.UpDownLeftRight)
				{
					var p1 = p0 + dir;
					if (p1.X < 0 || p1.Y < 0 || p1.X >= input.Length || p1.Y >= input.Length)
						continue;

					if (input[p1.Y][(int)p1.X] == '#')
						continue;

					plots1.Add(p1);
				}
			}

			plots0 = new HashSet<v2i>(plots1);
		}
		var answer1 = plots1.Count;

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static v2i GetStart(string[] input)
	{
		for (var y = 0; y < input.Length; y++)
			for (var x = 0; x < input.Length; x++)
				if (input[y][x] == 'S')
					return new v2i(x, y);

		return v2i.Zero;
	}
}