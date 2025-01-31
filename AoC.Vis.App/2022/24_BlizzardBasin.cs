﻿using System.Diagnostics;
using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;
using static Ujeby.AoC.App._2022_24.BlizzardBasin;

namespace Ujeby.AoC.Vis.App
{
	internal class BlizzardBasin : AoCRunnable
	{
		v2i _mapSize;
		Blizzard[] _blizzards = null;
		byte[,] _map = null;
		long _time = 0;

		v3i[] _elves;

		const int _frameStep = 16;

		v2i _end;
		v2i _start;
		v2i _destination;

		Stopwatch _sw = Stopwatch.StartNew();

		int[,] _mapUsage;

		static v4f _wallColor = new(.8);
		static readonly Dictionary<char, v4f> _blizzColors = new()
		{
			{ '>', new(.5, .7) },
			{ 'v', new(.4, .7) },
			{ '<', new(.3, .7) },
			{ '^', new(.2, .7) }
		};

		public override string Name => $"#24 {nameof(BlizzardBasin)}";

		public BlizzardBasin(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			Sdl2Wrapper.ShowCursor(false);

			var input = InputProvider.Read(AppSettings.InputDirectory, 2022, 24);

			_mapSize = new(input[0].Length, input.Length);
			_blizzards = Ujeby.AoC.App._2022_24.BlizzardBasin.ParseBlizzards(input);

			_start = new(1, 0);
			_end = new(_mapSize.X - 2, _mapSize.Y - 1);
			
			_elves = [new v3i(_start, 0)];
			_destination = _end;

			_map = Ujeby.AoC.App._2022_24.BlizzardBasin.GetMapInTime(0, _blizzards, _mapSize);
			_mapUsage = new int[_mapSize.Y, _mapSize.X];

			Grid.MinorSize = 12;
			Grid.SetCenter(_mapSize / 2 * Grid.MinorSize);
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
			Grid.Draw();

			var maxMDist = _mapSize.ManhLength();
			foreach (var e in _elves)
			{
				if (_map[e.Y, e.X] == 0)
					Grid.DrawCell((int)e.X, (int)e.Y, 
						fill: HeatMap.GetColorForValue(v2i.ManhDistance(_destination, e.ToV2i()), maxMDist + 1, .5));
			}

			for (var y = 0; y < _map.GetLength(0); y++)
				for (var x = 0; x < _map.GetLength(1); x++)
				{
					if (_map[y, x] == 0)
						continue;

					if (_map[y,x] == '#')
						Grid.DrawCell(x, y, fill: _wallColor);

					else
						Grid.DrawCell(x, y, fill: _blizzColors[(char)_map[y, x]]);
				}

			Grid.DrawMouseCursor();

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{_blizzards.Length} blizzards in {_mapSize.X - 2}x{_mapSize.Y - 2} ({new v2i(_mapSize.X - 2, _mapSize.Y - 2).Area()})"),
				new Text($"time: {_time}"),
				new Text($"elves: {_elves.Length}"));

			base.Render();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			Progress();
		}

		void Progress()
		{
			if (_elves.Length != 1 || _elves[0].ToV2i() != _destination)
			{
				_map = Ujeby.AoC.App._2022_24.BlizzardBasin.GetMapInTime(_time, _blizzards, _mapSize);
				_elves = Ujeby.AoC.App._2022_24.BlizzardBasin.Step(_time, _blizzards, _elves, _mapSize, _destination, _mapUsage);
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
