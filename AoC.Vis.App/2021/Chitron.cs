using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
    internal class Chitron : AoCRunnable
	{
		private int[,] _riskMap;

		private readonly bool _useAStar = true;
		private readonly bool _useDijkstra = true;

		private const int _stepsPerFrame = 10;

		private Alg.Dijkstra _dijkstra;
		private v2i[] _dijkstraPath = null;

		private Alg.AStar _aStar;
		private v2i[] _aStarPath = null;

		private int _userPoints = 0;
		private v2i _start = new();
		private v2i _end = new();

		public override string Name => $"#15 {nameof(Chitron)}";

		public Chitron(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			Sdl2Wrapper.ShowCursor(false);

			var input = InputProvider.Read(AppSettings.InputDirectory, 2021, 15);

			_riskMap = AoC.App._2021_15.Chitron.CreateRiskMap(input, input.Length);
			_riskMap = AoC.App._2021_15.Chitron.EnlargeRiskMap(_riskMap, input.Length, 5);

			_start = new(0, 0);
			_end = new(_riskMap.GetLength(0) - 1, _riskMap.GetLength(0) - 1);

			if (_useDijkstra)
				_dijkstra = new Alg.Dijkstra(_riskMap, _start, _end);
	
			if (_useAStar)
				_aStar = new Alg.AStar(_riskMap, _start, _end, (a, b) => v2i.ManhDistance(a, b));

			Grid.MinorSize = 2;
			Grid.MoveCenter(new v2i(_riskMap.GetLength(1), _riskMap.GetLength(0)) / 2 * Grid.MinorSize);
		}

		protected override void Update()
		{
			if (_useAStar && _aStarPath == null && _aStar != null)
			{
				for (var i = 0; i < _stepsPerFrame; i++)
					if (!_aStar.Step())
					{
						_aStarPath = _aStar.Path();
						break;
					}
			}

			if (_useDijkstra && _dijkstraPath == null && _dijkstra != null)
			{
				for (var i = 0; i < _stepsPerFrame; i++)
					if (!_dijkstra.Step())
					{
						_dijkstraPath = _dijkstra.Path();
						break;
					}
			}
		}

		protected override void Render()
		{
			Grid.Draw();

			for (var y = 0; y < _riskMap.GetLength(1); y++)
				for (var x = 0; x < _riskMap.GetLength(0); x++)
				{
					var color = new v4f(1.0 / (10 - _riskMap[y, x]))
					{
						W = 0.5f
					};

					Grid.DrawCell(x, y, fill: color);
				}

			if (_userPoints != 0)
			{
				if (_userPoints > 0)
					Grid.DrawCell((int)_start.X, (int)_start.Y, fill: new v4f(0, 0, 1, 1));
				if (_userPoints > 1)
					Grid.DrawCell((int)_end.X, (int)_end.Y, fill: new v4f(1, 0, 0, 1));
			}

			if (_useDijkstra)
			{
				if (_dijkstraPath != null)
					foreach (var p in _dijkstraPath)
						Grid.DrawCell((int)p.X, (int)p.Y, fill: new v4f(0, 1, 0, .5));

				else if (_dijkstra != null)
				{
					for (var y = 0; y < _dijkstra.Size.Y; y++)
						for (var x = 0; x < _dijkstra.Size.X; x++)
							if (_dijkstra.Visited[y, x])
								Grid.DrawCell((int)x, (int)y, fill: new v4f(0, 1, 0, .5));
				}
			}

			if (_useAStar)
			{
				if (_aStarPath != null)
					foreach (var p in _aStarPath)
						Grid.DrawCell((int)p.X, (int)p.Y, fill: new v4f(0, 0, 1, .5));

				else if (_aStar != null)
				{
					for (var y = 0; y < _aStar.Size.Y; y++)
						for (var x = 0; x < _aStar.Size.X; x++)
							if (_aStar.Visited[y, x])
								Grid.DrawCell((int)x, (int)y, fill: new v4f(1, 0, 0, .5));
				}
			}

			Grid.DrawMouseCursor();

			var ui = new List<TextLine>();

			var m = Grid.MousePositionDiscrete;
			if ((int)m.X >= 0 && (int)m.X < _riskMap.GetLength(0) &&
				(int)m.Y >= 0 && (int)m.Y < _riskMap.GetLength(0))
			{
				ui.Add(new Text($"risk: {_riskMap[(int)m.Y, (int)m.X]}"));
			}

			Sdl2Wrapper.DrawText(new(32, 32), HorizontalTextAlign.Left, VerticalTextAlign.Top, ui.ToArray());
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}

		protected override void LeftMouseUp()
		{
			var m = Grid.MousePositionDiscrete;
			var size = new v2i(_riskMap.GetLength(0), _riskMap.GetLength(0));

			// user path
			if ((int)m.X >= 0 && (int)m.X < size.X && (int)m.Y >= 0 && (int)m.Y < size.Y)
			{
				if (_userPoints == 1)
				{
					_end = new(m.X, m.Y);
					_userPoints = 2;

					if (_useAStar)
					{
						_aStarPath = null;
						_aStar = new Alg.AStar(_riskMap, _start, _end, (a, b) => v2i.ManhDistance(a, b));
					}

					if (_useDijkstra)
					{
						_dijkstraPath = null;
						_dijkstra = new Alg.Dijkstra(_riskMap, _start, _end);
					}
				}
				else
				{
					_start = new(m.X, m.Y);
					_userPoints = 1;
				}
			}
		}
	}
}
