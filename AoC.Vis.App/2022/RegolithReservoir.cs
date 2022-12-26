using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class RegolithReservoir : Sdl2Loop
	{
		private byte[,] _map;

		private Random _rnd;

		private long _sandCount;

		public RegolithReservoir(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(false);

			var input = new AoC.App.Year2022.Day14.RegolithReservoir().ReadInput();
			_map = AoC.App.Year2022.Day14.RegolithReservoir.CreateMap(input, ground: true);

			MinorGridSize = 6;
			GridOffset = new v2i(0, -500);

			_rnd = new Random(123);
			_sandCount = 0;
		}

		protected override void Update()
		{
			if (AoC.App.Year2022.Day14.RegolithReservoir.AddSand((500, 0), _map) != null)
				_sandCount++;
		}

		protected override void Render()
		{
			DrawGrid(showMinor: false);

			var blockerColor = new v4f(1, 1, 1, .7f);

			// height map
			for (var y = 0; y < _map.GetLength(0); y++)
				for (var x = 0; x < _map.GetLength(1); x++)
				{
					if (_map[y, x] == 0)
						continue;

					var color = blockerColor;
					if (_map[y, x] == (byte)'o')
						color = new v4f(0.4 + _rnd.NextDouble() / 2, 0.3 + _rnd.NextDouble() / 2, 0, 0.8f);

					DrawGridCell(x - 500, y, fill: color);
				}

			DrawGridMouseCursor();

			DrawText(new v2i(32, 32), v2i.Zero,
				new Text($"sand: {_sandCount}"));
		}

		protected override void Destroy()
		{
			ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			AoC.App.Year2022.Day14.RegolithReservoir.AddSand(((int)MouseGridPositionDiscrete.X, -(int)MouseGridPositionDiscrete.Y), _map);
		}
	}
}
