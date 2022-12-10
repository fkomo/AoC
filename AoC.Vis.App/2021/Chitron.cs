using System.Numerics;

namespace Ujeby.AoC.Vis.App
{
	internal class Chitron : MainLoop
	{
		private int[,] _riskMap;

		protected override void Init()
		{
			SDL2.SDL.SDL_ShowCursor(0);

			var input = new AoC.App.Year2021.Day15.Chitron().ReadInput();
			var _smallRiskMap = AoC.App.Year2021.Day15.Chitron.CreateRiskMap(input, input.Length);
			_riskMap = AoC.App.Year2021.Day15.Chitron.EnlargeRiskMap(_smallRiskMap, input.Length, 5);

			_gridSize = 16;
		}

		protected override void Update()
		{
			var m = _mouseGrid / _gridSize;
			_title += $" r[{(int)m.X} x {(int)-m.Y}]";

			if ((int)m.X >= 0 && (int)m.X < _riskMap.GetLength(0) && (int)-m.Y >= 0 && (int)-m.Y < _riskMap.GetLength(0))
				_title += $" = {_riskMap[(int)-m.Y, (int)m.X]}";
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
