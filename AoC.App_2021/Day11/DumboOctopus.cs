using FFMpegCore;
using System.Drawing;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day11
{
	internal class DumboOctopus : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
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

				//SaveMapToImage(map, i, "part1");

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

				//SaveMapToImage(map, i, "part2");

				if (answer2 == long.MaxValue)
				{
					for (var y = 0; y < input.Length; y++)
						for (var x = 0; x < input[0].Length; x++)
							if (map[y][x] == 0xff)
								map[y][x] = 0;
				}
			}

			//CreateVideoFromImages(Path.Combine(_workingDir, "output-part1.mp4"), "part1");
			//CreateVideoFromImages(Path.Combine(_workingDir, "output-part2.mp4"), "part2");

			return (answer1.ToString(), answer2.ToString());
		}

		private (int, int)[] _dir = new (int, int)[]
		{
			( -1, -1 ), ( 0, -1 ), ( 1, -1 ), ( 1, 0 ), ( 1, 1 ), ( 0, 1 ), ( -1, 1 ), ( -1, 0 )
		};

		private long Flash(byte[][] map, int x, int y, int x0, int y0)
		{
			long flashes = 1;
			map[y][x] = 0xff;

			for (var d = 0; d < _dir.Length; d++)
			{
				var x1 = x + _dir[d].Item1;
				var y1 = y + _dir[d].Item2;

				if ((x0 == x1 && y0 == y1) ||
					x1 < 0 || y1 < 0 || x1 == map[0].Length || y1 == map.Length || map[y1][x1] == 0xff)
					continue;

				map[y1][x1]++;
			}

			for (var d = 0; d < _dir.Length; d++)
			{
				var x1 = x + _dir[d].Item1;
				var y1 = y + _dir[d].Item2;

				if ((x0 == x1 && y0 == y1) ||
					x1 < 0 || y1 < 0 || x1 == map[0].Length || y1 == map.Length || map[y1][x1] == 0xff)
					continue;

				if (map[y1][x1] > 9)
					flashes += Flash(map, x1, y1, x, y);
			}

			return flashes;
		}

		private void SaveMapToImage(byte[][] map, int step, string fileNamePrefix)
		{
#pragma warning disable CA1416 // Validate platform compatibility
			var size = 32;
			int width = map[0].Length * size;
			int height = map.Length * size;

			using Bitmap bmp = new(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			using Graphics gfx = Graphics.FromImage(bmp);
			gfx.Clear(Color.Black);

			for (var y = 0; y < map.Length; y++)
				for (var x = 0; x < map[0].Length; x++)
				{
					var c = Math.Clamp(map[y][x] * 10, 0, 0xff);

					Point pt = new(x * size, y * size);
					Size sz = new(size, size);
					Rectangle rect = new(pt, sz);
					gfx.FillRectangle(
						new SolidBrush(Color.FromArgb(c, c, c)), 
						rect);
				}

			bmp.Save(Path.Combine(_workingDir, $"{fileNamePrefix}.{step.ToString("D3")}.png"));
#pragma warning restore CA1416 // Validate platform compatibility
		}

		private void CreateVideoFromImages(string output, string inputImagePrefix)
		{
			var outputFiles = Directory.EnumerateFiles(_workingDir, $"{inputImagePrefix}.*.png");
			FFMpeg.JoinImageSequence(output, frameRate: 60,
				outputFiles.Select(f => ImageInfo.FromPath(f)).ToArray());
			foreach (var outputFile in outputFiles)
				File.Delete(outputFile);
		}
	}
}
