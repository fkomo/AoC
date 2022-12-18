using SDL2;
using System.Numerics;
using Ujeby.Common.Drawing.Entities;
using Ujeby.Common.Drawing.Interfaces;

namespace Ujeby.Common.Drawing
{
	public abstract class BaseLoop : IRunnable
	{
		/// <summary>
		/// mouse position in window (from top-left)
		/// </summary>
		protected Vector2 _mouseWindow;

		/// <summary>
		/// mouse position in grid
		/// </summary>
		protected Vector2 _mouseGrid = new(0, 0);

		protected Vector2 _mouseGridDiscrete => _mouseGrid / _gridSize;

		/// <summary>
		/// grid offset from window center
		/// </summary>
		protected Vector2 _gridOffset = new(0, 0);

		protected uint _mouseState;
		protected bool _mouseLeft;
		protected bool _mouseRight;

		protected string _title;

		protected bool _endLoop;

		public bool DragEnabled = true;

		protected BaseLoop()
		{
		}

		protected bool _drag = false;
		protected Vector2 _dragStart = new();

		public void Run(Func<bool> handleInput)
		{
			Init();

			while (Core.HandleInput(handleInput) && !_endLoop)
			{
				_mouseState = SDL.SDL_GetMouseState(out int mouseX, out int mouseY);
				_mouseWindow.X = mouseX;
				_mouseWindow.Y = mouseY;

				var mouseCentered = new Vector2(_mouseWindow.X - Core.WindowSize.X / 2, Core.WindowSize.Y - mouseY - Core.WindowSize.Y / 2);

				// mouse right
				var right = (_mouseState & 4) == 4;
				if (DragEnabled)
				{
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
				}
				_mouseRight = right;

				// mouse position in grid
				_mouseGrid.X = mouseCentered.X - _gridOffset.X;
				_mouseGrid.Y = mouseCentered.Y + _gridOffset.Y;

				// mouse left
				var left = (_mouseState & 1) == 1;
				if (left)
					LeftMouseDown(_mouseGrid);
				else if (!left && _mouseLeft)
					LeftMouseUp(_mouseGrid);
				_mouseLeft = left;

				_gridSize = Math.Max(2, _gridSize + Core.MouseWheel);
				Core.MouseWheel = 0;

				Update();

				// clear backbuffer
				SDL.SDL_SetRenderDrawColor(Core.RendererPtr, _bgColor, _bgColor, _bgColor, 0xff);
				SDL.SDL_RenderClear(Core.RendererPtr);

				Render();

				SDL.SDL_SetWindowTitle(Core.WindowPtr, _title);

				// display backbuffer
				SDL.SDL_RenderPresent(Core.RendererPtr);
			}

			Destroy();
		}

		protected virtual void LeftMouseDown(Vector2 _mouseGrid)
		{
		}

		protected virtual void LeftMouseUp(Vector2 _mouseGrid)
		{
		}

		protected abstract void Init();
		protected abstract void Update();
		protected abstract void Render();
		protected abstract void Destroy();

		protected void DrawRect(int x, int y, int w, int h, byte r, byte g, byte b, byte a,
			bool fill = true)
		{
			var rect = new SDL.SDL_Rect
			{
				x = x,
				y = (int)Core.WindowSize.Y - y,
				w = w,
				h = -h,
			};

			SDL.SDL_SetRenderDrawColor(Core.RendererPtr, r, g, b, a);

			if (fill)
				SDL.SDL_RenderFillRect(Core.RendererPtr, ref rect);
			else
				SDL.SDL_RenderDrawRect(Core.RendererPtr, ref rect);
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
			var origin = Core.WindowSize / 2 + _gridOffset;

			if (showMinor)
			{
				for (var ix = 1; origin.X + ix * size < Core.WindowSize.X; ix++)
				{
					SDL.SDL_SetRenderDrawColor(Core.RendererPtr, _minorGridColor, _minorGridColor, _minorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Core.RendererPtr, (int)origin.X + ix * size, 0, (int)origin.X + ix * size, (int)Core.WindowSize.Y);
				}

				for (var ix = 1; origin.X - ix * size >= 0; ix++)
				{
					SDL.SDL_SetRenderDrawColor(Core.RendererPtr, _minorGridColor, _minorGridColor, _minorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Core.RendererPtr, (int)origin.X - ix * size, 0, (int)origin.X - ix * size, (int)Core.WindowSize.Y);
				}

				for (var iy = 1; origin.Y + iy * size < Core.WindowSize.Y; iy++)
				{
					SDL.SDL_SetRenderDrawColor(Core.RendererPtr, _minorGridColor, _minorGridColor, _minorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Core.RendererPtr, 0, (int)origin.Y + iy * size, (int)Core.WindowSize.X, (int)origin.Y + iy * size);
				}

				for (var iy = 1; origin.Y - iy * size >= 0; iy++)
				{
					SDL.SDL_SetRenderDrawColor(Core.RendererPtr, _minorGridColor, _minorGridColor, _minorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Core.RendererPtr, 0, (int)origin.Y - iy * size, (int)Core.WindowSize.X, (int)origin.Y - iy * size);
				}
			}

			if (showMajor)
			{
				for (var ix = 1; origin.X + ix * size < Core.WindowSize.X; ix++)
				{
					if (ix % _majorGrid != 0)
						continue;

					SDL.SDL_SetRenderDrawColor(Core.RendererPtr, _majorGridColor, _majorGridColor, _majorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Core.RendererPtr, (int)origin.X + ix * size, 0, (int)origin.X + ix * size, (int)Core.WindowSize.Y);
				}

				for (var ix = 1; origin.X - ix * size >= 0; ix++)
				{
					if (ix % _majorGrid != 0)
						continue;

					SDL.SDL_SetRenderDrawColor(Core.RendererPtr, _majorGridColor, _majorGridColor, _majorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Core.RendererPtr, (int)origin.X - ix * size, 0, (int)origin.X - ix * size, (int)Core.WindowSize.Y);
				}

				for (var iy = 1; origin.Y + iy * size < Core.WindowSize.Y; iy++)
				{
					if (iy % _majorGrid != 0)
						continue;

					SDL.SDL_SetRenderDrawColor(Core.RendererPtr, _majorGridColor, _majorGridColor, _majorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Core.RendererPtr, 0, (int)origin.Y + iy * size, (int)Core.WindowSize.X, (int)origin.Y + iy * size);
				}

				for (var iy = 1; origin.Y - iy * size >= 0; iy++)
				{
					if (iy % _majorGrid != 0)
						continue;

					SDL.SDL_SetRenderDrawColor(Core.RendererPtr, _majorGridColor, _majorGridColor, _majorGridColor, 0xff);
					SDL.SDL_RenderDrawLine(Core.RendererPtr, 0, (int)origin.Y - iy * size, (int)Core.WindowSize.X, (int)origin.Y - iy * size);
				}
			}

			if (showMain)
			{
				SDL.SDL_SetRenderDrawColor(Core.RendererPtr, _zeroColor, _zeroColor, _zeroColor, 0xff);
				SDL.SDL_RenderDrawLine(Core.RendererPtr, (int)origin.X, 0, (int)origin.X, (int)Core.WindowSize.Y);
				SDL.SDL_RenderDrawLine(Core.RendererPtr, 0, (int)origin.Y, (int)Core.WindowSize.X, (int)origin.Y);
			}
		}

		protected void DrawGridCell(int x, int y, byte r, byte g, byte b, byte a,
			bool fill = true)
		{
			var gx = (Core.WindowSize.X / 2 + _gridOffset.X + x * _gridSize) - _gridSize / 2;
			var gy = (Core.WindowSize.Y / 2 - _gridOffset.Y + y * _gridSize) - _gridSize / 2;

			DrawRect((int)gx, (int)gy, _gridSize, _gridSize, r, g, b, a, fill: fill);
		}

		protected void DrawGridCell(int x, int y, Vector4 color,
			bool fill = true)
		{
			var bColor = color * 255;
			DrawGridCell(x, y, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W, fill: fill);
		}

		protected void DrawGridRect(int x, int y, int w, int h, byte r, byte g, byte b, byte a,
			bool fill = true)
		{
			DrawRect(
				(int)(Core.WindowSize.X / 2 + _gridOffset.X + x * _gridSize) - _gridSize / 2,
				(int)(Core.WindowSize.Y / 2 - _gridOffset.Y + (y + 1) * _gridSize) - _gridSize / 2,
				w * _gridSize,
				-h * _gridSize,
				r, g, b, a,
				fill: fill);
		}

		protected void DrawGridCursor(bool printCoords = true)
		{
			// mouse cursor
			DrawGridCell((int)_mouseGridDiscrete.X, (int)_mouseGridDiscrete.Y, 0xff, 0xff, 0x00, 0xff,
				fill: false);

			if (printCoords)
				DrawTextLines(_mouseWindow + new Vector2(16, 16), new Text($"[{(int)_mouseGridDiscrete.X};{(int)_mouseGridDiscrete.Y}]"));
		}

		protected void DrawTextLines(Vector2 position, params TextLine[] lines)
		{
			var font = Core.CurrentFont;
			var fontSprite = SpriteCache.Get(Core.CurrentFont.SpriteId);

			var scale = new Vector2(2, 2);

			var sourceRect = new SDL2.SDL.SDL_Rect();
			var destinationRect = new SDL2.SDL.SDL_Rect();

			var textPosition = position;
			foreach (var line in lines)
			{
				if (line is Text text)
				{
					var color = text.Color * 255;
					SDL2.SDL.SDL_SetTextureColorMod(fontSprite.TexturePtr, (byte)color.X, (byte)color.Y, (byte)color.Z);

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

						SDL2.SDL.SDL_RenderCopy(Core.RendererPtr, fontSprite.TexturePtr, ref sourceRect, ref destinationRect);
						textPosition.X += (charAabb.Size.X + font.Spacing.X) * scale.X;
					}

					textPosition.Y += (font.CharSize.Y + font.Spacing.Y) * scale.Y;
					textPosition.X = position.X;
				}
				else if (line is EmptyLine)
				{
					textPosition.Y += font.CharSize.Y + font.Spacing.Y;
				}
			}
		}

		protected static Vector2 GetTextSize(TextLine[] lines, Font font)
		{
			var scale = new Vector2(2, 2);

			var size = Vector2.Zero;
			foreach (var line in lines)
			{
				if (line is Text text)
				{
					var lineLength = 0;
					for (var i = 0; i < text.Value.Length; i++)
					{
						var charIndex = (int)text.Value[i] - 32;
						var charAabb = font.CharBoxes[charIndex];

						lineLength += (int)(charAabb.Size.X + font.Spacing.X);
					}
					size.X = Math.Max(lineLength, size.X);
					size.Y += font.CharSize.Y + font.Spacing.Y;
				}
				else if (line is EmptyLine)
				{
					size.Y += font.CharSize.Y + font.Spacing.Y;
				}
			}

			return size * scale;
		}
	}
}
