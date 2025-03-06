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
	internal class SettlersOfTheNorthPole : AoCRunnable
	{
		char[][] _map;
		char[][] _map2;
		v2i[] _acres = [];
		long _minute;

		public override string Name => $"#18 {nameof(SettlersOfTheNorthPole)}";

		public SettlersOfTheNorthPole(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
			_updateAfter = 16;
		}

		protected override void Init()
		{
			Reset();

			Grid.MinorSize = 18;
			Grid.MoveCenter(new v2i(_map[0].Length / 2 * Grid.MinorSize));
		}

		protected override void Update()
		{
			Ujeby.AoC.App._2018_18.SettlersOfTheNorthPole.WaitOneMinute(_map, _map2, _acres);
			(_map, _map2) = (_map2, _map);

			_minute++;
		}

		protected override void Render()
		{
			Grid.Draw(showMinor: true, showMajor: true);

			var rnd = new Random(123);
			foreach (var a in _acres)
			{
				var r = rnd.NextDouble();

				var tile = _map.Get(a);
				if (tile == '\0')
					continue;

				var color = v4f.Zero;
				if (tile == '#')
					color = new v4f(.7, .4, .1, .8);
				else if (tile == '|')
					color = new v4f(0, .2, 0, .8);
				else if (tile == '.')
					color = new v4f(.2, .8, .2, .6 + r * .2);

				Grid.DrawCell(a, fill: color);
			}

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_minute)}: {_minute}"),
				new Text($"resourceValue: {_map.Sum(x => x.Count(xx => xx == '|')) * _map.Sum(x => x.Count(xx => xx == '#'))}")
				);

			base.Render();
		}

		protected override void LeftMouseUp()
		{
			Reset();
		}

		private void Reset()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, 2018, 18);
			//var input = InputProvider.Read(AppSettings.InputDirectory, 2018, 18, ".sample");

			_map = Ujeby.AoC.App._2018_18.SettlersOfTheNorthPole.CreateMap(input);
			_map2 = [.. _map.Select(x => x.ToArray())];
			
			_acres = [.. new aab2i(new v2i(1), new v2i(_map.Length - 2)).EnumPoints()];
			_minute = 0;
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}