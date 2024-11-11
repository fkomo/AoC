using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
    internal class TrickShot : AoCRunnable
	{
		private v2i[] _path = null;
		private AABox2i _target;
		private bool _hit = false;
		private v2i _dir;

		public override string Name => $"#17 {nameof(TrickShot)}";

		public TrickShot(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			Sdl2Wrapper.ShowCursor(false);

			var input = InputProvider.Read(AppSettings.InputDirectory, 2021, 17);
			_target = AoC.App._2021_17.TrickShot.CreateTarget(input);
		}

		protected override void Update()
		{

		}

		protected override void Render()
		{
			Grid.Draw(showMinor: false);

			Grid.DrawRect((int)_target.Min.X, -(int)_target.Min.Y + 1, (int)_target.Size.X + 1, -(int)_target.Size.Y - 1, 
				fill: new v4f(0, 0, 1, 0.5));

			if (_path != null)
			{
				foreach (var p in _path)
				{
					var color = _hit ? new v4f(0, 1, 0, 0.7) : new v4f(1, 0, 0, 0.7);
					Grid.DrawCell((int)p.X, -(int)p.Y, fill: color);
				}

				if (_path.Length > 1)
					for (var p = 0; p < _path.Length - 1; p++)
					{
						var color = _hit ? new v4f(0, 1, 0, 0.3) : new v4f(1, 0, 0, 0.3);
						Grid.DrawLine((int)_path[p].X, -(int)_path[p].Y, (int)_path[p + 1].X, -(int)_path[p + 1].Y, color);
					}
			}

			Grid.DrawMouseCursor(
				style: GridCursorStyles.FullRowColumn);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"max height: {_path?.Max(p => p.Y)}"),
				new Text($"direction: {_dir}"));
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			_dir = Grid.MousePositionDiscrete;
			_dir.Y = -_dir.Y;

			_hit = AoC.App._2021_17.TrickShot.SimThrow(_dir, _target, out _path);
		}
	}
}
