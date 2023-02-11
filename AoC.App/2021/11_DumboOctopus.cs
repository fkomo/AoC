using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2021_11
{
	internal class DumboOctopus : PuzzleBase
	{
		private static v2i[] _dir;

		protected override (string, string) SolvePuzzle(string[] input)
		{
			var map = input.Select(l => l.Select(c => (byte)(c - '0')).ToArray()).ToArray();

			_dir = new v2i[8];
			for (int nIdx = 0, x = -1; x <= 1; x++)
				for (int y = -1; y <= 1; y++)
				{
					if (x == 0 && y == 0)
						continue;

					_dir[nIdx++] = new(x, y);
				}

			// part1
			long answer1 = 0;
			var i = 0;
			while (i++ < 100)
			{
				for (var y = 0; y < input.Length; y++)
					for (var x = 0; x < input[0].Length; x++)
						map[y][x]++;

				var p = new v2i();
				for (p.Y = 0; p.Y < input.Length; p.Y++)
					for (p.X = 0; p.X < input[0].Length; p.X++)
						if (map[p.Y][p.X] > 9 && map[p.Y][p.X] != 0xff)
							answer1 += Flash(map, p, p);

				//Ujeby.Graphics.VideoOutput.SaveMapToImage(map, i, "part1", _workingDir);

				for (var y = 0; y < input.Length; y++)
					for (var x = 0; x < input[0].Length; x++)
						if (map[y][x] == 0xff)
							map[y][x] = 0;
			}

			// part2
			long answer2 = long.MaxValue;
			var n = input.Length * input[0].Length;
			map = input.Select(l => l.Select(c => (byte)(c - '0')).ToArray()).ToArray();
			i = 0;
			while (answer2 == long.MaxValue)
			{
				i++;
				for (var y = 0; y < input.Length; y++)
					for (var x = 0; x < input[0].Length; x++)
						map[y][x]++;

				var p = new v2i();
				for (p.Y = 0; p.Y < input.Length; p.Y++)
					for (p.X = 0; p.X < input[0].Length; p.X++)
						if (map[p.Y][p.X] > 9 && map[p.Y][p.X] != 0xff &&
							Flash(map, p, p) == n)
						{
							answer2 = i;
							break;
						}

				//Ujeby.Graphics.VideoOutput.SaveMapToImage(map, i, "part2", _workingDir);

				if (answer2 == long.MaxValue)
				{
					for (var y = 0; y < input.Length; y++)
						for (var x = 0; x < input[0].Length; x++)
							if (map[y][x] == 0xff)
								map[y][x] = 0;
				}
			}

			//Ujeby.Graphics.VideoOutput.CreateVideoFromImages(Path.Combine(_workingDir, "output-part1.mp4"), _workingDir, "part1");
			//Ujeby.Graphics.VideoOutput.CreateVideoFromImages(Path.Combine(_workingDir, "output-part2.mp4"), _workingDir, "part2");

			return (answer1.ToString(), answer2.ToString());
		}

		private long Flash(byte[][] map, v2i p, v2i p0)
		{
			long flashes = 1;
			map[p.Y][(int)p.X] = 0xff;

			foreach (var dir in _dir)
			{
				var p1 = p + dir;
				if ((p0 == p1) || p1.X < 0 || p1.Y < 0 || p1.X == map[0].Length || p1.Y == map.Length || map[p1.Y][(int)p1.X] == 0xff)
					continue;

				map[p1.Y][p1.X]++;
			}

			foreach (var dir in _dir)
			{
				var p1 = p + dir;
				if ((p0 == p1) || p1.X < 0 || p1.Y < 0 || p1.X == map[0].Length || p1.Y == map.Length || map[p1.Y][p1.X] == 0xff)
					continue;

				if (map[p1.Y][p1.X] > 9)
					flashes += Flash(map, p1, p);
			}

			return flashes;
		}
	}
}
