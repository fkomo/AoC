using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
    internal class LavaductLagoon : AoCRunnable
	{
		public override string Name => $"#{_puzzle.Day:d2} {nameof(LavaductLagoon)}";

		AoC.App._2023_18.LavaductLagoon _puzzle = new();
		v2i[] _trench = null;

		public LavaductLagoon(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, _puzzle.Year, _puzzle.Day);

			var digPlan = AoC.App._2023_18.LavaductLagoon.CreateDigPlanPart2(input);
			_trench = AoC.App._2023_18.LavaductLagoon.CreateTrench(digPlan);
		}

		protected override void Update()
		{
		}

		protected override void Render()
		{
			Grid.Draw(true, true, true);

			var trenchColor = Colors.White;
			for (var i = 0; i < _trench.Length - 1; i++)
				Grid.DrawLine(_trench[i] / 50000, _trench[i + 1] / 50000, trenchColor);

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{_trench.Length} vertices"));
	
			base.Render();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}

		protected override void LeftMouseDown()
		{
		}

		protected override void LeftMouseUp()
		{
		}
	}
}
