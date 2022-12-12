using SDL2;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Ujeby.AoC.Vis.App
{
	public abstract class BaseLoop
	{
		/// <summary>
		/// mouse position in window (from top-left)
		/// </summary>
		protected Vector2 _mouseWindow;

		/// <summary>
		/// mouse position in grid
		/// </summary>
		protected Vector2 _mouseGrid = new(0, 0);

		/// <summary>
		/// grid offset from window center
		/// </summary>
		protected Vector2 _gridOffset = new(0, 0);

		protected uint _mouseState;
		protected bool _mouseLeft;
		protected bool _mouseRight;

		protected string _title;

		protected BaseLoop()
		{
			Init();
		}

		protected bool _drag = false;
		protected Vector2 _dragStart = new();

		public void Run(Func<bool> handleInput)
		{
			while (handleInput())
			{
				_mouseState = SDL.SDL_GetMouseState(out int mouseX, out int mouseY);
				_mouseWindow.X = mouseX - Program.WindowSize.X / 2;
				_mouseWindow.Y = Program.WindowSize.Y - mouseY - Program.WindowSize.Y / 2;

				// mouse right
				var right = (_mouseState & 4) == 4;
				if (right && !_mouseRight)
				{
					_drag = true;

					_dragStart.X = mouseX;
					_dragStart.Y = mouseY;
				}
				else if (!right && _mouseRight)
				{
					_drag = false;
				}
				else if (_drag)
				{
					_gridOffset.X += mouseX - _dragStart.X;
					_gridOffset.Y += mouseY - _dragStart.Y;

					_dragStart.X = mouseX;
					_dragStart.Y = mouseY;
				}
				_mouseRight = right;

				// mouse position in grid
				_mouseGrid.X = _mouseWindow.X - _gridOffset.X;
				_mouseGrid.Y = _mouseWindow.Y + _gridOffset.Y;

				// mouse left
				var left = (_mouseState & 1) == 1;
				if (left)
					LeftMouseDown(_mouseGrid);
				else if (!left && _mouseLeft)
					LeftMouseUp(_mouseGrid);
				_mouseLeft = left;

				_title = $"mouse[btn={_mouseState}, window={mouseX}x{mouseY}, grid={(int)_mouseGrid.X}x{(int)_mouseGrid.Y}]";
				if (_drag)
					_title += $" drag[{(int)_dragStart.X} x {(int)_dragStart.Y}]";

				_gridSize = Math.Max(2, _gridSize + Program.MouseWheel);
				Program.MouseWheel = 0;

				Update();

				// clear backbuffer
				SDL.SDL_SetRenderDrawColor(Program.RendererPtr, _bgColor, _bgColor, _bgColor, 0xff);
				SDL.SDL_RenderClear(Program.RendererPtr);

				Render();

				SDL.SDL_SetWindowTitle(Program.WindowPtr, _title);

				// display backbuffer
				SDL.SDL_RenderPresent(Program.RendererPtr);
			}
		}

		protected abstract void LeftMouseDown(Vector2 _mouseGrid);
		protected abstract void LeftMouseUp(Vector2 _mouseGrid);

		protected abstract void Init();
		protected abstract void Update();
		protected abstract void Render();

		protected void DrawRect(int x, int y, int w, int h, byte r, byte g, byte b, byte a,
			bool fill = true)
		{
			var rect = new SDL.SDL_Rect
			{
				x = x,
				y = (int)Program.WindowSize.Y - y,
				w = w,
				h = -h,
			};

			SDL.SDL_SetRenderDrawColor(Program.RendererPtr, r, g, b, a);

			if (fill)
				SDL.SDL_RenderFillRect(Program.RendererPtr, ref rect);
			else
				SDL.SDL_RenderDrawRect(Program.RendererPtr, ref rect);
		}

		private byte _bgColor = 0x0d;
		private byte _zeroColor = 0xcc;
		private byte _majorGridColor = 0x20;
		private byte _minorGridColor = 0x10;

		public int _gridSize = 16;
		public int _majorGrid = 10;

		protected void DrawGrid(
			bool showMain = true, bool showMajor = true, bool showMinor = true)
		{
			var size = _gridSize;
			var origin = Program.WindowSize / 2 + _gridOffset;

			if (showMinor)
			{
				for (var ix = 1; origin.X + ix * size < Program.WindowSize.X; ix++)
				{
					SDL.SDL_SetRenderDrawColor(Program.RendererPtr, _minorGridColor, _minorGridColor, _minorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Program.RendererPtr, (int)origin.X + ix * size, 0, (int)origin.X + ix * size, (int)Program.WindowSize.Y);
				}

				for (var ix = 1; origin.X - ix * size >= 0; ix++)
				{
					SDL.SDL_SetRenderDrawColor(Program.RendererPtr, _minorGridColor, _minorGridColor, _minorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Program.RendererPtr, (int)origin.X - ix * size, 0, (int)origin.X - ix * size, (int)Program.WindowSize.Y);
				}

				for (var iy = 1; origin.Y + iy * size < Program.WindowSize.Y; iy++)
				{
					SDL.SDL_SetRenderDrawColor(Program.RendererPtr, _minorGridColor, _minorGridColor, _minorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Program.RendererPtr, 0, (int)origin.Y + iy * size, (int)Program.WindowSize.X, (int)origin.Y + iy * size);
				}

				for (var iy = 1; origin.Y - iy * size >= 0; iy++)
				{
					SDL.SDL_SetRenderDrawColor(Program.RendererPtr, _minorGridColor, _minorGridColor, _minorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Program.RendererPtr, 0, (int)origin.Y - iy * size, (int)Program.WindowSize.X, (int)origin.Y - iy * size);
				}
			}

			if (showMajor)
			{
				for (var ix = 1; origin.X + ix * size < Program.WindowSize.X; ix++)
				{
					if (ix % _majorGrid != 0)
						continue;

					SDL.SDL_SetRenderDrawColor(Program.RendererPtr, _majorGridColor, _majorGridColor, _majorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Program.RendererPtr, (int)origin.X + ix * size, 0, (int)origin.X + ix * size, (int)Program.WindowSize.Y);
				}

				for (var ix = 1; origin.X - ix * size >= 0; ix++)
				{
					if (ix % _majorGrid != 0)
						continue;

					SDL.SDL_SetRenderDrawColor(Program.RendererPtr, _majorGridColor, _majorGridColor, _majorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Program.RendererPtr, (int)origin.X - ix * size, 0, (int)origin.X - ix * size, (int)Program.WindowSize.Y);
				}

				for (var iy = 1; origin.Y + iy * size < Program.WindowSize.Y; iy++)
				{
					if (iy % _majorGrid != 0)
						continue;

					SDL.SDL_SetRenderDrawColor(Program.RendererPtr, _majorGridColor, _majorGridColor, _majorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Program.RendererPtr, 0, (int)origin.Y + iy * size, (int)Program.WindowSize.X, (int)origin.Y + iy * size);
				}

				for (var iy = 1; origin.Y - iy * size >= 0; iy++)
				{
					if (iy % _majorGrid != 0)
						continue;

					SDL.SDL_SetRenderDrawColor(Program.RendererPtr, _majorGridColor, _majorGridColor, _majorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Program.RendererPtr, 0, (int)origin.Y - iy * size, (int)Program.WindowSize.X, (int)origin.Y - iy * size);
				}
			}

			if (showMain)
			{
				SDL.SDL_SetRenderDrawColor(Program.RendererPtr, _zeroColor, _zeroColor, _zeroColor, 0xff);
				SDL.SDL_RenderDrawLine(Program.RendererPtr, (int)origin.X, 0, (int)origin.X, (int)Program.WindowSize.Y);
				SDL.SDL_RenderDrawLine(Program.RendererPtr, 0, (int)origin.Y, (int)Program.WindowSize.X, (int)origin.Y);
			}
		}

		protected void DrawGridCell(int x, int y, byte r, byte g, byte b, byte a,
			bool fill = true)
		{
			var gx = (Program.WindowSize.X / 2 + _gridOffset.X + x * _gridSize) - _gridSize / 2;
			var gy = (Program.WindowSize.Y / 2 - _gridOffset.Y + y * _gridSize) - _gridSize / 2;

			DrawRect((int)gx, (int)gy, _gridSize, _gridSize, r, g, b, a, fill: fill);
		}

		protected void DrawGridRect(int x, int y, int w, int h, byte r, byte g, byte b, byte a,
			bool fill = true)
		{
			DrawRect(
				(int)(Program.WindowSize.X / 2 + _gridOffset.X + x * _gridSize) - _gridSize / 2,
				(int)(Program.WindowSize.Y / 2 - _gridOffset.Y + (y + 1) * _gridSize) - _gridSize / 2,
				w * _gridSize,
				-h * _gridSize,
				r, g, b, a,
				fill: fill);
		}
	}
}
