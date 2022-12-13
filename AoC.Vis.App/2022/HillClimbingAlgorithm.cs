using System.Numerics;
using Ujeby.AoC.Vis.App.Common;

namespace Ujeby.AoC.Vis.App
{
	internal class HillClimbingAlgorithm : BaseLoop
	{
		private int[,] _heightMap;

		private long[,] _dijkstraDist;
		private (int x, int y)[] _dijkstraPath = Array.Empty<(int x, int y)>();

		private (int x, int y)[] _bfsPath = Array.Empty<(int x, int y)>();

		private (int x, int y) _start;
		private (int x, int y) _end;

		protected override void Init()
		{
			SDL2.SDL.SDL_ShowCursor(0);

			var input = new AoC.App.Year2022.Day12.HillClimbingAlgorithm().ReadInput();
			_heightMap = AoC.App.Year2022.Day12.HillClimbingAlgorithm.CreateHeightMap(input, out _start, out _end);

			_dijkstraDist = AoC.App.Year2021.Day15.Dijkstra.Create(_heightMap, _start,
				connectionCheck: AoC.App.Year2022.Day12.HillClimbingAlgorithm.CheckHeight);

			_dijkstraPath = AoC.App.Year2021.Day15.Dijkstra.Path(_start, _end, _heightMap, _dijkstraDist,
				connectionCheck: AoC.App.Year2022.Day12.HillClimbingAlgorithm.CheckHeight);

			Console.WriteLine($"shortest dijkstra path={_dijkstraPath.Length}");

			var bfsPrev = AoC.App.Year2022.Day12.BreadthFirstSearch.Create(_heightMap, _start,
				connectionCheck: AoC.App.Year2022.Day12.HillClimbingAlgorithm.CheckHeight);
			_bfsPath = AoC.App.Year2022.Day12.BreadthFirstSearch.Path(_start, _end, bfsPrev);
			
			Console.WriteLine($"shortest bfs path={_bfsPath.Length}");

			_gridSize = 10;
			_gridOffset = new Vector2(-_heightMap.GetLength(1) / 2 * _gridSize, -_heightMap.GetLength(0) / 2 * _gridSize);
		}

		protected override void Update()
		{
			var m = _mouseGrid / _gridSize;
			_title += $" height[{(int)m.X}x{(int)-m.Y}]";
			if ((int)m.X >= 0 && (int)m.X < _heightMap.GetLength(1) && (int)-m.Y >= 0 && (int)-m.Y < _heightMap.GetLength(0))
			{
				var height = _heightMap[(int)-m.Y, (int)m.X];
				_title += $"={height}/{(char)('a' + height - 1)}";
			}
		}

		protected override void Render()
		{
			DrawGrid(showMain: true, showMajor: true, showMinor: true);

			// height map
			var maxHeight = 'z' - 'a' + 2;
			for (var y = 0; y < _heightMap.GetLength(0); y++)
				for (var x = 0; x < _heightMap.GetLength(1); x++)
				{
					var color = HeatMap.GetColorForValue(_heightMap[y, x], maxHeight);
					DrawGridCell(x, -y, color.R, color.G, color.B, 0x77);
				}

			// dijkstra distance map
			//var maxDist = _dijkstraDist.Cast<long>().Where(l => l < long.MaxValue).Max() + 1;
			//for (var y = 0; y < _dijkstraDist.GetLength(0); y++)
			//	for (var x = 0; x < _dijkstraDist.GetLength(1); x++)
			//	{
			//		if (_dijkstraDist[y, x] == long.MaxValue)
			//			continue;
			//		var color = HeatMap.GetColorForValue(_dijkstraDist[y, x], maxDist);
			//		DrawGridCell(x, -y, color.R, color.G, color.B, 0x77);
			//	}

			DrawGridRect(0, 0, _heightMap.GetLength(1), _heightMap.GetLength(0), 0xff, 0xff, 0xff, 0xff,
				fill: false);

			// start
			DrawGridCell(_start.x, -_start.y, 0xff, 0xff, 0xff, 0xff);
			// end
			DrawGridCell(_end.x, -_end.y, 0xff, 0xff, 0xff, 0xff);

			// path
			foreach (var (x, y) in _bfsPath)
				DrawGridCell(x, -y, 0xff, 0xff, 0xff, 0x77);
			foreach (var (x, y) in _dijkstraPath)
				DrawGridCell(x, -y, 0xff, 0x00, 0x00, 0x77);

			// mouse cursor
			var mouseCursorOnGrid = _mouseGrid / _gridSize;
			DrawGridCell((int)mouseCursorOnGrid.X, (int)mouseCursorOnGrid.Y, 0xff, 0xff, 0x00, 0xff,
				fill: false);
		}

		protected override void LeftMouseDown(Vector2 position)
		{
		}

		protected override void LeftMouseUp(Vector2 position)
		{

		}
	}
}
