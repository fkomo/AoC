using System.Diagnostics;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class MonkeyMap : Sdl2Loop
	{
		private char[][] _map;

		private int _pathToDraw = 0;
		private v3i[] _path;

		private Stopwatch _stopwatch = Stopwatch.StartNew();

		private int _userPathToDraw = 0;
		private int _userPoints = 0;
		private v3i _userStart;
		private v3i _userEnd;
		private v3i[] _userPath;

		public MonkeyMap(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(false);

			var input = new AoC.App.Year2022.Day22.MonkeyMap().ReadInput();
			_map = AoC.App.Year2022.Day22.MonkeyMap.CreateMap(input);

			_path = AoC.App.Year2022.Day22.MonkeyMap.Travel(
				_map, 
				AoC.App.Year2022.Day22.MonkeyMap.ReadDirections(input),
				new(Array.IndexOf(_map[0], '.'), 0, 0));

			MinorGridSize = 5;
			GridOffset -= new v2i(_map[0].Length, _map.Length) * MinorGridSize / 2;
		}

		protected override void Update()
		{
			if (_userPath == null && _userPoints == 2)
			{
				long distance = 0;
				if (_userStart.X == _userEnd.X)
				{
					distance = Math.Abs(_userEnd.Y - _userStart.Y);
					_userStart.Z = _userEnd.Y > _userStart.Y ? 1 : 3;
				}
				else if (_userStart.Y == _userEnd.Y)
				{
					distance = Math.Abs(_userEnd.X - _userStart.X);
					_userStart.Z = _userEnd.X > _userStart.X ? 0 : 2;
				}

				if (distance > 0)
				{
					_userPath = AoC.App.Year2022.Day22.MonkeyMap.Travel(
						_map,
						new string[] { distance.ToString() },
						_userStart);
					_userPathToDraw = 0;
				}
				else
				{
					_userPath = null;
					_userPathToDraw = 0;
				}
				
				_userPoints = 0;
			}

			if (_stopwatch.ElapsedMilliseconds > 1)
			{
				if (_pathToDraw < _path.Length)
					_pathToDraw++;

				if (_userPath != null && _userPathToDraw < _userPath.Length)
					_userPathToDraw++;

				_stopwatch.Restart();
			}
		}

		protected override void Render()
		{
			DrawGrid(showMain: false, showMajor: false);

			DrawGridRect(0, 0, _map[0].Length, _map.Length, new v4f(1));

			// map
			for (var y = 0; y < _map.Length; y++)
				for (var x = 0; x < _map[0].Length; x++)
				{
					var color = Colors.White;
					switch (_map[y][x])
					{
						case ' ': continue;
						case '#': color = new v4f(0.8f); break;
						case '.': color = new v4f(0.3f); break;
					};

					DrawGridCellFill(x, y, color);
				}

			// path
			for (var i = Math.Max(0, _pathToDraw - 100); i < _pathToDraw; i++)
				DrawGridCellFill((int)_path[i].X, (int)_path[i].Y, HeatMap.GetColorForValue(i, _path.Length, 0.5f));

			// userPath
			if (_userPoints != 0)
			{
				if (_userPoints > 0)
					DrawGridCellFill((int)_userStart.X, (int)_userStart.Y, new v4f(0, 0, 1, 1));
				if (_userPoints > 1)
					DrawGridCellFill((int)_userEnd.X, (int)_userEnd.Y, new v4f(1, 0, 0, 1));
			}
			for (var i = 0; i < _userPathToDraw; i++)
				DrawGridCellFill((int)_userPath[i].X, (int)_userPath[i].Y, HeatMap.GetColorForValue(i, _userPath.Length, 0.5f));
			DrawGridMouseCursor();

			var p = _path.Last();
			DrawText(new v2i(32, 32), v2i.Zero, 
				new Text($"password = {1000 * (p.Y + 1) + 4 * (p.X + 1) + p.Z} (1000 * {p.Y + 1} + 4 * {p.X + 1} + {p.Z})"));
		}
		protected override void Destroy()
		{
			ShowCursor();
		}

		protected override void LeftMouseUp(v2i position)
		{
			//_pathToDraw = Math.Max(_pathToDraw - 10, 0);

			if (_userPoints == 1)
			{
				_userEnd = new v3i(MouseGridPositionDiscrete.X, MouseGridPositionDiscrete.Y, 0);
				_userPoints = 2;
				_userPath = null;
			}
			else
			{
				_userStart = new v3i(MouseGridPositionDiscrete.X, MouseGridPositionDiscrete.Y, 0);
				_userPoints = 1;
			}
		}
	}
}
