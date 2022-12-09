using SDL2;
using System.Numerics;

namespace Ujeby.AoC.Vis.App
{
	internal class RopeBridge : MainLoop
	{
		protected override void Init()
		{
			rope = new (int x, int y)[10];
		}

		private (int x, int y) _target;
		private (int x, int y)[] rope;

		protected override void Update()
		{
			AoC.App.Year2022.Day09.RopeBridge.SimulateRope(rope, _target.x, _target.y);
		}

		protected override void Render()
		{
			DrawGrid();

			for (var p = 1; p < rope.Length; p++)
				DrawGridCell(rope[p].x, rope[p].y, 0x77, 0x77, 0x77, 0x77);

			DrawGridCell(rope[0].x, rope[0].y, 0x77, 0x00, 0x00, 0xff);
		}
	}
}
