using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class Grid : Sdl2Loop
	{
		public Grid(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(false);

			MinorGridSize = 16;
			MajorGridSize = 10;
		}

		protected override void Update()
		{
		}

		protected override void Render()
		{
			DrawGrid();

			DrawRect(160, 320, 320, 160, new v4f(0, 0, 1, 1));
			DrawGridCell(0, 0, new v4f(1, 1, 1, 1));
			DrawGridCell(10, 20, new v4f(0, 1, 0, 1));
			DrawGridRect(10, 20, 16, 32, new v4f(1, 0, 0, 1));

			DrawRectFill(160, 320, 320, 160, new v4f(0, 0, 1, 0.5f));
			DrawGridCellFill(0, 0, new v4f(1, 1, 1, 0.5));
			DrawGridCellFill(10, 20, new v4f(0, 1, 0, 0.5f));
			DrawGridRectFill(10, 20, 16, 32, new v4f(1, 0, 0, 0.5f));

			DrawGridMouseCursor();

			DrawText(new v2i(32, 32), 
				new Text($"grid"));
		}
		protected override void Destroy()
		{
			ShowCursor();
		}
	}
}
