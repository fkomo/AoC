using System.Diagnostics;
using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class ALongWalk : AoCRunnable
	{
		public override string Name => $"#{_puzzle.Day:d2} {nameof(ALongWalk)}";

		AoC.App._2023_23.ALongWalk _puzzle = new();

		private string[] _input;
		int _currentPath = 0;
		v2i[][] _allPaths;

		readonly Stopwatch _sw = Stopwatch.StartNew();
		const int _frameStep = 1;

		readonly Dictionary<char, v4f> _colors = new()
		{
			{ '.', new v4f(0) },
			{ '#', new v4f(1, 1, 1, .2) },
			{ '>', new v4f(1, 1, 1, 1) },
			{ 'v', new v4f(1, 1, 1, 1) },
		};

		protected Gui _gui = new();

		public ALongWalk(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			_input = InputProvider.Read(AppSettings.InputDirectory, _puzzle.Year, _puzzle.Day);//, ".sample");

			var allPaths = new List<v2i[]>();
			AoC.App._2023_23.ALongWalk.LongestHike(_input, false, allPaths);
			_allPaths = allPaths.ToArray();

			Grid.MinorSize = 5;
			Grid.MoveCenter(new v2i(_input.Length, _input.Length) / 2 * Grid.MinorSize);

			// TODO select path to draw from gui list
		}

		protected override void Update()
		{
			_gui.Update(MousePosition);
		}

		protected override void Render()
		{
			Grid.Draw(true, true, true);

			for (var y = 0; y < _input.Length; y++)
				for (var x = 0; x < _input[y].Length; x++)
					Grid.DrawCell(new v2i(x, y), fill: _colors[_input[y][x]]);

			if (_allPaths != null)
				Grid.DrawCells(_allPaths[_currentPath]);

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"path {_currentPath}/{_allPaths.Length - 1} [{_allPaths[_currentPath].Length - 1} steps]")
				);

			_gui.Render();

			base.Render();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			_gui.LeftMouseDown(MousePosition);
		}

		protected override void LeftMouseUp()
		{
			_gui.LeftMouseUp(MousePosition);
			_currentPath = (_currentPath + 1) % _allPaths.Length;
		}
	}
}
