using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class SpiralMemory : AoCRunnable
	{
		private v2i[] _path;

		public override string Name => $"#03 {nameof(SpiralMemory)}";

		public SpiralMemory(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
		}

		protected override void Update()
		{
		}

		protected override void Render()
		{
			Grid.Draw();

			if (_path != null)
			{
				for (var p = 0; p < _path.Length; p++)
					Grid.DrawCell(_path[p], fill: HeatMap.GetColorForValue(p, _path.Length, alpha: 0.5));
			}

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_path.Length)}: {_path?.Length}")
				);

			base.Render();
		}

		protected override void LeftMouseDown()
		{
			var tmp = new List<v2i>();

			var spiralEnum = Ujeby.AoC.App._2017_03.SpiralMemory.EnumSpiral().GetEnumerator();
			for (var i = 0; i < _path?.Length + 1; i++)
			{
				spiralEnum.MoveNext();
				tmp.Add(spiralEnum.Current);
			}

			_path = tmp.ToArray();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
