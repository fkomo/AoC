﻿using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class RaceCondition : AoCRunnable
	{
		char[][] _map;

		bool _showPath;
		int _pathToDraw;
		v2i[] _path = [];

		v2i[] _cheats = [];

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

			var metaMap = AoC.App._2024_20.RaceCondition.MetaMap(_map, out _path);

			_cheats = AoC.App._2024_20.RaceCondition.All2Cheats(metaMap, _path, minSavedTime: 100);
		}

		protected override void Update()
		{
			if (_showPath)
				_pathToDraw = (_pathToDraw + 1) % _path.Length;
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

			if (_showPath)
			{
				for (var i = 0; i < _pathToDraw; i++)
					Grid.DrawCell(_path[i], fill: HeatMap.GetColorForValue(i, _pathToDraw + 1, alpha: .5));
			}

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_map)}: {_map.ToAAB2i()}"),
				new Text($"{nameof(_path)}: {_path.Length}"),
				new Text($"{nameof(_cheats)}: {_cheats.Length}")
				);

			base.Render();
		}

		protected override void LeftMouseUp()
		{
			_showPath = !_showPath;
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
