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
	internal class ReservoirResearch : AoCRunnable
	{
		v2i[] _clay;

		char[][] _map;
		aab2i _area;
		Queue<v2i> _springs;
		int _w = 0;
		private int _minY;

		public override string Name => $"#17 {nameof(ReservoirResearch)}";

		public ReservoirResearch(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			Reset();

			Grid.MinorSize = 8;
			Grid.MoveCenter(new v2i(_map[0].Length / 2 * Grid.MinorSize, 200));
		}

		protected override void Update()
		{
			Ujeby.AoC.App._2018_17.ReservoirResearch.ProcessSpring(_map, _springs, _area);

			if (_springs.Count == 0)
			{
				var before = _w;

				_w = _map.Skip(_minY).Sum(x => x.Count(xx => xx == '~' || xx == '|'));
				if (_w != before)
				{
					_map.Find('+', out v2i spring);
					_springs.Enqueue(spring);
				}
			}
		}

		protected override void Render()
		{
			Grid.Draw(showMinor: false, showMajor: false);

			foreach (var t in _map.ToAAB2i().EnumPoints())
			{
				var tile = _map.Get(t);
				if (tile == '\0')
					continue;

				var color = v4f.Zero;
				if (tile == '#')
					color = new v4f(.5, .5, .5, .8);
				else if (tile == '~')
					color = new v4f(.2, .2, 1, .5);
				else if (tile == '|')
					color = new v4f(.2, .2, 1, .5 + Random.Shared.NextDouble() * .4);

				Grid.DrawCell(t, fill: color);
			}

			Grid.DrawCells(_springs, border: new v4f(1, 1, 0, 1));

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_clay)}: {_clay.Length}"),
				new Text($"{nameof(_springs)}: {_springs.Count}"),
				new Text($"water: {_w}"),
				new Text($"flowing: {_map.Skip(_minY).Sum(x => x.Count(xx => xx == '|'))}"),
				new Text($"still: {_map.Skip(_minY).Sum(x => x.Count(xx => xx == '~'))}")
				);

			base.Render();
		}

		protected override void LeftMouseUp()
		{
			Reset();
		}

		private void Reset()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, 2018, 17);

			_map = Ujeby.AoC.App._2018_17.ReservoirResearch.CreateMap(input, new v2i(500, 0));
			_clay = [.. _map.EnumAll('#')];

			_area = _map.ToAAB2i();
			_springs = new Queue<v2i>();

			_map.Find('+', out v2i spring);
			_springs.Enqueue(spring);

			_minY = 0;
			for (_minY = 0; _minY < _map.Length; _minY++)
			{
				if (_map[_minY].Contains('#'))
					break;
			}
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
