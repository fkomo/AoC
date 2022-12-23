using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class RegolithReservoir : Sdl2Loop
	{
		private byte[,] _map;

		private Random _rnd;

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
		}

		protected override void Update()
		{
			AoC.App.Year2022.Day14.RegolithReservoir.AddSand((500, 0), _map);
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

					var color = (_map[y, x] == (byte)'#') ? 
						blockerColor : new v4f(0.4 + _rnd.NextDouble() / 2, 0.3 + _rnd.NextDouble() / 2, 0, 0.8f);

					DrawGridCellFill(x - 500, y, color);
				}

			DrawGridMouseCursor();
		}

		protected override void Destroy()
		{
			ShowCursor();
		}

		protected override void LeftMouseDown(v2i position)
		{
			AoC.App.Year2022.Day14.RegolithReservoir.AddSand(((int)MouseGridPositionDiscrete.X, -(int)MouseGridPositionDiscrete.Y), _map);
		}
	}
}
