using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2021_25
{
	public class SeaCucumber : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var seaFloor = input.Select(line => line.ToCharArray()).ToArray();
			var size = new v2i(seaFloor[0].Length, seaFloor.Length);

			var map = new char[size.Y, (int)size.X];
			for (int y = 0; y < size.Y; y++)
				for (int x = 0; x < size.X; x++)
					map[y, x] = seaFloor[y][x];

			Debug.Line();
			DrawMap(map, "init");

			// part1
			long? answer1 = 0;
			var movement = true;
			while (movement)
			{
				movement = false;

				// east
				map = Move('>', new(1, 0), map, size, out bool eMovement);
				movement |= eMovement;

				//DrawMap(map, $"{answer1}e");

				// south
				map = Move('v', new(0, 1), map, size, out bool sMovement);
				movement |= sMovement;

				answer1++;
				//DrawMap(map, $"{answer1}");
			}

			return (answer1?.ToString(), "*");
		}

		private static char[,] Move(char seaCucumber, v2i dir, char[,] prev, v2i size, out bool movement)
		{
			movement = false;
			var next = prev.Clone() as char[,];

			var p = new v2i();
			for (; p.Y < size.Y; p.Y++)
				for (p.X = 0; p.X < size.X; p.X++)
				{
					if (prev[p.Y, p.X] != seaCucumber)
						continue;

					var p1 = (p + dir) % size;
					if (prev[p1.Y, p1.X] == '.')
					{
						next[p.Y, p.X] = '.';
						next[p1.Y, p1.X] = prev[p.Y, p.X];
						movement = true;
					}
				}

			return next;
		}

		private static void DrawMap(char[,] map, string step)
		{
			Debug.Line(step);
			for (int y = 0; y < map.GetLength(0); y++)
			{
				var line = string.Empty;
				for (int x = 0; x < map.GetLength(1); x++)
					line += map[y, x];

				Debug.Line(line);
			}
			Debug.Line();
		}
	}
}
