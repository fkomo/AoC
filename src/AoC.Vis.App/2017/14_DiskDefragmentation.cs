using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Extensions;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class DiskDefragmentation : AoCRunnable
	{
		char[][] _mem;

		readonly List<v2i[]> _regions = [];

		public override string Name => $"#14 {nameof(DiskDefragmentation)}";

		public DiskDefragmentation(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			_mem = AoC.App._2017_14.DiskDefragmentation.CreateMemMap(InputProvider.Read(AppSettings.InputDirectory, 2017, 14));

			Grid.MinorSize = 8;
			Grid.MoveCenter(new v2i(_mem.Length / 2 * Grid.MinorSize));
		}

		protected override void Update()
		{
			Progress();
		}

		void Progress()
		{
			for (var y = 0; y < _mem.Length; y++)
				for (var x = 0; x < _mem.Length; x++)
				{
					if (_mem[y][x] == '#' && !_regions.Any(xx => xx.Contains(new v2i(x, y))))
					{
						_regions.Add(_mem.FloodFillNonRecWithDistance(new v2i(x, y), v2i.DownUpLeftRight, '.', 'x').Keys.ToArray());
						return;
					}
				}
		}

		protected override void Render()
		{
			Grid.Draw(showMajor: false, showMinor: false, showAxis: false);

			foreach (var r in _regions)
				Grid.DrawCells(r, fill: new v4f(v3f.FromRGB(r.GetHashCode()), 1));

			Grid.DrawRect(v2i.Zero, new v2i(_mem.Length), border: new v4f(0, 0, 1, 1));

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"size: {_mem.Length}x{_mem.Length}")
				);

			base.Render();
		}

		protected override void LeftMouseDown()
		{
			_regions.Clear();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
