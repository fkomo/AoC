using System.Numerics;

namespace Ujeby.AoC.Vis.App
{
	internal class RopeBridge : BaseLoop
	{
		protected override void Init()
		{
			SDL2.SDL.SDL_ShowCursor(0); 
			
			_snake = new (int x, int y)[20];
		}

		private (int x, int y)? _apple = null;
		private (int x, int y)[] _snake;

		protected override void Update()
		{
			if (_apple.HasValue)
			{
				var dirx = _apple.Value.x - _snake[0].x;

				if (dirx != 0)
					dirx /= Math.Abs(dirx);

				var diry = _apple.Value.y - _snake[0].y;
				if (diry != 0)
					diry /= Math.Abs(diry);

				if (dirx == 0 && diry == 0)
				{
					_apple = null;
					return;
				}

				AoC.App.Year2022.Day09.RopeBridge.SimulateRope(_snake, dirx, diry);
			}
		}

		protected override void Render()
		{
			DrawGrid();

			// target
			if (_apple.HasValue)
				DrawGridCell(_apple.Value.x, _apple.Value.y, 0x00, 0xff, 0x00, 0x77);

			// rope
			for (var p = 1; p < _snake.Length; p++)
				DrawGridCell(_snake[p].x, _snake[p].y, 0x77, 0x77, 0x77, 0x77);
			DrawGridCell(_snake[0].x, _snake[0].y, 0xff, 0x00, 0x00, 0xff);

			DrawGridMouseCursor();
		}

		protected override void Destroy()
		{
			SDL2.SDL.SDL_ShowCursor(1);
		}

		protected override void LeftMouseDown(Vector2 position)
		{
			var mouseCursorOnGrid = position / _gridSize;
			_apple = new((int)mouseCursorOnGrid.X, (int)mouseCursorOnGrid.Y);
		}
	}
}
