using Ujeby.Graphics;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class Dummy : Sdl2Loop
	{
		public Dummy(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(false); 
		}

		protected override void Update()
		{
		}

		protected override void Render()
		{
			DrawGrid();


			DrawGridMouseCursor(
				style: GridCursorStyles.FullRowColumn);
		}

		protected override void Destroy()
		{
			ShowCursor();
		}
	}
}
