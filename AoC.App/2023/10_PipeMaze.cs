using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_10;

[AoCPuzzle(Year = 2023, Day = 10, Answer1 = "7102", Answer2 = null, Skip = false)]
public class PipeMaze : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var start = FindStart(input);

		// part1
		var loop = Array.Empty<v2i>();
		foreach (var dir in v2i.DownUpLeftRight)
			if (FindLoop(start, dir, input, out loop))
				break;

		var answer1 = loop.Length / 2;

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static readonly Dictionary<v2i, string> _pipes = new()
	{
		{ v2i.Up, "7|F" },
		{ v2i.Down, "J|L" },
		{ v2i.Left, "F-L" },
		{ v2i.Right, "J-7" },
	};

	static readonly Dictionary<(char, v2i), v2i> _nextStep = new()
	{
		{ ('J', v2i.Right), v2i.Down },
		{ ('J', v2i.Up), v2i.Left },

		{ ('L', v2i.Up), v2i.Right },
		{ ('L', v2i.Left), v2i.Down },

		{ ('F', v2i.Down), v2i.Right },
		{ ('F', v2i.Left), v2i.Up },

		{ ('7', v2i.Down), v2i.Left },
		{ ('7', v2i.Right), v2i.Up },

		{ ('-', v2i.Left), v2i.Left },
		{ ('-', v2i.Right), v2i.Right },

		{ ('|', v2i.Down), v2i.Down },
		{ ('|', v2i.Up), v2i.Up },
	};

	static bool FindLoop(v2i origin, v2i step, string[] map, out v2i[] loop)
	{
		loop = Array.Empty<v2i>();
		var next = origin + step;
		if (next.Y < 0 || next.X < 0)
			return false;

		var tile = map[next.Y][(int)next.X];
		if (!_pipes[step].Contains(tile))
			return false;

		var path = new List<v2i>
		{
			origin
		};

		while (tile != 'S')
		{
			if (path.Contains(next))
				return false;

			path.Add(next);
			step = _nextStep[(tile, step)];

			next += step;
			if (next.Y < 0 || next.X < 0)
				return false;

			tile = map[next.Y][(int)next.X];
		}

		loop = path.ToArray();
		return true;
	}

	static v2i FindStart(string[] input)
	{
		for (var y = 0; y < input.Length; y++)
			for (var x = 0; x < input[y].Length; x++)
				if (input[y][x] == 'S')
					return new v2i(x, y);

		throw new Exception("no start!");
	}
}