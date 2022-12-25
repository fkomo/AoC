using SDL2;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl.Interfaces;
using Ujeby.Vectors;

namespace Ujeby.Graphics.Sdl
{
	public abstract class Sdl2Loop : IRunnable
	{
		public bool InvertY = false;

		/// <summary>
		/// workspace dragging with right mouse button
		/// </summary>
		public bool DragEnabled = true;

		/// <summary>
		/// size/step of minor grid lines
		/// </summary>
		public int MinorGridSize = 16;

		/// <summary>
		/// copunt of minor grid lines per major
		/// </summary>
		public int MajorGridSize = 10;

		/// <summary>
		/// mouse position in window (from top-left)
		/// </summary>
		protected v2i MouseWindowPosition;

		/// <summary>
		/// mouse position in grid (from window center)
		/// </summary>
		protected v2i MouseGridPosition;

		/// <summary>
		/// MouseGridPosition / GridSize 
		/// </summary>
		protected v2i MouseGridPositionDiscrete => MouseGridPosition / MinorGridSize;

		/// <summary>
		/// grid offset from window center
		/// </summary>
		protected v2i GridOffset;

		protected readonly v2i _windowSize;

		protected uint _mouseState;
		protected bool _mouseLeft;
		protected bool _mouseRight;

		protected string Title;

		protected bool _terminate;

		protected Sdl2Loop(v2i windowSize)
		{
			_windowSize = windowSize;
		}

		protected bool _drag = false;
		protected v2i _dragStart;

		public void Run(Func<bool> handleInput)
		{
			Init();

			while (Sdl2Wrapper.HandleInput(handleInput) && !_terminate)
			{
				_mouseState = SDL.SDL_GetMouseState(out int mouseX, out int mouseY);
				MouseWindowPosition.Set(mouseX, mouseY);

				// mouse right
				var right = (_mouseState & 4) == 4;
				if (DragEnabled)
				{
					if (right && !_mouseRight)
					{
						_drag = true;
						_dragStart = MouseWindowPosition;
					}
					else if (!right && _mouseRight)
					{
						_drag = false;
					}
					else if (_drag)
					{
						GridOffset += MouseWindowPosition - _dragStart;
						_dragStart = MouseWindowPosition;
					}
				}
				_mouseRight = right;

				MouseGridPosition = (MouseWindowPosition - _windowSize / 2) - GridOffset;

				// mouse left
				var left = (_mouseState & 1) == 1;
				if (left)
					LeftMouseDown(MouseGridPosition);
				else if (!left && _mouseLeft)
					LeftMouseUp(MouseGridPosition);
				_mouseLeft = left;

				MinorGridSize = Math.Max(2, MinorGridSize + Sdl2Wrapper.MouseWheel);
				Sdl2Wrapper.MouseWheel = 0;

				Update();

				// clear backbuffer
				SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, _bgColor, _bgColor, _bgColor, 0xff);
				SDL.SDL_RenderClear(Sdl2Wrapper.RendererPtr);

				Render();

				if (Title != null)
					SDL.SDL_SetWindowTitle(Sdl2Wrapper.WindowPtr, Title);

				// display backbuffer
				SDL.SDL_RenderPresent(Sdl2Wrapper.RendererPtr);
			}

			Destroy();
		}

		protected virtual void LeftMouseDown(v2i _mouseGrid)
		{
		}

		protected virtual void LeftMouseUp(v2i _mouseGrid)
		{
		}

		protected abstract void Init();
		protected abstract void Update();
		protected abstract void Render();
		protected abstract void Destroy();

		protected void DrawRect(int x, int y, int w, int h, v4f color)
		{
			var bColor = color * 255;
			DrawRect(x, y, w, h, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
		}
		protected void DrawRectFill(int x, int y, int w, int h, v4f color)
		{
			var bColor = color * 255;
			DrawRectFill(x, y, w, h, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
		}
		protected void DrawGridCell(int x, int y, v4f color)
		{
			var bColor = color * 255;
			DrawGridCell(x, y, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
		}
		protected void DrawGridCellFill(int x, int y, v4f color)
		{
			var bColor = color * 255;
			DrawGridCellFill(x, y, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
		}
		protected void DrawGridRect(int x, int y, int w, int h, v4f color)
		{
			var bColor = color * 255;
			DrawGridRect(x, y, w, h, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
		}
		protected void DrawGridRectFill(int x, int y, int w, int h, v4f color)
		{
			var bColor = color * 255;
			DrawGridRectFill(x, y, w, h, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
		}

		protected void DrawRect(int x, int y, int w, int h, byte r, byte g, byte b, byte a)
		{
			var rect = new SDL.SDL_Rect
			{
				x = x,
				y = y,
				w = w,
				h = h,
			};

			_ = SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, r, g, b, a);
			_ = SDL.SDL_RenderDrawRect(Sdl2Wrapper.RendererPtr, ref rect);
		}
		protected void DrawRectFill(int x, int y, int w, int h, byte r, byte g, byte b, byte a)
		{
			var rect = new SDL.SDL_Rect
			{
				x = x,
				y = y,
				w = w,
				h = h,
			};

			_ = SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, r, g, b, a);
			_ = SDL.SDL_RenderFillRect(Sdl2Wrapper.RendererPtr, ref rect);
		}
		protected void DrawGridCell(int x, int y, byte r, byte g, byte b, byte a)
		{
			var cell = _windowSize / 2 + GridOffset + new v2i(x, y) * MinorGridSize - MinorGridSize / 2;

			DrawRect((int)cell.X, (int)cell.Y, MinorGridSize, MinorGridSize, r, g, b, a);
		}
		protected void DrawGridCellFill(int x, int y, byte r, byte g, byte b, byte a)
		{
			var gx = (_windowSize.X / 2 + GridOffset.X + x * MinorGridSize) - MinorGridSize / 2;
			var gy = (_windowSize.Y / 2 + GridOffset.Y + y * MinorGridSize) - MinorGridSize / 2;

			DrawRectFill((int)gx, (int)gy, MinorGridSize, MinorGridSize, r, g, b, a);
		}
		protected void DrawGridRect(int x, int y, int w, int h, byte r, byte g, byte b, byte a)
		{
			DrawRect(
				(int)(_windowSize.X / 2 + GridOffset.X + x * MinorGridSize) - MinorGridSize / 2,
				(int)(_windowSize.Y / 2 + GridOffset.Y + (y) * MinorGridSize) - MinorGridSize / 2,
				w * MinorGridSize,
				h * MinorGridSize,
				r, g, b, a);
		}
		protected void DrawGridRectFill(int x, int y, int w, int h, byte r, byte g, byte b, byte a)
		{
			DrawRectFill(
				(int)(_windowSize.X / 2 + GridOffset.X + x * MinorGridSize) - MinorGridSize / 2,
				(int)(_windowSize.Y / 2 + GridOffset.Y + (y) * MinorGridSize) - MinorGridSize / 2,
				w * MinorGridSize,
				h * MinorGridSize,
				r, g, b, a);
		}

		private byte _bgColor = 0x07;
		private byte _zeroColor = 0xcc;
		private byte _majorGridColor = 0x10;
		private byte _minorGridColor = 0x0d;
		private byte _gridAlpha = 0xaa;

		protected void DrawGrid(
			bool showMain = true, bool showMajor = true, bool showMinor = true)
		{
			var size = MinorGridSize;
			var origin = _windowSize / 2 + GridOffset;

			if (showMinor)
			{
				for (var ix = 1; origin.X + ix * size < _windowSize.X; ix++)
				{
					SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, _minorGridColor, _minorGridColor, _minorGridColor, _gridAlpha);
					SDL.SDL_RenderDrawLine(Sdl2Wrapper.RendererPtr, (int)origin.X + ix * size, 0, (int)origin.X + ix * size, (int)_windowSize.Y);
				}

				for (var ix = 1; origin.X - ix * size >= 0; ix++)
				{
					SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, _minorGridColor, _minorGridColor, _minorGridColor, _gridAlpha);
					SDL.SDL_RenderDrawLine(Sdl2Wrapper.RendererPtr, (int)origin.X - ix * size, 0, (int)origin.X - ix * size, (int)_windowSize.Y);
				}

				for (var iy = 1; origin.Y + iy * size < _windowSize.Y; iy++)
				{
					SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, _minorGridColor, _minorGridColor, _minorGridColor, _gridAlpha);
					SDL.SDL_RenderDrawLine(Sdl2Wrapper.RendererPtr, 0, (int)origin.Y + iy * size, (int)_windowSize.X, (int)origin.Y + iy * size);
				}

				for (var iy = 1; origin.Y - iy * size >= 0; iy++)
				{
					SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, _minorGridColor, _minorGridColor, _minorGridColor, _gridAlpha);
					SDL.SDL_RenderDrawLine(Sdl2Wrapper.RendererPtr, 0, (int)origin.Y - iy * size, (int)_windowSize.X, (int)origin.Y - iy * size);
				}

				SDL.SDL_RenderDrawLine(Sdl2Wrapper.RendererPtr, (int)origin.X, 0, (int)origin.X, (int)_windowSize.Y);
				SDL.SDL_RenderDrawLine(Sdl2Wrapper.RendererPtr, 0, (int)origin.Y, (int)_windowSize.X, (int)origin.Y);
			}

			if (showMajor)
			{
				for (var ix = 1; origin.X + ix * size < _windowSize.X; ix++)
				{
					if (ix % MajorGridSize != 0)
						continue;

					SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, _majorGridColor, _majorGridColor, _majorGridColor, _gridAlpha);
					SDL.SDL_RenderDrawLine(Sdl2Wrapper.RendererPtr, (int)origin.X + ix * size, 0, (int)origin.X + ix * size, (int)_windowSize.Y);
				}

				for (var ix = 1; origin.X - ix * size >= 0; ix++)
				{
					if (ix % MajorGridSize != 0)
						continue;

					SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, _majorGridColor, _majorGridColor, _majorGridColor, _gridAlpha);
					SDL.SDL_RenderDrawLine(Sdl2Wrapper.RendererPtr, (int)origin.X - ix * size, 0, (int)origin.X - ix * size, (int)_windowSize.Y);
				}

				for (var iy = 1; origin.Y + iy * size < _windowSize.Y; iy++)
				{
					if (iy % MajorGridSize != 0)
						continue;

					SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, _majorGridColor, _majorGridColor, _majorGridColor, _gridAlpha);
					SDL.SDL_RenderDrawLine(Sdl2Wrapper.RendererPtr, 0, (int)origin.Y + iy * size, (int)_windowSize.X, (int)origin.Y + iy * size);
				}

				for (var iy = 1; origin.Y - iy * size >= 0; iy++)
				{
					if (iy % MajorGridSize != 0)
						continue;

					SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, _majorGridColor, _majorGridColor, _majorGridColor, _gridAlpha);
					SDL.SDL_RenderDrawLine(Sdl2Wrapper.RendererPtr, 0, (int)origin.Y - iy * size, (int)_windowSize.X, (int)origin.Y - iy * size);
				}
			}

			if (showMain)
			{
				SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, _zeroColor, _zeroColor, _zeroColor, _gridAlpha);
				SDL.SDL_RenderDrawLine(Sdl2Wrapper.RendererPtr, (int)origin.X, 0, (int)origin.X, (int)_windowSize.Y);
				SDL.SDL_RenderDrawLine(Sdl2Wrapper.RendererPtr, 0, (int)origin.Y, (int)_windowSize.X, (int)origin.Y);
			}
		}

		protected void DrawGridMouseCursor(
			bool printCoords = true)
		{
			var m = MouseGridPositionDiscrete;

			DrawGridCell((int)m.X, (int)m.Y, 0xff, 0xff, 0x00, 0xff);

			if (printCoords)
				DrawText(MouseWindowPosition + new v2i(16, 16), v2i.Zero, new Text($"[{(int)m.X};{(int)m.Y}]"));
		}

		protected void DrawText(v2i position, v2i spacing, params TextLine[] lines)
		{
			var font = Sdl2Wrapper.CurrentFont;
			var fontSprite = SpriteCache.Get(Sdl2Wrapper.CurrentFont.SpriteId);

			var scale = new v2i(2, 2);

			var sourceRect = new SDL.SDL_Rect();
			var destinationRect = new SDL.SDL_Rect();

			var textPosition = position;
			foreach (var line in lines)
			{
				if (line is Text text)
				{
					var color = text.Color * 255;
					SDL.SDL_SetTextureColorMod(fontSprite.TexturePtr, (byte)color.X, (byte)color.Y, (byte)color.Z);

					for (var i = 0; i < text.Value.Length; i++)
					{
						var charIndex = (int)text.Value[i] - 32;
						var charAabb = font.CharBoxes[charIndex];

						sourceRect.x = (int)(font.CharSize.X * charIndex + charAabb.Min.X);
						sourceRect.y = (int)charAabb.Min.Y;
						sourceRect.w = (int)charAabb.Size.X;
						sourceRect.h = (int)charAabb.Size.Y;

						destinationRect.x = (int)(textPosition.X);
						destinationRect.y = (int)(textPosition.Y);
						destinationRect.w = (int)(charAabb.Size.X * scale.X);
						destinationRect.h = (int)(charAabb.Size.Y * scale.Y);

						SDL.SDL_RenderCopy(Sdl2Wrapper.RendererPtr, fontSprite.TexturePtr, ref sourceRect, ref destinationRect);
						textPosition.X += (charAabb.Size.X + font.Spacing.X + spacing.X) * scale.X;
					}

					textPosition.Y += (font.CharSize.Y + font.Spacing.Y + spacing.Y) * scale.Y;
					textPosition.X = position.X;
				}
				else if (line is EmptyLine)
				{
					textPosition.Y += font.CharSize.Y + font.Spacing.Y + spacing.Y;
				}
			}
		}

		protected void ShowCursor(bool show = true)
		{
			SDL2.SDL.SDL_ShowCursor(show ? 1 : 0);
		}
	}
}
