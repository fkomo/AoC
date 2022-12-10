namespace Ujeby.AoC.Vis.App
{
	internal class RopeBridge : MainLoop
	{
		protected override void Init()
		{
			rope = new (int x, int y)[10];
		}

		private (int x, int y)? _target = null;
		private (int x, int y)[] rope;

		protected override void Update()
		{
			if (_target.HasValue)
				AoC.App.Year2022.Day09.RopeBridge.SimulateRope(rope, _target.Value.x, _target.Value.y);
		}

		protected override void Render()
		{
			DrawGrid();

			for (var p = 1; p < rope.Length; p++)
				DrawGridCell(rope[p].x, rope[p].y, 0x77, 0x77, 0x77, 0x77);

			DrawGridCell(rope[0].x, rope[0].y, 0x77, 0x00, 0x00, 0xff);
		}

		protected override void LeftMouseDown()
		{
			_target = new((int)_mouseGrid.X / _gridSize, (int)_mouseGrid.Y / _gridSize);

		}
	}
}
