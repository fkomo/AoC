using Ujeby.AoC.App.Year2022.Day09;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2021.Day11
{
	internal class DumboOctopus : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var map = input.Select(l => l.Select(c => (byte)(c - '0')).ToArray()).ToArray();

			// part1
			long answer1 = 0;
			var i = 0;
			while (i++ < 100)
			{
				for (var y = 0; y < input.Length; y++)
					for (var x = 0; x < input[0].Length; x++)
						map[y][x]++;

				for (var y = 0; y < input.Length; y++)
					for (var x = 0; x < input[0].Length; x++)
						if (map[y][x] > 9 && map[y][x] != 0xff)
							answer1 += Flash(map, x, y, x, y);

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

				for (var y = 0; y < input.Length; y++)
					for (var x = 0; x < input[0].Length; x++)
						if (map[y][x] > 9 && map[y][x] != 0xff &&
							Flash(map, x, y, x, y) == n)
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

		private long Flash(byte[][] map, int x, int y, int x0, int y0)
		{
			long flashes = 1;
			map[y][x] = 0xff;

			foreach (var dir in Directions.All.Values)
			{
				var x1 = x + dir[0];
				var y1 = y + dir[1];

				if ((x0 == x1 && y0 == y1) ||
					x1 < 0 || y1 < 0 || x1 == map[0].Length || y1 == map.Length || map[y1][x1] == 0xff)
					continue;

				map[y1][x1]++;
			}

			foreach (var dir in Directions.All.Values)
			{
				var x1 = x + dir[0];
				var y1 = y + dir[1];

				if ((x0 == x1 && y0 == y1) ||
					x1 < 0 || y1 < 0 || x1 == map[0].Length || y1 == map.Length || map[y1][x1] == 0xff)
					continue;

				if (map[y1][x1] > 9)
					flashes += Flash(map, x1, y1, x, y);
			}

			return flashes;
		}
	}
}
