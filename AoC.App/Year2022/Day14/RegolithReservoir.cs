using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day14
{
	public class RegolithReservoir : PuzzleBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			// part1
			var map = CreateMap(input); 
			long? answer1 = 0;
			while (true)
			{
				var result = AddSand((500, 0), map);
				if (!result.HasValue)
					break;

				answer1++;
			}

			// part2
			map = CreateMap(input, ground: true);
			long? answer2 = 0;
			while (true)
			{
				var result = AddSand((500, 0), map);
				if (!result.HasValue)
					break;

				answer2++;
			}

			return (answer1?.ToString(), answer2?.ToString());
		}

		public static byte[,] CreateMap(string[] input, 
			bool ground = false)
		{
			var lines = input
				.Select(l =>
					l.Split(" -> ").Select(p => (x: int.Parse(p[..p.IndexOf(',')]), y: int.Parse(p[(p.IndexOf(',') + 1)..]))).ToArray())
				.ToArray();

			var dim = (x: lines.Max(l => l.Max(p => p.x)) * 2, y: lines.Max(l => l.Max(p => p.y)) + 3);
			var map = new byte[dim.y, dim.x];

			if (ground)
				for (var x = 0; x < map.GetLength(1); x++)
					map[dim.y - 1, x] = (byte)'#';

			foreach (var line in lines)
			{
				for (var p = 0; p < line.Length - 1; p++)
				{
					if (line[p].x == line[p + 1].x)
					{
						var from = line[p].y;
						var to = line[p + 1].y;

						if (from > to)
						{
							var tmp = to;
							to = from;
							from = tmp;
						}

						for (var y = from; y <= to; y++)
							map[y, line[p].x] = (byte)'#';
					}
					else
					{
						var from = line[p].x;
						var to = line[p + 1].x;

						if (from > to)
						{
							var tmp = to;
							to = from;
							from = tmp;
						}

						for (var x = from; x <= to; x++)
							map[line[p].y, x] = (byte)'#';
					}
				}
			}

			return map;
		}

		public static (int x, int y)? AddSand((int x, int y) sand, byte[,] map)
		{
			while (true)
			{
				if (map[sand.y, sand.x] != 0 || sand.y == map.GetLength(0) - 1)
					break;

				if (map[sand.y + 1, sand.x] == 0)
					sand.y++;

				else
				{
					if (sand.x == 0)
						break;

					if (map[sand.y + 1, sand.x - 1] == 0)
					{
						sand.y++;
						sand.x--;
					}
					else
					{
						if (sand.x == map.GetLength(1) - 1)
							break;

						if (map[sand.y + 1, sand.x + 1] == 0)
						{
							sand.y++;
							sand.x++;
						}
						else
						{
							map[sand.y, sand.x] = (byte)'o';
							return sand;
						}
					}
				}
			}

			return null;
		}
	}
}