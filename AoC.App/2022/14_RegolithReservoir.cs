using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2022_14
{
	public class RegolithReservoir : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			var map = CreateMap(input); 
			long? answer1 = 0;
			while (true)
			{
				var result = AddSand(new(500, 0), map);
				if (!result.HasValue)
					break;

				answer1++;
			}

			// part2
			map = CreateMap(input, ground: true);
			long? answer2 = 0;
			while (true)
			{
				var result = AddSand(new(500, 0), map);
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
					l.Split(" -> ").Select(p => new v2i(int.Parse(p[..p.IndexOf(',')]), int.Parse(p[(p.IndexOf(',') + 1)..]))).ToArray())
				.ToArray();

			var dim = new v2i(lines.Max(l => l.Max(p => p.X)) * 2, lines.Max(l => l.Max(p => p.Y)) + 3);
			var map = new byte[dim.Y, dim.X];

			if (ground)
				for (var x = 0; x < map.GetLength(1); x++)
					map[dim.Y - 1, x] = (byte)'#';

			foreach (var line in lines)
			{
				for (var p = 0; p < line.Length - 1; p++)
				{
					if (line[p].X == line[p + 1].X)
					{
						var from = line[p].Y;
						var to = line[p + 1].Y;

						if (from > to)
						{
							var tmp = to;
							to = from;
							from = tmp;
						}

						for (var y = from; y <= to; y++)
							map[y, line[p].X] = (byte)'#';
					}
					else
					{
						var from = line[p].X;
						var to = line[p + 1].X;

						if (from > to)
						{
							var tmp = to;
							to = from;
							from = tmp;
						}

						for (var x = from; x <= to; x++)
							map[line[p].Y, x] = (byte)'#';
					}
				}
			}

			return map;
		}

		public static v2i? AddSand(v2i sand, byte[,] map)
		{
			while (true)
			{
				if (map[sand.Y, sand.X] != 0 || sand.Y == map.GetLength(0) - 1)
					break;

				if (map[sand.Y + 1, sand.X] == 0)
					sand.Y++;

				else
				{
					if (sand.X == 0)
						break;

					if (map[sand.Y + 1, sand.X - 1] == 0)
					{
						sand.Y++;
						sand.X--;
					}
					else
					{
						if (sand.X == map.GetLength(1) - 1)
							break;

						if (map[sand.Y + 1, sand.X + 1] == 0)
						{
							sand.Y++;
							sand.X++;
						}
						else
						{
							map[sand.Y, sand.X] = (byte)'o';
							return sand;
						}
					}
				}
			}

			return null;
		}
	}
}