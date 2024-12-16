using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Grid.CharMapExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_15;

[AoCPuzzle(Year = 2024, Day = 15, Answer1 = "1479679", Answer2 = null, Skip = false)]
public class WarehouseWoes : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var split = input.Split(string.Empty);

		var map = split[0].Select(x => x.ToArray()).ToArray();
		PrintMap(map);

		var steps = split[1].SelectMany(x => x.Select(xx => _dirs[xx]).ToArray()).ToArray();
		Debug.Line($"{steps.Length} steps");

		// part1
		map = MoveRobot(map, steps);
		PrintMap(map); 
		
		var answer1 = map.EnumAll('O').Select(x => x.Y * 100 + x.X).Sum();

		// part2
		map = EnlargeMap(split[0].Select(x => x.ToArray()).ToArray());
		PrintMap(map);

		map = MoveRobot2(map, steps);
		PrintMap(map); 
		
		var answer2 = map.EnumAll('[').Select(x => x.Y * 100 + x.X).Sum();

		return (answer1.ToString(), answer2.ToString());
	}

	static char[][] MoveRobot(char[][] map, v2i[] steps)
	{
		map.Find('@', out v2i pos);

		foreach (var step in steps)
		{
			var pos2 = pos + step;
			var at = map.At(pos2);

			if (at == '#')
				continue;

			else if (at == '.')
			{
				map[pos.Y][pos.X] = '.';
				map[pos2.Y][pos2.X] = '@';
				pos += step;
			}
			else // O
			{
				var push = 0;
				for (var i = 1; map.At(pos2 + step * i) != '#'; i++)
				{
					if (map.At(pos2 + step * i) == '.')
					{
						push = i;
						break;
					}
				}

				if (push == 0)
					continue;

				map[pos.Y][pos.X] = '.';
				map[pos2.Y][pos2.X] = '@';
				for (var i = 1; i <= push; i++)
					map[pos2.Y + push * step.Y][pos2.X + push * step.X] = 'O';

				pos += step;
			}
		}

		return map;
	}

	static char[][] MoveRobot2(char[][] map, v2i[] steps)
	{
		map.Find('@', out v2i pos);

		foreach (var step in steps)
		{
			PrintMap(map);

			var pos2 = pos + step;
			var at = map.At(pos2);

			if (at == '#')
				continue;

			else if (at == '.')
			{
				map[pos.Y][pos.X] = '.';
				map[pos2.Y][pos2.X] = '@';
				pos = pos2;
			}
			else // []
			{
				if (step.Y == 0)
				{
					var push = 0;
					for (var i = 2; map.At(pos2 + step * i) != '#'; i++)
					{
						if (map.At(pos2 + step * i) == '.')
						{
							push = i;
							break;
						}
					}

					if (push == 0)
						continue;

					var to = pos.X + step.X * push;
					var from = pos2.X;
					if (from > to)
						(from, to) = (to, from);

					var toPush = map[pos2.Y].Skip((int)from).Take(push).ToArray();
					for (var i = 0; i < toPush.Length; i++)
						map[pos2.Y][from + i] = toPush[i];

					map[pos.Y][pos.X] = '.';
					map[pos2.Y][pos2.X] = '@';
					pos = pos2;
				}
				else
				{
					// TODO 
				}
			}
		}

		return map;
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

	static void PrintMap(char[][] map)
	{
		foreach (var line in map)
			Debug.Line(new string(line));

		Debug.Line();
	}

	readonly Dictionary<char, v2i> _dirs = new()
	{
		{ '^', v2i.Down },
		{ 'v', v2i.Up },
		{ '>', v2i.Right },
		{ '<', v2i.Left }
	};
}

static class Extensions
{
	public static char At(this char[][] map, v2i at) => map[at.Y][at.X];
}