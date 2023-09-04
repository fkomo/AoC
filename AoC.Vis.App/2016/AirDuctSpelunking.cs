using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class AirDuctSpelunking : AoCRunnable
	{
		private char[][] _map;
		private v2i _mapSize;
		private char _selected;
		private Dictionary<int, v2i> _poi;

		private v2i[,][] _path;
		private int[,] _dist;
		private int[] _keys;
		private (int[] Path, long Length)? _shortest;
		private bool _return = false;

		public override string Name => $"#24 {nameof(AirDuctSpelunking)}";

		public AirDuctSpelunking(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, 2016, 24);
			_map = input.Select(y => y.ToArray()).ToArray();
			_mapSize = new v2i(_map[0].Length, _map.Length);

			Grid.MinorSize = 10;
			Grid.SetCenter(_mapSize / 2 * Grid.MinorSize);

			_poi = Ujeby.AoC.App._2016_24.AirDuctSpelunking.CreatePoi(_map, _mapSize);
			_keys = _poi.Keys.ToArray();

			Ujeby.AoC.App._2016_24.AirDuctSpelunking.CreatePaths(_poi, _map, _mapSize, 
				out _path, out _dist);

			_shortest = Ujeby.AoC.App._2016_24.AirDuctSpelunking.ShortestPath(_poi, _dist, new int[] { 0 });
		}

		protected override void Update()
		{
			var p = Grid.MousePositionDiscrete;

			_selected = (v2i.Zero <= p && p < _mapSize) ? _map[p.Y][p.X] : '?';
		}

		protected override void Render()
		{
			Grid.Draw();

			for (var y = 0; y < _mapSize.Y;y++)
			{
				for (var x = 0; x < _mapSize.X; x++)
				{
					var c = Colors.Black;
					
					if (_map[y][x] == '#')
						c = new v4f(1, 1, 1, .1);
					else if (_map[y][x] == '0')
						c = Colors.Green;
					else if (_map[y][x] != '.')
						c = Colors.Red;

					Grid.DrawCell(x, y, fill: c);
				}
			}

			//foreach (var key in _keys)
			//{
			//	if (key == 0)
			//		continue;
			//	var path = _path[0, key];
			//	var c = HeatMap.GetColorForValue(key, _keys.Length, alpha: 0.5);
			//	for (var i = 0; i < path.Length; i++)
			//		Grid.DrawCell(path[i], fill: c);
			//}

			if (_shortest != null)
			{
				for (var i = 0; i < _shortest.Value.Path.Length - 1; i++)
				{
					var path = _path[_shortest.Value.Path[i], _shortest.Value.Path[i + 1]];
					var c = HeatMap.GetColorForValue(i, _shortest.Value.Path.Length - 1, alpha: 0.5);
					for (var p = 0; p < path.Length; p++)
						Grid.DrawCell(path[p], fill: c);
				}
			}

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_mapSize)}: {_mapSize}"),
				new Text($"{nameof(_poi)}: {_poi.Count}"),
				new Text($"{nameof(_selected)}: {_selected}"),
				new Text($"{nameof(_return)}: {_return}"),
				new Text($"{nameof(_shortest.Value.Length)}: {_shortest.Value.Length}")
				);

			base.Render();
		}

		protected override void LeftMouseUp()
		{
			_return = !_return;
			_shortest = Ujeby.AoC.App._2016_24.AirDuctSpelunking.ShortestPath(_poi, _dist, new int[] { 0 }, ret: _return);
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
