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

			// height map
			for (var y = 0; y < _map.GetLength(0); y++)
				for (var x = 0; x < _map.GetLength(1); x++)
				{
					byte c = 0;
					if (_map[y, x] == (byte)'#')
						c = 0xff;
					else if (_map[y, x] == (byte)'o')
						c = 0x77;

					if (c != 0)
						DrawGridCell(x - 500, -y, c, c, c, 0x77);
				}

			DrawGridMouseCursor();
		}

		protected override void Destroy()
		{
			SDL2.SDL.SDL_ShowCursor(1);
		}

		protected override void LeftMouseDown(Vector2 position)
		{
			// TODO add sand at cursor
		}
	}
}
