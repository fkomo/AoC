using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
    internal class PulsePropagation : AoCRunnable
	{
		AoC.App._2023_20.PulsePropagation _puzzle = new();

		public override string Name => $"#{_puzzle.Day:d2} {nameof(PulsePropagation)}";

		Dictionary<string, AoC.App._2023_20.PulsePropagation.Module> _modules;
		Graph _graph;

		public PulsePropagation(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, _puzzle.Year, _puzzle.Day);
			_modules = AoC.App._2023_20.PulsePropagation.CreateModules(input);

			_graph = new Graph() { PickZone = 10 };
			_graph.SetData(_modules
				.ToDictionary(
					x => x.Key,
					x => new GraphNode
					{
						Id = x.Key,
						Children = x.Value.Output
					}));

			_graph.GraphLayout_Random(new v2i(-256), new v2i(256));
		}

		protected override void Update()
		{
			_graph.Update(Grid);
		}

		protected override void Render()
		{
			Grid.Draw(true, false, false);

			_graph.Render(Grid, new v2i(32, 64));

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{_modules.Count} modules"));

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
