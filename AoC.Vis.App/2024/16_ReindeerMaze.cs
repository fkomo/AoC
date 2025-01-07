using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class ReindeerMaze : AoCRunnable
	{
		char[][] _map;
		Dictionary<v2i, int> _visited = [];
		int _score;

		bool _showBest = false;
		v2i[] _bestPlaces = [];

		public override string Name => $"#16 {nameof(ReindeerMaze)}";

		public ReindeerMaze(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			//var input = InputProvider.Read(AppSettings.InputDirectory, 2024, 16, suffix: ".sample");
			var input = InputProvider.Read(AppSettings.InputDirectory, 2024, 16);

			_map = input.Select(x => x.ToArray()).ToArray();

			_map.Find('S', out v2i start);
			_map.Find('E', out v2i end);
			var nodes = new v2i[] { start, end }.Concat(_map.EnumAll('.')).ToArray();

			_visited = Ujeby.AoC.App._2024_16.ReindeerMaze.CustomBreadthFirstSearch(nodes, start);
			_score = _visited[end];

			_bestPlaces = Ujeby.AoC.App._2024_16.ReindeerMaze.PickBestPlaces(_visited, start, end);
		}

		protected override void Update()
		{
		}

		protected override void Render()
		{
			Grid.Draw();

			var color = new v4f(.5);
			foreach (var p in _map.ToAAB2i().EnumPoints().Where(x => _map.Get(x) == '#'))
				Grid.DrawCell(p, fill: color);

			if (_showBest)
			{
				color = new v4f(0, .8, 0, .5);
				foreach (var b in _bestPlaces)
					Grid.DrawCell(b, fill: color);
			}
			else
			{
				foreach (var v in _visited.Where(x => x.Value <= _score))
					Grid.DrawCell(v.Key, fill: HeatMap.GetColorForValue(v.Value, _score + 1, alpha: .5));
			}

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			_visited.TryGetValue(Grid.MousePositionDiscrete, out int nodeScore);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_map)}: {_map.ToAAB2i()}"),
				new Text($"{nameof(_score)}: {_score}"),
				new Text($"{nameof(nodeScore)}: {nodeScore}"),
				new Text($"{nameof(_bestPlaces)}: {_bestPlaces.Length}")
				);

			base.Render();
		}

		protected override void LeftMouseUp()
		{
			_showBest = !_showBest;
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
