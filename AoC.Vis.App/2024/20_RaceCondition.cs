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
		char[][] _map;
		v2i _start;
		v2i _end;

		int _pathToDraw;
		v2i[] _path = [];
		int[][] _metaMap;
		private int _maxCheat;
		private v2i[] _offsets;
		private (v2i, v2i)[] _allCheats;
		Dictionary<v2i, v2i[]> _cheats = [];

		int _step = 64;
		readonly Stopwatch _sw = new Stopwatch();

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
			_map.Find('S', out _start);
			_map.Find('E', out _end);

			_metaMap = AoC.App._2024_20.RaceCondition.MetaMap(_map, out _path);

			_maxCheat = 19;
			_offsets = new aab2i(new v2i(-_maxCheat), new v2i(_maxCheat))
				.EnumPoints()
				.Where(x => x.ManhLength() <= _maxCheat)
				.Except([v2i.Zero])
				.ToArray();

			_allCheats = AoC.App._2024_20.RaceCondition.AllCheats(_metaMap, _path, _maxCheat, minSavedTime: 100);
			_cheats = _allCheats
				.GroupBy(x => x.Item1)
				.ToDictionary(x => x.Key, x => x.Select(xx => xx.Item2).ToArray());

			_sw.Start();
		}

		protected override void Update()
		{
			if (_sw.ElapsedMilliseconds > _step)
			{
				_pathToDraw++;

				_sw.Restart();
			}
		}

		protected override void Render()
		{
			Grid.Draw();

			var color = new v4f(.2, .2, .2, .8);
			foreach (var p in _map.EnumAll('#'))
				Grid.DrawCell(p, fill: color);

			Grid.DrawCell(_start, fill: new v4f(0, .5, .5, .5));
			Grid.DrawCell(_end, fill: new v4f(.5, .5, 0, .5));

			var cheats = _cheats.Skip(System.Math.Min(_pathToDraw, _cheats.Count)).Take(1);
			foreach (var p in cheats)
			{
				foreach (var to in p.Value)
				{
					Grid.DrawCell(p.Key, fill: new v4f(0, .5, 0, .5));
					Grid.DrawCell(to, fill: new v4f(.5, 0, 0, .5));
					Grid.DrawLine(p.Key, to, new v4f(.5, .5, .5, .5));
				}
			}

			for (var i = 0; i < _offsets.Length; i++)
				Grid.DrawCell(_offsets[i] + cheats.First().Key, fill: new v4f(0, 0, .5, .5));

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			var distance = _metaMap.ToAAB2i().Contains(Grid.MousePositionDiscrete) ? _metaMap.Get(Grid.MousePositionDiscrete) : int.MinValue;

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_map)}: {_map.ToAAB2i()}"),
				new Text($"{nameof(_path)}: {_path.Length}"),
				new Text($"{nameof(_allCheats)}: {_allCheats.Length}"),
				new Text($"{nameof(distance)}: {distance}")
				);

			base.Render();
		}

		protected override void LeftMouseUp()
		{
			_pathToDraw = 0;
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
