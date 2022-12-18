using System.Numerics;

namespace Ujeby.AoC.Vis.App
{
	internal class RegolithReservoir : BaseLoop
	{
		private byte[,] _map;

		protected override void Init()
		{
			SDL2.SDL.SDL_ShowCursor(0);

			var input = new AoC.App.Year2022.Day14.RegolithReservoir().ReadInput();
			_map = AoC.App.Year2022.Day14.RegolithReservoir.CreateMap(input, ground: true);

			_gridSize = 6;
			_gridOffset = new Vector2(0, -500);
		}

		protected override void Update()
		{
			AoC.App.Year2022.Day14.RegolithReservoir.AddSand((500, 0), _map);
		}

		protected override void Render()
		{
			DrawGrid(showMinor: false);

			var blockerColor = new Vector4(1, 1, 1, .7f);
			var sandColor = new Vector4(0.9f, 0.7f, 0, .7f);

			// height map
			for (var y = 0; y < _map.GetLength(0); y++)
				for (var x = 0; x < _map.GetLength(1); x++)
				{
					if (_map[y, x] == 0)
						continue;

					var color = (_map[y, x] == (byte)'#') ? blockerColor : sandColor;

					DrawGridCell(x - 500, -y, color);
				}

			DrawGridCursor();
		}

		protected override void Destroy()
		{
			SDL2.SDL.SDL_ShowCursor(1);
		}

		protected override void LeftMouseDown(Vector2 position)
		{
			AoC.App.Year2022.Day14.RegolithReservoir.AddSand(((int)_mouseGridDiscrete.X, -(int)_mouseGridDiscrete.Y), _map);
		}
	}
}
