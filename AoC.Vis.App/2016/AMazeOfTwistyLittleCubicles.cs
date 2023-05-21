using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class AMazeOfTwistyLittleCubicles : AoCRunnable
	{
		private v2i _mazeSize;
		private bool[,] _maze;
		private long _officeDesignerFavNum;

		private v2i _start = new(1, 1);
#if _DEBUG_SAMPLE
		private v2i _destination = new(7, 4);
		private long _maxSteps = 10;
#else
		private v2i _destination = new(31, 39);
		private long _maxSteps = 50;
#endif

		private v2i[] _path;
		private Alg.BreadthFirstSearch _bfs;

		private v2i[] _visitedToMaxSteps;

		public override string Name => $"#18 {nameof(AMazeOfTwistyLittleCubicles)}";

		public AMazeOfTwistyLittleCubicles(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, 2016, 13);
			_officeDesignerFavNum = long.Parse(input.Single());

			_mazeSize = _start + new v2i(_maxSteps);
			_maze = new bool[_mazeSize.X, _mazeSize.Y];

			Grid.MinorSize = (int)(WindowSize.Y / _mazeSize.Y / 2);
			Grid.MoveCenter(_mazeSize / 2 * Grid.MinorSize);

			CreateMaze(_mazeSize);

			_bfs = new Ujeby.Alg.BreadthFirstSearch(null, _mazeSize, _start, (p1, p2, map) =>
			{
				return !AoC.App._2016_13.AMazeOfTwistyLittleCubicles.WallAtCached(p1, _officeDesignerFavNum);
			});
			_bfs.StepFull();

			GetPath(_destination);
		}

		private void CreateMaze(v2i mazeSize)
		{
			_mazeSize = mazeSize;
			for (var i = 0; i < _mazeSize.Area(); i++)
			{
				var p = new v2i(i % _mazeSize.X, i / _mazeSize.X);
				_maze[p.X, p.Y] = AoC.App._2016_13.AMazeOfTwistyLittleCubicles.WallAtCached(p, _officeDesignerFavNum);
			}
		}

		protected override void Update()
		{
		}

		protected override void Render()
		{
			Grid.Draw();

			var p = new v2i();
			for (p.Y = 0; p.Y < _mazeSize.Y; p.Y++)
				for (p.X = 0; p.X < _mazeSize.X; p.X++)
					if (_maze[p.X, p.Y])
						Grid.DrawCell(p, fill: new v4f(0.4f));

			Grid.DrawCell(_start, fill: Colors.Blue);
			Grid.DrawCellsHeatPath(_path);

			Grid.DrawCells(_bfs.VisitedHashSet, border: new v4f(1, 1, 1, .5));
			Grid.DrawCells(_visitedToMaxSteps, fill: new v4f(.8, .8, .8, .5));

			Grid.DrawRect(new v2i(), new v2i(_mazeSize), new v4f(0, 0, 1, 1));

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_mazeSize)}: {_mazeSize}"),
				new Text($"{nameof(_officeDesignerFavNum)}: {_officeDesignerFavNum}"),
				new Text($"{nameof(_destination)}: {_destination}"),
				new Text($"{nameof(_path.Length)}: {_path?.Length}"),
				new Text($"{nameof(_visitedToMaxSteps)}: {_visitedToMaxSteps.Length}")
				);

			base.Render();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			_path = null;
		}

		private void GetPath(v2i destination)
		{
			_destination = destination;
			AoC.App._2016_13.AMazeOfTwistyLittleCubicles.Reset();
			_path = AoC.App._2016_13.AMazeOfTwistyLittleCubicles.Path(_start, _destination, _officeDesignerFavNum, new v2i[] { _start })
				.Skip(1).ToArray();

			_visitedToMaxSteps = _bfs.VisitedHashSet.Where(p => _bfs.Path(p).Length <= _path.Length)
				.Distinct().ToArray();
		}

		protected override void LeftMouseUp()
		{
			var p = Grid.MousePositionDiscrete;
			if (p.X >= _mazeSize.X || p.Y >= _mazeSize.Y || p.X < 0 || p.Y < 0)
				return;

			if (AoC.App._2016_13.AMazeOfTwistyLittleCubicles.WallAt(p, _officeDesignerFavNum))
				return;

			GetPath(Grid.MousePositionDiscrete);
		}
	}
}
