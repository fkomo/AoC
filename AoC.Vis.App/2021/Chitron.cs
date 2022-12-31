using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class Chitron : Sdl2Loop
	{
		private int[,] _riskMap;

		private long[,] _dijkstraDist;
		private v2i[] _dijkstraPath = null;

		private long[,] _aStarDist;
		private v2i[] _aStarPath = null;

		private int _userPoints = 0;
		private v2i _start = new();
		private v2i _end = new();

		public Chitron(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(false);

			var input = new AoC.App.Year2021.Day15.Chitron().ReadInput();

			_riskMap = AoC.App.Year2021.Day15.Chitron.CreateRiskMap(input, input.Length);
			_riskMap = AoC.App.Year2021.Day15.Chitron.EnlargeRiskMap(_riskMap, input.Length, 5);

			_start = new(0, 0);
			_end = new(_riskMap.GetLength(0) - 1, _riskMap.GetLength(0) - 1);

			MinorGridSize = 8;

			// TODO Init with progressbar (iterate with update/render)
		}

		protected override void Update()
		{
			if (_aStarPath == null)
			{
				_aStarDist = AoC.App.Year2021.Day15.AStar.Create(_riskMap, _start, _end, (a, b) => v2i.ManhDistance(a, b), out int[,] prev);
				_aStarPath = AoC.App.Year2021.Day15.Dijkstra.Path(_start, _end, prev);
			}

			if (_dijkstraPath == null)
			{
				_dijkstraDist = AoC.App.Year2021.Day15.Dijkstra.Create(_riskMap, _start, out int[,] prev);
				_dijkstraPath = AoC.App.Year2021.Day15.Dijkstra.Path(_start, _end, prev);
			}
		}

		protected override void Render()
		{
			DrawGrid();

			for (var y = 0; y < _riskMap.GetLength(1); y++)
				for (var x = 0; x < _riskMap.GetLength(0); x++)
				{
					var color = new v4f(1.0 / (10 - _riskMap[y, x]))
					{
						W = 0.5f
					};

					DrawGridCell(x, y, fill: color);
				}

			if (_userPoints != 0)
			{
				if (_userPoints > 0)
					DrawGridCell((int)_start.X, (int)_start.Y, fill: new v4f(0, 0, 1, 1));
				if (_userPoints > 1)
					DrawGridCell((int)_end.X, (int)_end.Y, fill: new v4f(1, 0, 0, 1));
			}

			if (_dijkstraPath != null)
				foreach (var p in _dijkstraPath)
					DrawGridCell((int)p.X, (int)p.Y, fill: new v4f(0, 1, 0, 0.5f));

			if (_aStarPath != null)
				foreach (var p in _aStarPath)
					DrawGridCell((int)p.X, (int)p.Y, fill: new v4f(1, 0, 0, 0.5f));

			DrawGridMouseCursor();

			var ui = new List<TextLine>
			{
				new Text($"path distance: {_dijkstraDist[_dijkstraDist.GetLength(0) - 1, _dijkstraDist.GetLength(0) - 1]}")
			};

			if ((int)MouseGridPositionDiscrete.X >= 0 && (int)MouseGridPositionDiscrete.X < _riskMap.GetLength(0) &&
				(int)MouseGridPositionDiscrete.Y >= 0 && (int)MouseGridPositionDiscrete.Y < _riskMap.GetLength(0))
			{
				ui.Add(new Text($"risk: {_riskMap[(int)MouseGridPositionDiscrete.Y, (int)MouseGridPositionDiscrete.X]}"));
				ui.Add(new Text($"distance: {_dijkstraDist[(int)MouseGridPositionDiscrete.Y, (int)MouseGridPositionDiscrete.X]}"));
			}

			DrawText(new v2i(32, 32), v2i.Zero, ui.ToArray());
		}

		protected override void Destroy()
		{
			ShowCursor();
		}

		protected override void LeftMouseUp()
		{
			// user path
			if (_userPoints == 1
				&& (int)MouseGridPositionDiscrete.X >= 0 && (int)MouseGridPositionDiscrete.X < _riskMap.GetLength(0)
				&& (int)MouseGridPositionDiscrete.Y >= 0 && (int)MouseGridPositionDiscrete.Y < _riskMap.GetLength(0))
			{
				_end = new(MouseGridPositionDiscrete.X, MouseGridPositionDiscrete.Y);
				_userPoints = 2;

				_aStarPath = null;
				_dijkstraPath = null;
			}
			else if (
				(int)MouseGridPositionDiscrete.X >= 0 && (int)MouseGridPositionDiscrete.X < _riskMap.GetLength(0) &&
				(int)MouseGridPositionDiscrete.Y >= 0 && (int)MouseGridPositionDiscrete.Y < _riskMap.GetLength(0))
			{
				_start = new(MouseGridPositionDiscrete.X, MouseGridPositionDiscrete.Y);
				_userPoints = 1;
			}
		}
	}
}
