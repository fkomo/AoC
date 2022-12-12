using System.Numerics;
using Ujeby.AoC.App.Year2022.Day09;

namespace Ujeby.AoC.Vis.App
{
	internal class Chitron : BaseLoop
	{
		private int[,] _riskMap;
		private long[,] _dist;
		private (int x, int y)[] _path;

		protected override void Init()
		{
			SDL2.SDL.SDL_ShowCursor(0);

			var input = new AoC.App.Year2021.Day15.Chitron().ReadInput();

			_riskMap = AoC.App.Year2021.Day15.Chitron.CreateRiskMap(input, input.Length);
			_riskMap = AoC.App.Year2021.Day15.Chitron.EnlargeRiskMap(_riskMap, input.Length, 5);

			AoC.App.Year2021.Day15.Chitron.Dijkstra(_riskMap, (0,0), out _dist);

			var size = _riskMap.GetLength(0);
			var x = size - 1;
			var y = size - 1;

			var tmp = new List<(int x, int y)>
			{
				(x, y)
			};

			// TODO Chitron visualization _path
			_path = tmp.ToArray();

			_gridSize = 8;
		}

		protected override void Update()
		{
			var m = _mouseGrid / _gridSize;
			_title += $" risk[{(int)m.X}x{(int)-m.Y}]";
			if ((int)m.X >= 0 && (int)m.X < _riskMap.GetLength(0) && (int)-m.Y >= 0 && (int)-m.Y < _riskMap.GetLength(0))
			{
				_title += $"={_riskMap[(int)-m.Y, (int)m.X]}";
				_title += $" dist[{_dist[(int)-m.Y, (int)m.X]}]";
			}
			_title += $" total={_dist[_dist.GetLength(0) - 1, _dist.GetLength(0) - 1]}";
		}

		protected override void Render()
		{
			DrawGrid();

			for (var y = 0; y < _riskMap.GetLength(1); y++)
				for (var x = 0; x < _riskMap.GetLength(0); x++)
				{
					var c = (byte)(0xff / (10 - _riskMap[y, x]));
					DrawGridCell(x, -y, c, c, c, 0x77);
				}

			foreach (var (x, y) in _path)
				DrawGridCell(x, -y, 0x00, 0xff, 0x00, 0xaa);

			// mouse cursor
			var mouseCursorOnGrid = _mouseGrid / _gridSize;
			DrawGridCell((int)mouseCursorOnGrid.X, (int)mouseCursorOnGrid.Y, 0xff, 0x00, 0x00, 0xaa);
		}

		protected override void LeftMouseDown(Vector2 position)
		{
		}

		protected override void LeftMouseUp(Vector2 position)
		{

		}
	}
}
