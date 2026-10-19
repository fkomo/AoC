using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
    internal class LANParty : AoCRunnable
	{
		AoC.App._2024_23.LANParty _puzzle = new();

		public override string Name => $"#{_puzzle.Day:d2} {nameof(LANParty)}";

		Dictionary<string, List<string>> _network;
		Graph _graph;

		public LANParty(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, _puzzle.Year, _puzzle.Day, suffix: ".sample");
			_network = AoC.App._2024_23.LANParty.CreateNetwork(input);

			_graph = new Graph() { PickZone = 5 };
			_graph.SetData(_network
				.ToDictionary(
					x => x.Key,
					x => new GraphNode
					{
						Id = x.Key,
						Children = [.. x.Value]
					}));

			_graph.GraphLayout_Random(new v2i(-64), new v2i(64));
		}

		protected override void Update()
		{
			_graph.Update(Grid, MousePosition);
		}

		protected override void Render()
		{
			Grid.Draw(true, showMajor: true, showMinor: true);

			_graph.Render(Grid, new v2i(32, 64));

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{_network.Count} pc's"));

			Grid.DrawMouseCursor(style: GridCursorStyles.FullRowColumn);

			base.Render();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			_graph.LeftMouseDown(Grid, MousePosition);
		}

		protected override void LeftMouseUp()
		{
			_graph.LeftMouseUp(Grid, MousePosition);
		}
	}
}
