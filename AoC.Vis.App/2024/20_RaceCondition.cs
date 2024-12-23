using System.Diagnostics;
using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Extensions;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class RaceCondition : AoCRunnable
	{
		int[][] _metaMap;
		char[][] _map;

		bool _progressPath;
		int _pathToDraw;
		v2i[] _path = [];

		readonly HashSet<v2i> _cheats = [];

		v2i[] _offsets = [];

		public override string Name => $"#20 {nameof(RaceCondition)}";

		public RaceCondition(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			//var input = InputProvider.Read(AppSettings.InputDirectory, 2024, 20, suffix: ".sample");
			var input = InputProvider.Read(AppSettings.InputDirectory, 2024, 20);

			_map = input.Select(x => x.ToArray()).ToArray();

			_metaMap = AoC.App._2024_20.RaceCondition.MetaMap(_map, out _path);

			//_cheats = AoC.App._2024_20.RaceCondition.All2Cheats(_metaMap, _path, minSavedTime: 100);
			_offsets = new aab2i(new v2i(-20), new v2i(20)).EnumPoints().Where(x => x.ManhLength() <= 20).Except([v2i.Zero]).ToArray();
		}

		protected override void Update()
		{
			if (_progressPath)
			{
				_pathToDraw = (_pathToDraw + 1) % _path.Length;

				foreach (v2i p in _offsets)
				{
					var pp = _path[_pathToDraw] + p;
					if (_metaMap.ToAAB2i().Contains(pp) && _metaMap.Get(pp) >= _pathToDraw)
						_cheats.Add(pp);
				}
			}
		}

		protected override void Render()
		{
			Grid.Draw();

			var color = new v4f(.2, .2, .2, .8);
			foreach (var p in _map.EnumAll('#'))
				Grid.DrawCell(p, fill: color);

			color = new v4f(.5, .5, .5, .8);
			foreach (v2i p in _cheats)
				Grid.DrawCell(p, fill: color);

			for (var i = 0; i < _pathToDraw; i++)
				Grid.DrawCell(_path[i], fill: HeatMap.GetColorForValue(i, _pathToDraw + 1, alpha: .2));

			//color = new v4f(0, .8, .0, .5);
			//foreach (v2i p in _offsets)
			//{
			//	var pp = _path[_pathToDraw] + p;
			//	if (_metaMap.ToAAB2i().Contains(pp) && _metaMap.Get(pp) >= _pathToDraw)
			//		Grid.DrawCell(pp, fill: color);
			//}

			color = new v4f(0, .8, .0, .5);
			foreach (v2i p in _cheats)
				Grid.DrawCell(p, fill: color);

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_map)}: {_map.ToAAB2i()}"),
				new Text($"{nameof(_path)}: {_path.Length}"),
				new Text($"{nameof(_cheats)}: {_cheats.Count}")
				);

			base.Render();
		}

		protected override void LeftMouseUp()
		{
			_progressPath = !_progressPath;
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
