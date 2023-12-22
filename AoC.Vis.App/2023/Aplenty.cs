using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
    internal class Aplenty : AoCRunnable
	{
		public override string Name => $"#{_puzzle.Day:d2} {nameof(Aplenty)}";

		AoC.App._2023_19.Aplenty _puzzle = new();
		Dictionary<string, AoC.App._2023_19.Aplenty.Rule[]> _workflows;

		Graph _graph;

		public Aplenty(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, _puzzle.Year, _puzzle.Day);
			_workflows = AoC.App._2023_19.Aplenty.CreateWorkflows(input);

			_workflows.Add("R", Array.Empty<AoC.App._2023_19.Aplenty.Rule>());
			_workflows.Add("A", Array.Empty<AoC.App._2023_19.Aplenty.Rule>());

			_graph = new Graph();
			
			_graph.SetData(_workflows
				.ToDictionary(
					x => x.Key,
					x => new GraphNode
					{
						Id = x.Key,
						Children = x.Value.Select(r => r.Result).ToArray()
					}));

			_graph.GraphLayout_Random(new v2i(-256), new v2i(256));
		}

		protected override void Update()
		{
			_graph.Update(Grid, MousePosition);
		}

		protected override void Render()
		{
			Grid.Draw(true, false, false);

			_graph.Render(Grid, new v2i(32, 64));

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{_workflows.Count} workflows"));
	
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
