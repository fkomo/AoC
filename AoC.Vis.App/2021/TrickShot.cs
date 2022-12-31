using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class TrickShot : Sdl2Loop
	{
		private v2i[] _path = null;
		private AABBi _target;
		private bool _hit = false;
		private v2i _dir;

		public TrickShot(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(false);

			var input = new Ujeby.AoC.App.Year2021.Day17.TrickShot().ReadInput();
			_target = Ujeby.AoC.App.Year2021.Day17.TrickShot.CreateTarget(input);
		}

		protected override void Update()
		{

		}

		protected override void Render()
		{
			DrawGrid(showMinor: false);

			DrawGridRect((int)_target.Min.X, -(int)_target.Min.Y + 1, (int)_target.Size.X + 1, -(int)_target.Size.Y - 1, 
				fill: new v4f(0, 0, 1, 0.5));

			if (_path != null )
			{
				foreach (var p in _path)
					DrawGridCell((int)p.X, -(int)p.Y, fill: _hit ? new v4f(0, 1, 0, 0.5) : new v4f(1, 0, 0, 0.5));
			}

			DrawGridMouseCursor(
				style: GridCursorStyles.FullRowColumn);

			DrawText(new v2i(32, 32), v2i.Zero,
				new Text($"max height: {_path?.Max(p => p.Y)}"),
				new Text($"direction: {_dir}"));
		}

		protected override void Destroy()
		{
			ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			_dir = MouseGridPositionDiscrete;
			_dir.Y = -_dir.Y;

			_hit = Ujeby.AoC.App.Year2021.Day17.TrickShot.SimThrow(_dir, _target, out _path);
		}
	}
}
