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
				DrawGridCell(_apple.Value.x, _apple.Value.y, fill: new v4f(0, 1, 0, 0.5f));

			// rope
			for (var p = 1; p < _snake.Length; p++)
				DrawGridCell(_snake[p].x, _snake[p].y, fill: new v4f(1, 1, 1, 0.5f));
			DrawGridCell(_snake[0].x, _snake[0].y, fill: new v4f(1, 0, 0, 0.5f));

			DrawGridMouseCursor();
		}

		protected override void Destroy()
		{
			ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			_apple = new((int)MouseGridPositionDiscrete.X, (int)MouseGridPositionDiscrete.Y);
		}
	}
}
