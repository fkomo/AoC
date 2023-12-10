using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_10;

[AoCPuzzle(Year = 2023, Day = 10, Answer1 = "7102", Answer2 = "363", Skip = false)]
public class PipeMaze : PuzzleBase
{
	readonly static string _inOutTiles = "12";
	readonly static v2i[] _cornersAndSides = v2i.Corners.Concat(v2i.UpDownLeftRight).ToArray();

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToArray()).ToArray();

		var start = new v2i(-1);
		for (var y = 0; y < map.Length && start.X == -1; y++)
			for (var x = 0; x < map[y].Length; x++)
				if (map[y][x] == 'S')
				{
					start = new v2i(x, y);
					break;
				}
		Debug.Line($"mapSize: {map[0].Length}x{map.Length}");
		Debug.Line($"start: {start}");

		// part1
		var loop = Array.Empty<v2i>();
		foreach (var dir in v2i.DownUpLeftRight)
			if (FindLoop(start, dir, map, out loop))
				break;

		var answer1 = loop.Length / 2;

		// part2
		map = FixMap(map, loop);
		PrintMap(map);

		FillLeftRight(map, loop);
		PrintMap(map);

		var insideTile = FindInsideTile(map);
		var answer2 = map.Sum(x => x.Count(y => y == insideTile));

		return (answer1.ToString(), answer2.ToString());
	}

	static char FindInsideTile(char[][] map)
	{
		// first/last row
		for (var x = 0; x < map[0].Length; x++)
		{
			if (map[0][x] == _inOutTiles[0] || map.Last()[x] == _inOutTiles[0])
				return _inOutTiles[1];

			if (map[0][x] == _inOutTiles[1] || map.Last()[x] == _inOutTiles[1])
				return _inOutTiles[0];
		}

		// first/last column
		for (var y = 0; y < map.Length; y++)
		{
			if (map[y][0] == _inOutTiles[0] || map[y].Last() == _inOutTiles[0])
				return _inOutTiles[1];

			if (map[y][0] == _inOutTiles[1] || map[y].Last() == _inOutTiles[1])
				return _inOutTiles[0];
		}

		throw new Exception("never!");
	}

	static void FillLeftRight(char[][] map, v2i[] loop)
	{
		for (var i = 1; i < loop.Length; i++)
		{
			var tile = map[loop[i].Y][loop[i].X];

			var prevStep = loop[i] - loop[i - 1];
			var nextStep = _pipes[(tile, prevStep)];

			foreach (var s in _cornersAndSides)
			{
				var dest = s + loop[i];
				if (dest.X < 0 || dest.Y < 0 || dest.X == map[0].Length || dest.Y == map.Length)
					continue;

				if (map[dest.Y][(int)dest.X] != '.')
					continue;

				var left = nextStep.Sides.Contains(s);

				FloodFill(map, dest, left ? _inOutTiles[0] : _inOutTiles[1]);
			}
		}
	}

	/// <summary>
	/// fill empty space with specified tile
	/// </summary>
	/// <param name="map"></param>
	/// <param name="start"></param>
	/// <param name="tile"></param>
	static void FloodFill(char[][] map, v2i start, char tile)
	{
		if (start.X < 0 || start.Y < 0 || start.X == map[0].Length || start.Y == map.Length)
			return;

		if (map[start.Y][(int)start.X] == tile || map[start.Y][(int)start.X] != '.')
			return;

		map[start.Y][(int)start.X] = tile;
		foreach (var near in _cornersAndSides)
			FloodFill(map, start + near, tile);
	}

	/// <summary>
	/// erase all but main pipe loop
	/// </summary>
	/// <param name="map"></param>
	/// <param name="loop"></param>
	/// <returns></returns>
	static char[][] FixMap(char[][] map, v2i[] loop)
	{
		for (var y = 0; y < map.Length; y++)
			for (var x = 0; x < map[y].Length; x++)
				if (!loop.Contains(new v2i(x, y)))
					map[y][x] = '.';

		return map;
	}

	static void PrintMap(char[][] map)
	{
		for (var y = 0; y < map.Length; y++)
		{
			Debug.Line(new string(map[y])
				.Replace('F', '┌')
				.Replace('-', '─')
				.Replace('7', '┐')
				.Replace('J', '┘')
				.Replace('L', '└')
				);
		}
	}

	/// <summary>
	/// possible pipe tiles based on incoming direction
	/// </summary>
	static readonly Dictionary<v2i, string> _possiblePipeTiles = new()
	{
		{ v2i.Up, "7|F" },
		{ v2i.Down, "J|L" },
		{ v2i.Left, "F-L" },
		{ v2i.Right, "J-7" },
	};

	/// <summary>
	/// key: tile + incoming pipe direction 
	/// value: outgoing pipe direction + left side tiles
	/// </summary>
	static readonly Dictionary<(char Tile, v2i In), (v2i Out, v2i[] Sides)> _pipes = new()
	{
		{ ('J', v2i.Right), (v2i.Down, new v2i[] { new v2i(-1, -1) } ) },
		{ ('J', v2i.Up), (v2i.Left, new v2i[] { new v2i(1, -1), new v2i(1, 0), new v2i(1, 1), new v2i(0, 1), new v2i(-1, 1) } ) },

		{ ('L', v2i.Up), (v2i.Right, new v2i[] { new v2i(1, -1) } ) },
		{ ('L', v2i.Left), (v2i.Down, new v2i[] { new v2i(1, 1), new v2i(0, 1), new v2i(-1, 1), new v2i(-1, 0), new v2i(-1, -1), } ) },

		{ ('F', v2i.Down), (v2i.Right, new v2i[] { new v2i(-1, 1), new v2i(-1, 0), new v2i(-1, -1), new v2i(0, -1), new v2i(1, -1) } ) },
		{ ('F', v2i.Left), (v2i.Up, new v2i[] { new v2i(1, 1) } ) },

		{ ('7', v2i.Down), (v2i.Left, new v2i[] { new v2i(-1, 1) } ) },
		{ ('7', v2i.Right), (v2i.Up, new v2i[] { new v2i(-1, -1), new v2i(0, -1), new v2i(1, -1), new v2i(1, 0), new v2i(1, 1) } ) },

		{ ('-', v2i.Left), (v2i.Left, new v2i[] { new v2i(-1, 1), new v2i(0, 1), new v2i(1, 1) } ) },
		{ ('-', v2i.Right), (v2i.Right, new v2i[] { new v2i(-1, -1), new v2i(0, -1), new v2i(1, -1) } ) },

		{ ('|', v2i.Down), (v2i.Down, new v2i[] { new v2i(-1, -1), new v2i(-1, 0), new v2i(-1, 1) } ) },
		{ ('|', v2i.Up), (v2i.Up, new v2i[] { new v2i(1, -1), new v2i(1, 0), new v2i(1, 1) } ) },
	};

	static bool FindLoop(v2i origin, v2i step, char[][] map, out v2i[] loopPath)
	{
		loopPath = Array.Empty<v2i>();
		var next = origin + step;
		if (next.Y < 0 || next.X < 0)
			return false;

		var tile = map[next.Y][(int)next.X];
		if (!_possiblePipeTiles[step].Contains(tile))
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
			step = _pipes[(tile, step)].Out;

			next += step;
			if (next.Y < 0 || next.X < 0)
				return false;

			tile = map[next.Y][(int)next.X];
		}

		loopPath = path.ToArray();
		return true;
	}
}