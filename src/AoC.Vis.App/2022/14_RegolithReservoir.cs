using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class RegolithReservoir : AoCRunnable
	{
		byte[,] _map;

		Random _rnd;

		long _sandCount;
		const int _sandPerFrame = 10;

		List<v4f> _colors = new();

		public override string Name => $"#14 {nameof(RegolithReservoir)}";

		public RegolithReservoir(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			Sdl2Wrapper.ShowCursor(false);

			var input = InputProvider.Read(AppSettings.InputDirectory, 2022, 14);
			_map = AoC.App._2022_14.RegolithReservoir.CreateMap(input, ground: true);

			Grid.MinorSize = 5;
			Grid.MoveCenter(new v2i(_map.GetLength(1), _map.GetLength(0)) / 2 * Grid.MinorSize);

			_rnd = new Random(123);
			_sandCount = 0;
		}

		protected override void Update()
		{
			AddSand(new v2i(500, 0));
		}

		void AddSand(v2i p)
		{
			p.Y = System.Math.Max(p.Y, 0);
			if (p.X < 0 || p.Y < 0 || p.X >= _map.GetLength(1) || p.Y >= _map.GetLength(0))
				return;

			for (var i = 0; i < _sandPerFrame && AoC.App._2022_14.RegolithReservoir.AddSand(p, _map) != null; i++)
			{
				_colors.Add(new v4f(0.4 + _rnd.NextDouble() / 2, 0.3 + _rnd.NextDouble() / 2, 0, 0.8f));
				_sandCount++;
			}
		}

		protected override void Render()
		{
			Grid.Draw(showMinor: false);

			var blockerColor = new v4f(1, 1, 1, .7f);

			// height map
			var sandIdx = 0;
			for (var y = _map.GetLength(0) - 1; y >= 0 ; y--)
				for (var x = 0; x < _map.GetLength(1); x++)
				{
					if (_map[y, x] == 0)
						continue;

					var color = blockerColor;
					if (_map[y, x] == (byte)'o')
						color = _colors[sandIdx++];

					Grid.DrawCell(x, y, fill: color);
				}

			Grid.DrawMouseCursor(style: Graphics.GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"sand: {_sandCount}"));

			base.Render();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			AddSand(Grid.MousePositionDiscrete);
		}
	}
}
