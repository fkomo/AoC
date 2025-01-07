using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class GardenGroups : AoCRunnable
	{
		char[][] _map;
		aab2i _mapArea;
		Dictionary<char, v4f> _colors;

		public override string Name => $"#12 {nameof(GardenGroups)}";

		public GardenGroups(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, 2024, 12);
			_map = input.Select(x => x.ToCharArray()).ToArray();
			_mapArea = new aab2i(v2i.Zero, new v2i(_map.Length - 1));

			var plants = _mapArea.EnumPoints().Select(x => _map[x.Y][x.X]).Distinct().ToArray();
			_colors = plants.ToDictionary(x => x, x => new v4f(v3f.FromRGB(Random.Shared.Next()), .5));
		}

		protected override void Update()
		{
		}

		protected override void Render()
		{
			Grid.Draw(showMinor: false);

			foreach (var p in _mapArea.EnumPoints())
				Grid.DrawCell(p, fill: _colors[_map[p.Y][p.X]]);

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_mapArea)}: {_mapArea}")
				);

			base.Render();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
