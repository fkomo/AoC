using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class ARegularMap : AoCRunnable
	{
		int _currentRegex = -1;
		string _regex;

		HashSet<v2i> _doors;
		HashSet<v2i> _rooms;
		Dictionary<v2i, int> _distance = [];

		public override string Name => $"#20 {nameof(ARegularMap)}";

		public ARegularMap(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
			_updateAfter = 16;
		}

		protected override void Init()
		{
			LoadNext();

			//Grid.MinorSize = 18;
			//Grid.MoveCenter(new v2i(_map[0].Length / 2 * Grid.MinorSize));
		}

		protected override void Update()
		{
		}

		protected override void Render()
		{
			Grid.Draw(showMinor: true, showMajor: true);

			var map = aab2i.FromPoints([.. _rooms]);
			
			Grid.DrawCells(_doors, fill: new v4f(.5, .3, .1, .5));

			var maxDist = _distance.Max(x => x.Value);
			foreach (var r in _rooms)
				Grid.DrawCell(r, fill: HeatMap.GetColorForValue(_distance[r], maxDist, alpha: .5));

			var distOrdered = _distance.OrderByDescending(x => x.Value);
			Grid.DrawCell(distOrdered.Last().Key, border: new v4f(1, 1, 1, 1));
			Grid.DrawCell(distOrdered.First().Key, border: new v4f(1, 1, 0, 1));

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text(new string([.. _regex.Take(128)]) + (_regex.Length > 128 ? "..." : null)),
				new Text($"{nameof(_doors)}: {_doors.Count}"),
				new Text($"{nameof(_rooms)}: {_rooms.Count}"),
				new Text($"size: {map.Size}"),
				new Text($"{_distance.OrderByDescending(x => x.Value).First().Key}: {maxDist}")
				);

			base.Render();
		}

		protected override void LeftMouseUp()
		{
			LoadNext();
		}

		private void LoadNext()
		{
			//var input = InputProvider.Read(AppSettings.InputDirectory, 2018, 20, ".sample");
			var input = InputProvider.Read(AppSettings.InputDirectory, 2018, 20);

			_currentRegex = (_currentRegex + 1) % input.Length;
			_regex = input[_currentRegex];

			Ujeby.AoC.App._2018_20.ARegularMap.CreateMap(_regex, out _rooms, out _doors);

			_distance = Ujeby.Alg.Graph.SeedFillDistance(v2i.Zero, [.. v2i.UpDownLeftRight.Select(x => x * 2)],
				(from, to) => (!_rooms.Contains(to) || !_doors.Contains((to + from) / 2)) ? 0 : 1);
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}