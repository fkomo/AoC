using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class RopeBridge : Sdl2Loop
	{
		private v2i? _apple = null;
		private v2i[] _snake;

		public override string Name => $"#09 {nameof(RopeBridge)}";

		public RopeBridge(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(false); 
			
			_snake = new v2i[20];
		}

		protected override void Update()
		{
			if (_apple.HasValue)
			{
				var dir = _apple.Value - _snake[0];
				var dirAbs = dir.Abs();

				if (dir.X != 0)
					dir.X /= dirAbs.X;

				if (dir.Y != 0)
					dir.Y /= dirAbs.Y;

				if (dir.X == 0 && dir.Y == 0)
				{
					_apple = null;
					return;
				}

				AoC.App.Year2022.Day09.RopeBridge.SimulateRope(_snake, dir);
			}
		}

		protected override void Render()
		{
			DrawGrid();

			// target
			if (_apple.HasValue)
				DrawGridCell((int)_apple.Value.X, (int)_apple.Value.Y, fill: new v4f(0, 1, 0, .5));

			// rope
			for (var p = 1; p < _snake.Length; p++)
				DrawGridCell((int)_snake[p].X, (int)_snake[p].Y, fill: new v4f(1, 1, 1, .5));
			DrawGridCell((int)_snake[0].X, (int)_snake[0].Y, fill: new v4f(1, 0, 0, .5));

			DrawGridMouseCursor();
		}

		protected override void Destroy()
		{
			ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			_apple = new((int)Grid.MousePositionDiscrete.X, (int)Grid.MousePositionDiscrete.Y);
		}
	}
}
