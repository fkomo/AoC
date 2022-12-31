using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class Chitron : Sdl2Loop
	{
		private int[,] _riskMap;
		private long[,] _dist;
		private v2i[] _path;

		public Chitron(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(false);

			var input = new AoC.App.Year2021.Day15.Chitron().ReadInput();

			_riskMap = AoC.App.Year2021.Day15.Chitron.CreateRiskMap(input, input.Length);
			_riskMap = AoC.App.Year2021.Day15.Chitron.EnlargeRiskMap(_riskMap, input.Length, 5);

			_dist = AoC.App.Year2021.Day15.Dijkstra.Create(_riskMap, new());
			_path = AoC.App.Year2021.Day15.Dijkstra.Path(new(), new(_riskMap.GetLength(0) - 1, _riskMap.GetLength(0) - 1), _riskMap, _dist);

			MinorGridSize = 8;

			// TODO Init with progressbar (iterate with update/render)
		}

		protected override void Update()
		{
		}

		protected override void Render()
		{
			DrawGrid();

			for (var y = 0; y < _riskMap.GetLength(1); y++)
				for (var x = 0; x < _riskMap.GetLength(0); x++)
				{
					var color = new v4f(1.0 / (10 - _riskMap[y, x]));
					color.W = 0.5f;

					DrawGridCell(x, y, fill: color);
				}

			foreach (var p in _path)
				DrawGridCell((int)p.X, (int)p.Y, fill: new v4f(0, 1, 0, 0.5f));

			DrawGridMouseCursor();

			var ui = new List<TextLine>
			{
				new Text($"path distance: {_dist[_dist.GetLength(0) - 1, _dist.GetLength(0) - 1]}")
			};

			if ((int)MouseGridPositionDiscrete.X >= 0 && (int)MouseGridPositionDiscrete.X < _riskMap.GetLength(0) && 
				(int)MouseGridPositionDiscrete.Y >= 0 && (int)MouseGridPositionDiscrete.Y < _riskMap.GetLength(0))
			{
				ui.Add(new Text($"risk: {_riskMap[(int)MouseGridPositionDiscrete.Y, (int)MouseGridPositionDiscrete.X]}"));
				ui.Add(new Text($"distance: {_dist[(int)MouseGridPositionDiscrete.Y, (int)MouseGridPositionDiscrete.X]}"));
			}

			DrawText(new v2i(32, 32), v2i.Zero, ui.ToArray());
		}

		protected override void Destroy()
		{
			ShowCursor();
		}
	}
}
