using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class GridComputing : AoCRunnable
	{
		private v4i[] _gridNodes;
		private v4i? _selected;

		public override string Name => $"#22 {nameof(GridComputing)}";

		public GridComputing(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, 2016, 22);
			_gridNodes = input.Skip(2).Select(x =>
			{
				var n = x.ToNumArray();
				return new v4i(n[0], n[1], n[2], n[5]);
			}).ToArray();
		}

		protected override void Update()
		{
			var p = Grid.MousePositionDiscrete;
			_selected = _gridNodes.SingleOrDefault(x => x.X == p.X && x.Y == p.Y);
		}

		protected override void Render()
		{
			Grid.Draw();

			foreach (var gridNode in _gridNodes)
				Grid.DrawCell(gridNode.ToV2i(), fill: HeatMap.GetColorForValue(gridNode.W, 100, .5));

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_gridNodes)}: {_gridNodes.Length}"),
				new Text($"{nameof(_selected)}: {_selected?.ToString()}")
				);

			base.Render();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
