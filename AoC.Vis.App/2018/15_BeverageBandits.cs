using System.Diagnostics;
using Ujeby.AoC.App._2018_15;
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
	internal class BeverageBandits : AoCRunnable
	{
		int _rounds;
		bool _combatEnded;
		int _elvesAp = 1;
		char[][] _map;
		List<Unit> _units;
		Dictionary<Unit, v4f> _colors;

		const int _frameStep = 64;
		readonly Stopwatch _sw = Stopwatch.StartNew();

		public override string Name => $"#15 {nameof(BeverageBandits)}";

		public BeverageBandits(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			Reset();

			Grid.MinorSize = 20;
			Grid.MoveCenter(new v2i(_map.Length / 2 * Grid.MinorSize));
		}

		protected override void Update()
		{
			if (!_combatEnded && _sw.ElapsedMilliseconds > _frameStep)
			{
				if (!Ujeby.AoC.App._2018_15.BeverageBandits.Round(_units, _map))
					_combatEnded = true;

				else
					_rounds++;

				_sw.Restart();
			}
		}

		protected override void Render()
		{
			Grid.Draw();

			var rnd = new Random(_units.GetHashCode()); 
			foreach (var t in _map.ToAAB2i().EnumPoints())
			{
				if (_map.Get(t) != '.')
					Grid.DrawCell(t, fill: new v4f(.5, .5, .5, rnd.NextDouble() * .2 + .4));
			}

			foreach (var u in _units)
				Grid.DrawCell(u.Pos, fill: (u is Elf) ? new v4f(1, 0, 0, u.Hp / 200.0) : new v4f(0, 1, 0, u.Hp / 200.0), border: _colors[u]);

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_rounds)}: {_rounds}"),
				new Text($"{nameof(_elvesAp)}: {_elvesAp}"),
				new Text($"score: {_rounds * _units.Sum(x => x.Hp)}"),
				new Text($"elves: {_units.Count(x => x is Elf)}"),
				new Text($"goblins: {_units.Count(x => x is Goblin)}")
				);

			base.Render();
		}

		protected override void LeftMouseUp()
		{
			_elvesAp++;
			Reset();
		}

		private void Reset()
		{
			//var input = InputProvider.Read(AppSettings.InputDirectory, 2018, 13, suffix: ".sample");
			var input = InputProvider.Read(AppSettings.InputDirectory, 2018, 15);

			_map = Ujeby.AoC.App._2018_15.BeverageBandits.CreateMap(input, out v2i[] elves, out v2i[] goblins);
			_units = Ujeby.AoC.App._2018_15.BeverageBandits.CreateUnitsList(elves, goblins, elvesAp: _elvesAp);
			_rounds = 0;
			_combatEnded = false;

			_colors = _units.ToDictionary(x => x, x => new v4f(v3f.FromRGB(Random.Shared.Next()), 1));
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
