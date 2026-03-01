using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class PrintingDepartment : AoCRunnable
	{
        public override string Name => $"#04 {nameof(PrintingDepartment)}";

		public PrintingDepartment(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, 2025, 09);

		}

		protected override void Update()
		{
		}

		protected override void Render()
		{

			base.Render();
		}

		protected override void LeftMouseUp()
		{
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
