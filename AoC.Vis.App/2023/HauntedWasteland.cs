using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
    internal class HauntedWasteland : AoCRunnable
	{
		AoC.App._2023_08.HauntedWasteland _puzzle = new();

		public override string Name => $"#{_puzzle.Day:d2} {nameof(HauntedWasteland)}";

		Graph _graph;

		public HauntedWasteland(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, _puzzle.Year, _puzzle.Day);

			_graph = new Graph();
			_graph.SetData(input.Skip(2)
				.ToDictionary(
					x => x[..3],
					x => new GraphNode
					{
						Id = x[..3],
						Children = new string[] { x.Substring(7, 3), x.Substring(12, 3) }
					}));

			_graph.GraphLayout_Random(new v2i(-128), new v2i(128));
		}

		protected override void Update()
		{
			_graph.Update(Grid);
		}

		protected override void Render()
		{
			Grid.Draw(true, false, false);

			_graph.Render(Grid, new v2i(32, 32));

			Grid.DrawMouseCursor(style: GridCursorStyles.FullRowColumn);

			base.Render();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			_graph.LeftMouseDown();
		}

		protected override void LeftMouseUp()
		{
			_graph.LeftMouseUp();
		}
	}
}
