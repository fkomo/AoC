using Ujeby.Graphics;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App.Ui
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

	public class Graph
	{
		public int PickZone { get; set; } = 5;

		string[] _selected = [];
		bool _moving = false;
		bool _selecting = false;
		string _underCursor;

		v2i _movingStart;

		v2i _selectionStart;
		aab2i _selection;

		public Dictionary<string, GraphNode> Data { get; private set; }

		string UnderCursor(v2i mousePosition)
			=> Data.Keys.FirstOrDefault(x => (Data[x].Meta.Position - mousePosition).ManhLength() < PickZone);

		public void LeftMouseDown(WorkspaceGrid grid, v2i mousePosition)
		{
			if (!_selecting && !_moving && _selected.Length > 0 && _underCursor != null && _selected.Contains(_underCursor))
			{
				_moving = true;
				_movingStart = grid.MousePositionDiscrete;
			}

			if (!_selecting && !_moving)
			{
				_selecting = true;
				_selectionStart = grid.MousePositionDiscrete;
			}
		}

		public void LeftMouseUp(WorkspaceGrid grid, v2i mousePosition)
		{
			_moving = false;
			_selecting = false;
		}

		public void Update(WorkspaceGrid grid, v2i mousePosition)
		{
			var p = grid.MousePositionDiscrete;
			_underCursor = UnderCursor(p);

			if (_moving)
			{
				foreach (var s in _selected)
					Data[s].Meta.Position += p - _movingStart;

				_movingStart = p;
			}
			else if (_selecting)
			{
				_selection = new aab2i([_selectionStart, grid.MousePositionDiscrete]);
				Console.WriteLine($"{_selection}");

				_selected = Data
					.Where(x => _selection.Contains(x.Value.Meta.Position))
					.Select(x => x.Key)
					.ToArray();
			}
			else
			{
				if (_selected.Length == 0 && _underCursor != null)
					_selected = [_underCursor];
			}
			//else
			//{
			//	_selected = new string[]
			//	{
			//		Data.FirstOrDefault(x =>
			//		{
			//			var dist = (x.Value.Meta.Position - p).Abs();
			//			return dist.X <= PickZone && dist.Y <= PickZone;
			//		})
			//		.Key
			//	};
			//}
		}

		public void Render(WorkspaceGrid grid, v2i? uiTopLeft = null)
		{
			var lineColor = Colors.DarkGray;
			lineColor.W = .8;

			foreach (var node in Data.Values)
			{
				foreach (var child in node.Children)
				{
					if (!Data.ContainsKey(child))
						continue;

					var from = node.Meta.Position;
					var to = Data[child].Meta.Position;
					grid.DrawLine((int)from.X, (int)from.Y, (int)to.X, (int)to.Y, lineColor);
				}
			}

			foreach (var node in Data.Values)
				grid.DrawCell(node.Meta.Position, Colors.White, Colors.LightGray);

			if (_selected.Length > 0)
			{
				foreach (var s in _selected)
				{
					var p = Data[s].Meta.Position;
					var fill = Colors.Yellow;
					fill.W = .5;

					DrawManhLength(grid, p, PickZone, fill);
					//grid.DrawRect((int)p.X - PickZone, (int)p.Y - PickZone, PickZone * 2 + 1, PickZone * 2 + 1, Colors.Red, fill);
				}

				if (uiTopLeft.HasValue)
				{
					//Sdl2Wrapper.DrawText(uiTopLeft.Value, null,
					//	new Text($"{nameof(_selected)}: {_selected?.ToString()}"),
					//	new Text($"{nameof(GraphNodeMeta.Position)}: {Data[_selected].Meta.Position}")
					//	);
				}
			}

			if (_underCursor != null)
			{
				var p = Data[_underCursor].Meta.Position;
				var fill = Colors.Red;
				fill.W = .5;
				DrawManhLength(grid, p, PickZone, fill);
				//grid.DrawRect((int)p.X - PickZone, (int)p.Y - PickZone, PickZone * 2 + 1, PickZone * 2 + 1, Colors.Red, fill);
			}

			if (_selecting)
			{
				var c1 = Colors.White;
				c1.W = .5;
				var c2 = Colors.Blue;
				c2.W = .5;

				grid.DrawRect(_selection.Min, _selection.Size, border: c1, fill: c2);
			}
		}

		static void DrawManhLength(WorkspaceGrid grid, v2i center, int length, v4f fill) => 
			grid.DrawCells(new aab2i(new v2i(-length), new v2i(length)).EnumPoints().Where(x => x.ManhLength() < length).Select(x => x + center), fill: fill);

		public void GraphLayout_Random(v2i from, v2i to)
		{
			var rng = new Random((int)DateTime.Now.Ticks);
			foreach (var node in Data)
			{
				node.Value.Meta = new GraphNodeMeta
				{
					Position = new v2i(rng.Next((int)from.X, (int)to.X), rng.Next((int)from.Y, (int)to.Y))
				};
			}
		}

		public void SetData(Dictionary<string, GraphNode> data)
		{
			Data = data;
		}
	}
}
