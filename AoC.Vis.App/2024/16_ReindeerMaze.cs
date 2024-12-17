using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Grid.CharMapExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class ReindeerMaze : AoCRunnable
	{
		char[][] _map;
		v2i[] _path;

		public override string Name => $"#16 {nameof(ReindeerMaze)}";

		public ReindeerMaze(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			//var input = InputProvider.Read(AppSettings.InputDirectory, 2024, 16, suffix: ".sample");
			var input = InputProvider.Read(AppSettings.InputDirectory, 2024, 16);

			_map = input.Select(x => x.ToArray()).ToArray();
			_path = AoC.App._2024_16.ReindeerMaze.ShortestPath(_map);
		}

		protected override void Update()
		{
		}

		protected override void Render()
		{
			Grid.Draw();

			foreach (var p in _map.ToAAB2i().EnumPoints())
			{
				var color = new v4f(.5);
				if (_map.Get(p) == '#')
					Grid.DrawCell(p, fill: color);
			}

			for (var i = 0; i < _path.Length; i++)
				Grid.DrawCell(_path[i], fill: HeatMap.GetColorForValue(i, _path.Length, alpha: .5));

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_map)}: {_map.ToAAB2i()}")
				);

			base.Render();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
