﻿using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class RegolithReservoir : Sdl2Loop
	{
		private byte[,] _map;

		private Random _rnd;

		private long _sandCount;
		private const int _sandPerFrame = 10;

		private List<v4f> _colors = new();

		public RegolithReservoir(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(false);

			var input = new AoC.App.Year2022.Day14.RegolithReservoir().ReadInput();
			_map = AoC.App.Year2022.Day14.RegolithReservoir.CreateMap(input, ground: true);

			MinorGridSize = 5;
			MoveGridCenter(new v2i(_map.GetLength(1), _map.GetLength(0)) / 2 * MinorGridSize);

			_rnd = new Random(123);
			_sandCount = 0;
		}

		protected override void Update()
		{
			AddSand(new v2i(500, 0));
		}

		private void AddSand(v2i p)
		{
			p.Y = Math.Max(p.Y, 0);
			if (p.X < 0 || p.Y < 0 || p.X >= _map.GetLength(1) || p.Y >= _map.GetLength(0))
				return;

			for (var i = 0; i < _sandPerFrame && AoC.App.Year2022.Day14.RegolithReservoir.AddSand(((int)p.X, (int)p.Y), _map) != null; i++)
			{
				_colors.Add(new v4f(0.4 + _rnd.NextDouble() / 2, 0.3 + _rnd.NextDouble() / 2, 0, 0.8f));
				_sandCount++;
			}
		}

		protected override void Render()
		{
			DrawGrid(showMinor: false);

			var blockerColor = new v4f(1, 1, 1, .7f);

			// height map
			var sandIdx = 0;
			for (var y = _map.GetLength(0) - 1; y >= 0 ; y--)
				for (var x = 0; x < _map.GetLength(1); x++)
				{
					if (_map[y, x] == 0)
						continue;

					var color = blockerColor;
					if (_map[y, x] == (byte)'o')
						color = _colors[sandIdx++];

					DrawGridCell(x, y, fill: color);
				}

			DrawGridMouseCursor(style: Graphics.GridCursorStyles.SimpleFill);

			DrawText(new v2i(32, 32), v2i.Zero,
				new Text($"sand: {_sandCount}"));
		}

		protected override void Destroy()
		{
			ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			AddSand(MouseGridPositionDiscrete);
		}
	}
}
