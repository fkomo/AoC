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
	internal class PrintingDepartment : AoCRunnable
	{
        private char[][] _map;
        private aab2i _aab2i;
        private Dictionary<v2i, int> _neighbours;

        public override string Name => $"#04 {nameof(PrintingDepartment)}";

		public PrintingDepartment(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			//var input = InputProvider.Read(AppSettings.InputDirectory, 2025, 04, suffix: ".sample");
            var input = InputProvider.Read(AppSettings.InputDirectory, 2025, 04);

            _map = Ujeby.AoC.App._2025_04.PrintingDepartment.ParseInput(input);
			_aab2i = _map.ToAAB2i();

            Grid.MinorSize = 7;
        }

        protected override void Update()
		{
            _neighbours = _aab2i.EnumPoints().ToDictionary(x => x, x => v2i.PlusMinusOne.Count(xx => _map.TryGet(xx + x, out char neighbour) && neighbour == '@'));
        }

		protected override void Render()
		{
            Grid.Draw(true, showMajor: true, showMinor: true);
            
			foreach (var p in _aab2i.EnumPoints())
				Grid.DrawCell(p - (_aab2i.Max + _aab2i.Min) / 2, fill: new v4f(1, 1, 1, 1.0 / (double)_neighbours[p]));

            Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

            Sdl2Wrapper.DrawText(new v2i(32, 32), null,
                new Text($"{nameof(_aab2i)}: {_aab2i}")
                );

            base.Render();
		}

		protected override void LeftMouseUp()
		{
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
