using System.Diagnostics;
using Ujeby.AoC.App._2024_14;
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
	internal class RestroomRedoubt : AoCRunnable
	{
		long _second;
		v2i _spaceSize;
		Robot[] _robots;
		v4f[] _colors;
		v2i _mid;

		long _dists;

		const int _frameStep = 1;
		readonly Stopwatch _sw = Stopwatch.StartNew();

		public override string Name => $"#14 {nameof(RestroomRedoubt)}";

		public RestroomRedoubt(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, 2024, 14);

			_spaceSize = new v2i(101, 103);
			_mid = _spaceSize / 2;

			_second = 0;
			_robots = input.Select(x => x.ToNumArray()).Select(x => new Robot(new v2i(x[0], x[1]), new v2i(x[2], x[3]))).ToArray();
			_colors = _robots.Select(x => new v4f(v3f.FromRGB(Random.Shared.Next()), .5)).ToArray();
		}

		protected override void Update()
		{
			var robotsAt = _robots.Select(x => (((x.Pos + x.Vel * _second) % _spaceSize) + _spaceSize) % _spaceSize).ToArray();

			var minLineLength = 10;
			for (var i = 0; i < robotsAt.Length; i++)
			{
				var lineFound = true;
				for (var len = 0; len < minLineLength; len++)
				{
					if (!robotsAt.Any(x => x == robotsAt[i] + new v2i(len, 0)))
					{
						lineFound = false;
						break;
					}
				}

				if (lineFound)
					return;
			}

			_second += 10;
		}

		protected override void Render()
		{
			Grid.Draw(showMinor: false);

			for (var i = 0; i < _robots.Length; i++)
			{
				var pos = (((_robots[i].Pos + _robots[i].Vel * _second) % _spaceSize) + _spaceSize) % _spaceSize;
				Grid.DrawCell(pos - _mid, fill: _colors[i]);
			}

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_second)}: {_second}"),
				new Text($"{nameof(_dists)}: {_dists}")
				);

			base.Render();
		}

		protected override void LeftMouseDown()
		{
			_second++;
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
