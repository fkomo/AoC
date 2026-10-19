using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class MovieTheater : AoCRunnable
	{
        v2i[] _points;
		v2i[][] _edges;
        v2i[][] _hEdges;
		v2i[][] _vEdges;

		int _rectIdx = 0;
		aab2i[] _rectangles;

        aab2i _bestRect = new();
        aab2i _currentRect;
        bool _currentGood;

        public override string Name => $"#09 {nameof(MovieTheater)}";

		public MovieTheater(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, 2025, 09);

			_points = Ujeby.AoC.App._2025_09.MovieTheater.ParseInput(input)
				.Select(x => x * 0.01f) // scale down
				.ToArray();

            var center = new v2i(_points.Min(x => x.X) + _points.Max(x => x.X) / 2, _points.Min(x => x.Y) + _points.Max(x => x.Y) / 2);

            _points = [.. _points.Select(x => x - center)];

            _edges = Ujeby.AoC.App._2025_09.MovieTheater.BuildEdges(_points, out _hEdges, out _vEdges);

			_rectIdx = 0;
			_rectangles = Ujeby.AoC.App._2025_09.MovieTheater.EnumRectangles(_points).ToArray();

            Grid.MinorSize = 1;
		}

		protected override void Update()
		{
			_currentRect = _rectangles[_rectIdx];
            _currentGood = Ujeby.AoC.App._2025_09.MovieTheater.CheckRectangle(_currentRect, _hEdges, _vEdges);

			if (_currentGood && (_currentRect.Size.Area() > _bestRect.Size.Area()))
				_bestRect = _currentRect;

            _rectIdx = (_rectIdx + 1) % _rectangles.Length;
		}

		protected override void Render()
		{
			var green = new v4f(0, 1, 0, .5);
            var red = new v4f(1, 0, 0, .5);

            var blue = new v4f(0, 0, 1, .3);
            var pink = new v4f(1, 0, 1, .3);

            foreach (var edge in _vEdges)
			{
                for (var y = edge[0].Y + 1; y < edge[1].Y; y++)
                	Grid.DrawCell(new v2i(edge[0].X, y), fill: green);
			}

            foreach (var edge in _hEdges)
            {
                for (var x = edge[0].X + 1; x < edge[1].X; x++)
                    Grid.DrawCell(new v2i(x, edge[0].Y), fill: green);
            }

            // this is inefficient af, but ok for visualization
            Grid.DrawCells(_bestRect.EnumPoints(), fill: blue);
            Grid.DrawCells(_bestRect.EnumBorderPoints(), fill: new v4f(0, 0, 1, 1));

            Grid.DrawCells(_currentRect.EnumPoints(), fill: _currentGood ? green : red);
            Grid.DrawCells(_currentRect.EnumBorderPoints(), fill: _currentGood ? green : red);

            Grid.DrawCells(_points, fill: red);

            Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_bestRect)}: {_bestRect} => {_bestRect.Size.Area()}"),
				new Text($"{nameof(_currentRect)}: {_currentRect} => {_currentRect.Size.Area()}"),
                new Text($"{nameof(_rectIdx)}: {_rectIdx}/{_rectangles.Length} {((float)_rectIdx / _rectangles.Length * 100.0f).ToString("F2")}%")
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
