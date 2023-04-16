using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class UnstableDiffusion : AoCRunnable
	{
		private AoC.App._2022_23.UnstableDiffusion.Elf[] _elves = null;

		private long _step;
		private int _direction;
		private bool _noMovement;

		public override string Name => $"#23 {nameof(UnstableDiffusion)}";

		public UnstableDiffusion(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			Sdl2Wrapper.ShowCursor(false);

			Grid.MinorSize = 4;

			var input = InputProvider.Read(AppSettings.InputDirectory, 2022, 23);
			_elves = AoC.App._2022_23.UnstableDiffusion.ParseElves(input);

			var min = new v2i(_elves.Min(e => e.Position.X), _elves.Min(e => e.Position.Y));
			var max = new v2i(_elves.Max(e => e.Position.X), _elves.Max(e => e.Position.Y));
			Grid.MoveCenter((max + min) / 2 * Grid.MinorSize);

			_step = 0;
			_direction = 0;
		}

		protected override void Update()
		{
		}

		protected override void Render()
		{
			Grid.Draw();

			var maxStep = _elves.Max(e => e.Steps);

			foreach (var elf in _elves)
				Grid.DrawCell((int)elf.Position.X, (int)elf.Position.Y, fill: HeatMap.GetColorForValue(elf.Steps, maxStep + 1, alpha: 0.7f));

			var min = new v2i(_elves.Min(e => e.Position.X), _elves.Min(e => e.Position.Y));
			var max = new v2i(_elves.Max(e => e.Position.X), _elves.Max(e => e.Position.Y));

			Grid.DrawRect((int)min.X, (int)min.Y, (int)(max.X - min.X + 1), (int)(max.Y - min.Y + 1), new v4f(0, 0, 1, 1));

			Grid.DrawMouseCursor();

			var empty = ((max - min) + new v2i(1, 1)).Area() - _elves.Length;

			Sdl2Wrapper.DrawText(new v2i(32, 32), 
				new Text($"step: {_step}"),
				new Text($"movement: {!_noMovement}"),
				new Text($"area: {min}x{max}"),
				new Text($"empty: {empty}"));
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			if (!_noMovement)
			{
				_elves = AoC.App._2022_23.UnstableDiffusion.Step(_elves, _direction, out _noMovement);

				_step++;
				_direction = (int)(_step % 4);

				// center grid
				//var min = new v2i(_elves.Min(e => e.Position.X), _elves.Min(e => e.Position.Y));
				//var max = new v2i(_elves.Max(e => e.Position.X), _elves.Max(e => e.Position.Y));
				//Grid.SetCenter((max + min) / 2 * Grid.MinorSize);
			}
		}
	}
}
