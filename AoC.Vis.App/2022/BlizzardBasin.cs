using System.Diagnostics;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;
using static Ujeby.AoC.App.Year2022.Day24.BlizzardBasin;

namespace Ujeby.AoC.Vis.App
{
	internal class BlizzardBasin : Sdl2Loop
	{
		private v2i _mapSize;
		private Blizzard[] _blizzards = null;
		private byte[,] _map = null;
		private long _time = 0;

		private v3i[] _elves;

		private const int _frameStep = 16;

		private v2i _end;
		private v2i _start;
		private v2i _destination;

		private Stopwatch _sw = Stopwatch.StartNew();

		private int[,] _mapUsage;

		private static v4f _wallColor = new(.8);
		private static readonly Dictionary<char, v4f> _blizzColors = new()
		{
			{ '>', new(.5, .7) },
			{ 'v', new(.4, .7) },
			{ '<', new(.3, .7) },
			{ '^', new(.2, .7) }
		};

		public BlizzardBasin(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(false);

			var input = new Ujeby.AoC.App.Year2022.Day24.BlizzardBasin().ReadInput();

			_mapSize = new(input[0].Length, input.Length);
			_blizzards = Ujeby.AoC.App.Year2022.Day24.BlizzardBasin.ParseBlizzards(input);

			_start = new(1, 0);
			_end = new(_mapSize.X - 2, _mapSize.Y - 1);
			
			_elves = new[] { new v3i(_start, 0) };
			_destination = _end;

			_map = Ujeby.AoC.App.Year2022.Day24.BlizzardBasin.GetMapInTime(0, _blizzards, _mapSize);
			_mapUsage = new int[_mapSize.Y, _mapSize.X];

			MinorGridSize = 12;
			SetGridCenter(_mapSize / 2 * MinorGridSize);
		}

		protected override void Update()
		{
			if (_sw.ElapsedMilliseconds >= _frameStep)
			{
				Progress();

				_sw.Restart();
			}
		}

		protected override void Render()
		{
			DrawGrid();

			var maxMDist = _mapSize.ManhLength();
			foreach (var e in _elves)
			{
				if (_map[e.Y, e.X] == 0)
					DrawGridCell((int)e.X, (int)e.Y, 
						fill: HeatMap.GetColorForValue(v2i.ManhDistance(_destination, e.ToV2i()), maxMDist + 1, .5));
			}

			for (var y = 0; y < _map.GetLength(0); y++)
				for (var x = 0; x < _map.GetLength(1); x++)
				{
					if (_map[y, x] == 0)
						continue;

					if (_map[y,x] == '#')
						DrawGridCell(x, y, fill: _wallColor);

					else
						DrawGridCell(x, y, fill: _blizzColors[(char)_map[y, x]]);
				}

			DrawGridMouseCursor();

			DrawText(new v2i(32, 32), v2i.Zero, 
				new Text($"{_blizzards.Length} winds in {_mapSize.X - 2}x{_mapSize.Y - 2} ({new v2i(_mapSize.X - 2, _mapSize.Y - 2).Area()})"),
				new Text($"time: {_time}"),
				new Text($"elves: {_elves.Length}"));
		}

		protected override void Destroy()
		{
			ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			Progress();
		}

		private void Progress()
		{
			if (_elves.Length != 1 || _elves[0].ToV2i() != _destination)
			{
				_map = Ujeby.AoC.App.Year2022.Day24.BlizzardBasin.GetMapInTime(_time, _blizzards, _mapSize);
				_elves = Ujeby.AoC.App.Year2022.Day24.BlizzardBasin.Step(_time, _blizzards, _elves, _mapSize, _destination, _mapUsage);
				_time++;
			}
			else
			{
				if (_destination == _end)
					_destination = _start;

				else if (_destination == _start)
					_destination = _end;

				_mapUsage = new int[_mapSize.Y, _mapSize.X];
			}
		}
	}
}
