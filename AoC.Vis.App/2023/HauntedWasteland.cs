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
		public override string Name => $"#08 {nameof(HauntedWasteland)}";

		Graph _graph;

		public HauntedWasteland(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var puzzle = new AoC.App._2023_08.HauntedWasteland();
			var input = InputProvider.Read(AppSettings.InputDirectory, puzzle.Year, puzzle.Day);

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

			_graph.RenderGraph(Grid);

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
