using Ujeby.AoC.App.Year2021.Day15;
using Ujeby.AoC.App.Year2022.Day12;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class HillClimbingAlgorithm : Sdl2Loop
	{
		private int[,] _heightMap;

		private const int _stepsPerFrame = 10;

		private BreadthFirstSearch _bfs;
		private v2i[] _bfsPath = null;

		private v2i _start;
		private v2i _end;

		public HillClimbingAlgorithm(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(false);

			var input = new AoC.App.Year2022.Day12.HillClimbingAlgorithm().ReadInput();
			_heightMap = AoC.App.Year2022.Day12.HillClimbingAlgorithm.CreateHeightMap(input, out _start, out _end);
			
			MinorGridSize = 10;
			MoveGridCenter(new v2i(_heightMap.GetLength(1), _heightMap.GetLength(0)) / 2 * MinorGridSize);

			_bfs = new BreadthFirstSearch(_heightMap, _start, AoC.App.Year2022.Day12.HillClimbingAlgorithm.CheckHeight);
		}

		protected override void Update()
		{
			if (_bfsPath == null && _bfs != null)
			{
				for (var i = 0; i < _stepsPerFrame; i++)
					if (!_bfs.Step())
						_bfsPath = _bfs.Path(_end);
			}
		}

		protected override void Render()
		{
			var ui = new List<TextLine>();

			DrawGrid();

			// height map
			if (_heightMap != null)
			{
				var maxHeight = 'z' - 'a' + 2;
				for (var y = 0; y < _heightMap.GetLength(0); y++)
					for (var x = 0; x < _heightMap.GetLength(1); x++)
						DrawGridCell(x, y, fill: HeatMap.GetColorForValue(_heightMap[y, x], maxHeight, 0.5f));

				if ((int)MouseGridPositionDiscrete.X >= 0 && (int)MouseGridPositionDiscrete.X < _heightMap.GetLength(1) &&
					(int)MouseGridPositionDiscrete.Y >= 0 && (int)MouseGridPositionDiscrete.Y < _heightMap.GetLength(0))
				{
					var height = _heightMap[(int)MouseGridPositionDiscrete.Y, (int)MouseGridPositionDiscrete.X];
					ui.Add(new Text($"height: {height}/{(char)('a' + height - 1)}"));
				}
			}

			// path
			if (_bfsPath != null)
			{
				ui.Add(new Text($"bfs path: {_bfsPath.Length}"));
				foreach (var p in _bfsPath)
					DrawGridCell((int)p.X, (int)p.Y, fill: new v4f(1, 1, 1, .5));
			}
			else if (_bfs != null)
			{
				for (var y = 0; y < _bfs.Size.Y; y++)
					for (var x = 0; x < _bfs.Size.X; x++)
						if (_bfs.Visited[y, x])
							DrawGridCell((int)x, (int)y, fill: new v4f(.5, .5, .5, .5));
			}

			DrawGridMouseCursor();

			DrawText(new v2i(32, 32), v2i.Zero, ui.ToArray());
		}

		protected override void Destroy()
		{
			ShowCursor();
		}

		protected override void LeftMouseDown()
		{
		}

		protected override void LeftMouseUp()
		{
			var m = MouseGridPositionDiscrete;

			if (_heightMap != null)
			{
				_heightMap[m.Y, m.X] = Math.Min('z' - 'a', _heightMap[m.Y, m.X] + 1);

				_bfsPath = null;
				_bfs = new BreadthFirstSearch(_heightMap, _start, AoC.App.Year2022.Day12.HillClimbingAlgorithm.CheckHeight);
			}
		}
	}
}
