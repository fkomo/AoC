using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class UnstableDiffusion : Sdl2Loop
	{
		private AoC.App.Year2022.Day23.UnstableDiffusion.Elf[] _elves = null;

		private long _step;
		private int _direction;
		private bool _noMovement;

		public override string Name => $"#23 {nameof(UnstableDiffusion)}";

		public UnstableDiffusion(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(false);

			Grid.MinorSize = 4;

			var input = new AoC.App.Year2022.Day23.UnstableDiffusion().ReadInput();
			_elves = AoC.App.Year2022.Day23.UnstableDiffusion.ParseElves(input);

			var min = new v2i(_elves.Min(e => e.Position.X), _elves.Min(e => e.Position.Y));
			var max = new v2i(_elves.Max(e => e.Position.X), _elves.Max(e => e.Position.Y));
			Grid.MoveCenter((max + min) / 2 * Grid.MinorSize);

			_step = 0;
			_direction = 0;
		}

		protected override void Update()
		{
			if (!_noMovement)
			{
				_elves = AoC.App.Year2022.Day23.UnstableDiffusion.Step(_elves, _direction, out _noMovement);

				_step++;
				_direction = (int)(_step % 4);

				var min = new v2i(_elves.Min(e => e.Position.X), _elves.Min(e => e.Position.Y));
				var max = new v2i(_elves.Max(e => e.Position.X), _elves.Max(e => e.Position.Y));
				Grid.SetCenter((max + min) / 2 * Grid.MinorSize);
			}
		}

		protected override void Render()
		{
			DrawGrid();

			var maxStep = _elves.Max(e => e.Steps);

			foreach (var elf in _elves)
				DrawGridCell((int)elf.Position.X, (int)elf.Position.Y, fill: HeatMap.GetColorForValue(elf.Steps, maxStep + 1, alpha: 0.7f));

			var min = new v2i(_elves.Min(e => e.Position.X), _elves.Min(e => e.Position.Y));
			var max = new v2i(_elves.Max(e => e.Position.X), _elves.Max(e => e.Position.Y));

			DrawGridRect((int)min.X, (int)min.Y, (int)(max.X - min.X + 1), (int)(max.Y - min.Y + 1), new v4f(0, 0, 1, 1));

			DrawGridMouseCursor();

			var empty = ((max - min) + new v2i(1, 1)).Area() - _elves.Length;

			DrawText(new v2i(32, 32), v2i.Zero, 
				new Text($"step: {_step}"),
				new Text($"movement: {!_noMovement}"),
				new Text($"area: {min}x{max}"),
				new Text($"empty: {empty}"));
		}
		protected override void Destroy()
		{
			ShowCursor();
		}
	}
}
