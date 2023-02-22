using System.Diagnostics;
using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
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
		private v3i _userStart = new();
		private v3i _userEnd = new();
		private v3i[] _userPath;

		public override string Name => $"#22 {nameof(MonkeyMap)}";

		public MonkeyMap(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(false);

			var input = InputProvider.Read(AppSettings.InputDirectory, 2022, 22);
			_map = AoC.App._2022_22.MonkeyMap.CreateMap(input);

			_path = AoC.App._2022_22.MonkeyMap.Travel(
				_map, 
				AoC.App._2022_22.MonkeyMap.ReadDirections(input),
				new(Array.IndexOf(_map[0], '.'), 0, 0));

			Grid.MinorSize = 5;
			Grid.MoveCenter(new v2i(_map.First().Length, _map.Length) / 2 * Grid.MinorSize);
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
					_userPath = AoC.App._2022_22.MonkeyMap.Travel(
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

			if (_stopwatch.ElapsedMilliseconds > 10)
			{
				if (_pathToDraw < _path.Length)
				{
					Grid.SetCenter(_path[_pathToDraw].ToV2i() * Grid.MinorSize);
					_pathToDraw++;
				}

				if (_userPath != null && _userPathToDraw < _userPath.Length)
					_userPathToDraw++;

				_stopwatch.Restart();
			}
		}

		protected override void Render()
		{
			DrawGrid(showAxis: false, showMajor: false);

			// map
			for (var y = 0; y < _map.Length; y++)
				for (var x = 0; x < _map[0].Length; x++)
				{
					var color = Colors.White;
					switch (_map[y][x])
					{
						case ' ': continue;
						case '#': color = new v4f(0.9f); break;
						case '.': color = new v4f(0.2f); break;
					};

					DrawGridCell(x, y, fill: color);
				}

			// path
			for (var i = Math.Max(0, _pathToDraw - 1000); i < _pathToDraw; i++)
				DrawGridCell((int)_path[i].X, (int)_path[i].Y, fill: HeatMap.GetColorForValue(i, _path.Length, .5));

			// userPath
			if (_userPoints != 0)
			{
				if (_userPoints > 0)
					DrawGridCell((int)_userStart.X, (int)_userStart.Y, fill: new v4f(0, 0, 1, 1));
				if (_userPoints > 1)
					DrawGridCell((int)_userEnd.X, (int)_userEnd.Y, fill: new v4f(1, 0, 0, 1));
			}
			for (var i = 0; i < _userPathToDraw; i++)
				DrawGridCell((int)_userPath[i].X, (int)_userPath[i].Y, fill: HeatMap.GetColorForValue(i, _userPath.Length, 1));
			
			//DrawGridMouseCursor(style: GridCursorStyles.FullRowColumn);

			var p = _path.Last();
			DrawText(new v2i(32, 32), 
				new Text($"password: {1000 * (p.Y + 1) + 4 * (p.X + 1) + p.Z} (1000 * {p.Y + 1} + 4 * {p.X + 1} + {p.Z})"));
		}
		protected override void Destroy()
		{
			ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			_pathToDraw = Math.Max(_pathToDraw - 1, 0);
		}

		protected override void LeftMouseUp()
		{
			// user path
			//if (_userPoints == 1)
			//{
			//	_userEnd = new v3i(MouseGridPositionDiscrete.X, MouseGridPositionDiscrete.Y, 0);
			//	_userPoints = 2;
			//	_userPath = null;
			//}
			//else
			//{
			//	_userStart = new v3i(MouseGridPositionDiscrete.X, MouseGridPositionDiscrete.Y, 0);
			//	_userPoints = 1;
			//}
		}
	}
}
