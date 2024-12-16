using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Grid.CharMapExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_15;

[AoCPuzzle(Year = 2024, Day = 15, Answer1 = "1479679", Answer2 = "1509780", Skip = false)]
public class WarehouseWoes : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var split = input.Split(string.Empty);

		var map = split[0].Select(x => x.ToArray()).ToArray();
		var steps = split[1].SelectMany(x => x.Select(xx => _dirs[xx]).ToArray()).ToArray();

		// part1
		map = MoveRobot(map, steps);
		var answer1 = map.EnumAll('O').Select(x => x.Y * 100 + x.X).Sum();

		// part2
		map = EnlargeMap(split[0].Select(x => x.ToArray()).ToArray());
		map = MoveRobot2(map, steps);
		var answer2 = map.EnumAll('[').Select(x => x.Y * 100 + x.X).Sum();

		return (answer1.ToString(), answer2.ToString());
	}

	static char[][] MoveRobot(char[][] map, v2i[] steps)
	{
		map.Find('@', out v2i pos);

		foreach (var step in steps)
		{
			var pos2 = pos + step;
			var at = map.Get(pos2);

			if (at == '#')
				continue;

			else if (at == '.')
			{
				map.Set(pos, '.');
				map.Set(pos2, '@');
				pos += step;
			}
			else // O
			{
				// check if crates can be pushed
				var push = 0;
				for (var i = 1; map.Get(pos2 + step * i) != '#'; i++)
				{
					if (map.Get(pos2 + step * i) == '.')
					{
						push = i;
						break;
					}
				}

				if (push == 0)
					continue;

				map.Set(pos, '.');
				map.Set(pos2, '@');

				// push crates
				for (var i = 1; i <= push; i++)
					map[pos2.Y + push * step.Y][pos2.X + push * step.X] = 'O';

				pos = pos2;
			}
		}

		return map;
	}

	static char[][] MoveRobot2(char[][] map, v2i[] steps)
	{
		map.Find('@', out v2i pos);

		foreach (var step in steps)
		{
			var pos2 = pos + step;
			var at = map.Get(pos2);

			if (at == '#')
				continue;

			else if (at == '.')
			{
				map.Set(pos, '.');
				map.Set(pos2, '@');
				pos = pos2;
			}
			else // wide crate []
			{
				// horizontal push
				if (step.Y == 0)
				{
					// check if crates can be pushed
					var push = 0;
					for (var i = 2; map.Get(pos2 + step * i) != '#'; i++)
					{
						if (map.Get(pos2 + step * i) == '.')
						{
							push = i;
							break;
						}
					}

					if (push == 0)
						continue;

					// push crates
					for (var i = push - 1; i >= 0; i--)
						map.Swap(new v2i(pos2.X + step.X * i, pos2.Y), new v2i(pos2.X + step.X * (i + 1), pos2.Y));

					map.Set(pos, '.');
					map.Set(pos2, '@');
					pos = pos2;
				}
				// vertical push
				else
				{
					var crate = pos + step;
					if (map.Get(crate) == ']')
						crate.X--;

					var toPush = new List<v2i>() { crate };
					if (TryPushCrates(map, crate, step, toPush)) // check if group of crates can be pushed
					{
						// push crates
						var ordered = step.Y < 0 ? toPush.OrderBy(x => x.Y) : toPush.OrderByDescending(x => x.Y);
						foreach (var c in ordered)
						{
							map.Set(c + step, '[');
							map.Set(c + step + v2i.Right, ']');
							map.Set(c, '.');
							map.Set(c + v2i.Right, '.');
						}

						map.Set(pos, '.');
						map.Set(pos2, '@');
						pos = pos2;
					}
				}
			}
		}

		return map;
	}

	static bool TryPushCrates(char[][] map, v2i crate, v2i step, List<v2i> toPush)
	{
		var nextCrate = crate + step;
		if (map.Get(nextCrate) == '.' && map.Get(nextCrate + v2i.Right) == '.')
			return true;

		if (map.Get(nextCrate) == '#' || map.Get(nextCrate + v2i.Right) == '#')
			return false;

		if (map.Get(crate + step) == '[')
		{
			if (!TryPushCrates(map, crate + step, step, toPush))
				return false;

			toPush.Add(crate + step);
			return true;
		}
		
		if (map.Get(crate + step) == ']')
		{
			if (!TryPushCrates(map, crate + step + v2i.Left, step, toPush))
				return false;

			toPush.Add(crate + step + v2i.Left);
		}

		if (map.Get(crate + step + v2i.Right) == '[')
		{
			if (!TryPushCrates(map, crate + step + v2i.Right, step, toPush))
				return false;

			toPush.Add(crate + step + v2i.Right);
		}

		// #
		return true;
	}

	static char[][] EnlargeMap(char[][] map)
	{
		var enlargedMap = new char[map.Length][];
		for (var y = 0; y < map.Length; y++)
		{
			enlargedMap[y] = new char[map[0].Length * 2];
			for (var x = 0; x < map[0].Length; x++)
			{
				var x2 = x * 2;
				switch (map[y][x])
				{
					case '#':
						enlargedMap[y][x2] = '#';
						enlargedMap[y][x2 + 1] = '#';
						break;
					case 'O':
						enlargedMap[y][x2] = '[';
						enlargedMap[y][x2 + 1] = ']';
						break;
					case '.':
						enlargedMap[y][x2] = '.';
						enlargedMap[y][x2 + 1] = '.';
						break;
					case '@':
						enlargedMap[y][x2] = '@';
						enlargedMap[y][x2 + 1] = '.';
						break;
				}
			}
		}

		return enlargedMap;
	}

	static readonly Dictionary<char, v2i> _dirs = new()
	{
		{ '^', v2i.Down },
		{ 'v', v2i.Up },
		{ '>', v2i.Right },
		{ '<', v2i.Left }
	};
}