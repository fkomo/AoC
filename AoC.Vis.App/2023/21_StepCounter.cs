using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
    internal class StepCounter : AoCRunnable
	{
		public override string Name => $"#{_puzzle.Day:d2} {nameof(StepCounter)}";

		AoC.App._2023_21.StepCounter _puzzle = new();
		
		HashSet<v2i> _plots = null;
		v2i _start = v2i.Zero;
		
		private string[] _input;

		protected Gui _gui = new();

		public StepCounter(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			_input = InputProvider.Read(AppSettings.InputDirectory, _puzzle.Year, _puzzle.Day);
			_start = AoC.App._2023_21.StepCounter.GetStart(_input);
			_plots = new HashSet<v2i>() { _start };
		}

		protected override void Update()
		{
			_gui.Update(MousePosition);
		}

		protected override void Render()
		{
			Grid.Draw(true, true, true);

			var plotColor = Colors.White;
			Grid.DrawCells(_plots, fill: plotColor);

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{_plots.Count} plots"));

			_gui.Render();

			base.Render();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			_gui.LeftMouseDown(MousePosition);

			_plots = AoC.App._2023_21.StepCounter.Step(_plots, _input);
		}

		protected override void LeftMouseUp()
		{
			_gui.LeftMouseUp(MousePosition);
		}
	}
}
