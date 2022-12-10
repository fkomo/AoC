﻿using System.Numerics;

namespace Ujeby.AoC.Vis.App
{
	internal class RopeBridge : MainLoop
	{
		protected override void Init()
		{
			snake = new (int x, int y)[20];
			SDL2.SDL.SDL_ShowCursor(0);
		}

		private (int x, int y)? _apple = null;
		private (int x, int y)[] snake;

		protected override void Update()
		{
			if (_apple.HasValue)
			{
				var dirx = _apple.Value.x - snake[0].x;

				if (dirx != 0)
					dirx /= Math.Abs(dirx);

				var diry = _apple.Value.y - snake[0].y;
				if (diry != 0)
					diry /= Math.Abs(diry);

				if (dirx == 0 && diry == 0)
				{
					_apple = null;
					return;
				}

				AoC.App.Year2022.Day09.RopeBridge.SimulateRope(snake, dirx, diry);
			}
		}

		protected override void Render()
		{
			DrawGrid();

			// target
			if (_apple.HasValue)
				DrawGridCell(_apple.Value.x, _apple.Value.y, 0x00, 0xff, 0x00, 0x77);

			// rope
			for (var p = 1; p < snake.Length; p++)
				DrawGridCell(snake[p].x, snake[p].y, 0x77, 0x77, 0x77, 0x77);
			DrawGridCell(snake[0].x, snake[0].y, 0xff, 0x00, 0x00, 0xff);

			// mouse cursor
			var mouseCursorOnGrid = _mouseGrid / _gridSize;
			DrawGridCell((int)mouseCursorOnGrid.X, (int)mouseCursorOnGrid.Y, 0xff, 0xff, 0xff, 0x77);
		}

		protected override void LeftMouseDown(Vector2 position)
		{
			var mouseCursorOnGrid = position / _gridSize;
			_apple = new((int)mouseCursorOnGrid.X, (int)mouseCursorOnGrid.Y);
		}

		protected override void LeftMouseUp(Vector2 position)
		{

		}
	}
}