using System.Diagnostics;
using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Grid.CharMapExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class DiskDefragmentation : AoCRunnable
	{
		char[][] _mem;

		const int _frameStep = 16;
		readonly Stopwatch _sw = Stopwatch.StartNew();

		public override string Name => $"#14 {nameof(DiskDefragmentation)}";

		public DiskDefragmentation(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			_mem = AoC.App._2017_14.DiskDefragmentation.CreateMemMap(InputProvider.Read(AppSettings.InputDirectory, 2017, 14));
		}

		protected override void Update()
		{
			if (_sw.ElapsedMilliseconds >= _frameStep)
			{
				Progress();
				_sw.Restart();
			}
		}

		void Progress()
		{
			for (var y = 0; y < _mem.Length; y++)
			{
				for (var x = 0; x < _mem.Length; x++)
				{
					if (_mem[y][x] == '#')
					{
						_mem.FloodFill(new v2i(x, y), 'x', v2i.DownUpLeftRight, '.', 'x');
						return;
					}
				}
			}
		}

		protected override void Render()
		{
			Grid.Draw();

			var rndColor = new v4f(Random.Shared.NextDouble(), Random.Shared.NextDouble(), Random.Shared.NextDouble(), .5);

			var pixelColor = new v4f(.5);
			var imageSize = new v2i(_mem[0].Length, _mem.Length);

			var p = new v2i();
			for (; p.Y < imageSize.Y; p.Y++)
				for (p.X = 0; p.X < imageSize.X; p.X++)
				{
					if (_mem[p.Y][(int)p.X] == '.')
						continue;

					if (_mem[p.Y][(int)p.X] == '#')
						Grid.DrawCell(p - imageSize / 2, fill: pixelColor);

					else
						Grid.DrawCell(p - imageSize / 2, fill: rndColor);
				}

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"size: {_mem.Length}x{_mem.Length}")
				);

			base.Render();
		}

		protected override void LeftMouseDown()
		{
			Init();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
