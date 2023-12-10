using System.Reflection.Metadata.Ecma335;
using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	public class GraphNodeMeta
	{
		public v2i Position { get; set; }
	}

	public class GraphNode
	{
		public string Id { get; set; }
		public string[] Children { get; set; }

		public GraphNodeMeta Meta { get; set; }

		public GraphNode() { }
	}

	internal class HauntedWasteland : AoCRunnable
	{
		public override string Name => $"#08 {nameof(HauntedWasteland)}";
		int _pickZone = 5;
		string _selected = null;
		bool _moving = false;

		Dictionary<string, GraphNode> _graph;

		public HauntedWasteland(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var puzzle = new AoC.App._2023_08.HauntedWasteland();
			var input = InputProvider.Read(AppSettings.InputDirectory, puzzle.Year, puzzle.Day, ".sample");

			_graph = input.Skip(2)
				.ToDictionary(
					x => x[..3],
					x => new GraphNode
					{
						Id = x[..3],
						Children = new string[] { x.Substring(7, 3), x.Substring(12, 3) }
					});

			GraphLayout_Random(_graph, new v2i(-128), new v2i(128));
		}

		protected override void Update()
		{
			var p = Grid.MousePositionDiscrete;
			_selected = _graph
				.FirstOrDefault(x =>
				{
					var dist = (x.Value.Meta.Position - p).Abs();
					return dist.X <= _pickZone && dist.Y <= _pickZone;
				})
				.Key;

			if (_moving)
			{
				_graph[_selected].Meta.Position = p;
			}
		}

		protected override void Render()
		{
			Grid.Draw();

			RenderGraph(_graph);

			Grid.DrawMouseCursor(style: GridCursorStyles.FullRowColumn);

			if (_selected != null)
			{
				var node = _graph[_selected];
				var p = node.Meta.Position;
				var fill = Colors.Yellow;
				fill.W = .2;
				Grid.DrawRect((int)p.X - _pickZone, (int)p.Y - _pickZone, (_pickZone * 2) + 1, (_pickZone * 2) + 1, Colors.Red, fill);

				Sdl2Wrapper.DrawText(new v2i(32, 32), null,
					new Text($"{nameof(_selected)}: {_selected?.ToString()}"),
					new Text($"{nameof(GraphNodeMeta.Position)}: {_graph[_selected].Meta.Position}")
					);
			}

			base.Render();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			if (_selected != null)
			{
				_moving = true;
			}
		}

		protected override void LeftMouseUp()
		{
			if (_moving)
			{
				_moving = false;
			}
		}

		private static void GraphLayout_Random(Dictionary<string, GraphNode> graph, v2i from, v2i to)
		{
			var rng = new Random((int)DateTime.Now.Ticks);
			foreach (var node in graph)
			{
				node.Value.Meta = new GraphNodeMeta
				{
					Position = new v2i(rng.Next((int)from.X, (int)to.X), rng.Next((int)from.Y, (int)to.Y))
				};
			}
		}

		private void RenderGraph(Dictionary<string, GraphNode> graph)
		{
			var lineColor = Colors.DarkGray;
			lineColor.W = .8;

			foreach (var node in graph.Values)
			{
				foreach (var child in node.Children)
				{
					var from = node.Meta.Position;
					var to = graph[child].Meta.Position;
					Grid.DrawLine((int)from.X, (int)from.Y, (int)to.X, (int)to.Y, lineColor);
				}
			}

			foreach (var node in graph.Values)
				Grid.DrawCell(node.Meta.Position, Colors.White, Colors.LightGray);
		}
	}
}
