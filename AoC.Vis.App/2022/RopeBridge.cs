using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class RopeBridge : Sdl2Loop
	{
		private (int x, int y)? _apple = null;
		private (int x, int y)[] _snake; 
		
		public RopeBridge(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(false); 
			
			_snake = new (int x, int y)[20];
		}

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
				DrawGridCellFill(_apple.Value.x, _apple.Value.y, new v4f(0, 1, 0, 0.5f));

			// rope
			for (var p = 1; p < _snake.Length; p++)
				DrawGridCellFill(_snake[p].x, _snake[p].y, new v4f(1, 1, 1, 0.5f));
			DrawGridCellFill(_snake[0].x, _snake[0].y, new v4f(1, 0, 0, 0.5f));

			DrawGridMouseCursor();
		}

		protected override void Destroy()
		{
			ShowCursor();
		}

		protected override void LeftMouseDown(v2i position)
		{
			var mouseCursorOnGrid = position / MinorGridSize;
			_apple = new((int)mouseCursorOnGrid.X, (int)mouseCursorOnGrid.Y);
		}
	}
}
