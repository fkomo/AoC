using System.Diagnostics;
using Ujeby.AoC.App._2018_13;
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
	internal class MineCartMadness : AoCRunnable
	{
		long _tick;
		char[][] _map;
		Cart[] _carts;
		Dictionary<Cart, v4f> _colors;

		readonly List<v2i> _collisions = [];

		const int _maxPathLength = 64;
		Dictionary<Cart, List<v2i>> _paths = [];

		const int _frameStep = 2;
		readonly Stopwatch _sw = Stopwatch.StartNew();

		public override string Name => $"#13 {nameof(MineCartMadness)}";

		public MineCartMadness(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			Grid.MinorSize = 4;

			Reset();
		}

		protected override void Update()
		{
			if (_carts.Length == 1)
				return;

			if (_sw.ElapsedMilliseconds > _frameStep)
			{
				_tick++;

				foreach (var c in _carts)
				{
					_paths[c].Add(c.Pos);
					if (_paths[c].Count > _maxPathLength)
						_paths[c].RemoveAt(0);
				}

				var cartBefore = _carts.ToArray();
				_carts = Ujeby.AoC.App._2018_13.MineCartMadness.MoveAndRemoveCollisionCarts(_carts, _map);

				_collisions.AddRange(cartBefore.Except(_carts).Select(x => x.Pos));

				_sw.Restart();
			}
		}

		protected override void Render()
		{
			Grid.Draw(showMinor: false);

			foreach (var t in _map.ToAAB2i().EnumPoints())
			{
				if (_map.Get(t) == '+')
					Grid.DrawCell(t, fill: new v4f(.5, .5, .5, .4));
				
				else if (_map.Get(t) != ' ')
					Grid.DrawCell(t, fill: new v4f(.5, .5, .5, .2));
			}

			foreach (var p in _paths)
			{
				var path = p.Value.AsEnumerable().Reverse().ToArray();
				for (var i = 0; i < path.Length; i++)
				{
					var pathColor = _colors[p.Key];
					pathColor.W = (pathColor.W / path.Length + 1) * (path.Length - i);

					Grid.DrawCell(path[i], fill: pathColor);
				}
			}

			foreach (var c in _carts)
				Grid.DrawCell(c.Pos, fill: _colors[c]);

			Grid.DrawCells(_collisions, fill: new v4f(1, 0, 0, 1), border: new v4f(1, 1, 0, 1));

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_tick)}: {_tick}")
				);

			base.Render();
		}

		protected override void LeftMouseUp()
		{
			Reset();
		}

		private void Reset()
		{
			//var input = InputProvider.Read(AppSettings.InputDirectory, 2018, 13, suffix: ".sample");
			var input = InputProvider.Read(AppSettings.InputDirectory, 2018, 13);

			Ujeby.AoC.App._2018_13.MineCartMadness.ParseInput(input, out _carts, out _map);

			_tick = 0;
			_colors = _carts.ToDictionary(x => x, x => new v4f(v3f.FromRGB(Random.Shared.Next()), 1));

			_paths = _carts.ToDictionary(x => x, x => new List<v2i>());

			_collisions.Clear();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
