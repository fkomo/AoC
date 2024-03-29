﻿using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class HillClimbingAlgorithm : AoCRunnable
	{
		private int[,] _heightMap;

		private const int _stepsPerFrame = 10;

		private Alg.BreadthFirstSearch _bfs;
		private v2i[] _bfsPath = null;

		private v2i _start;
		private v2i _end;

		public override string Name => $"#12 {nameof(HillClimbingAlgorithm)}";

		public HillClimbingAlgorithm(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			Sdl2Wrapper.ShowCursor(false);

			var input = InputProvider.Read(AppSettings.InputDirectory, 2022, 12);
			_heightMap = AoC.App._2022_12.HillClimbingAlgorithm.CreateHeightMap(input, out _start, out _end);
			
			Grid.MinorSize = 10;
			Grid.MoveCenter(new v2i(_heightMap.GetLength(1), _heightMap.GetLength(0)) / 2 * Grid.MinorSize);

			_bfs = new Alg.BreadthFirstSearch(_heightMap, new v2i(_heightMap.GetLength(1), _heightMap.GetLength(0)), _start, 
				AoC.App._2022_12.HillClimbingAlgorithm.CheckHeight);
		}

		protected override void Update()
		{
			if (_bfsPath == null && _bfs != null)
			{
				for (var i = 0; i < _stepsPerFrame; i++)
					if (!_bfs.Step())
						_bfsPath = _bfs.Path(_end);
			}
		}

		protected override void Render()
		{
			var ui = new List<TextLine>();

			Grid.Draw();

			// height map
			if (_heightMap != null)
			{
				var maxHeight = 'z' - 'a' + 2;
				for (var y = 0; y < _heightMap.GetLength(0); y++)
					for (var x = 0; x < _heightMap.GetLength(1); x++)
						Grid.DrawCell(x, y, fill: HeatMap.GetColorForValue(_heightMap[y, x], maxHeight, 0.5f));

				if ((int)Grid.MousePositionDiscrete.X >= 0 && (int)Grid.MousePositionDiscrete.X < _heightMap.GetLength(1) &&
					(int)Grid.MousePositionDiscrete.Y >= 0 && (int)Grid.MousePositionDiscrete.Y < _heightMap.GetLength(0))
				{
					var height = _heightMap[(int)Grid.MousePositionDiscrete.Y, (int)Grid.MousePositionDiscrete.X];
					ui.Add(new Text($"height: {height}/{(char)('a' + height - 1)}"));
				}
			}

			// path
			if (_bfsPath != null)
			{
				ui.Add(new Text($"bfs path: {_bfsPath.Length}"));
				foreach (var p in _bfsPath)
					Grid.DrawCell((int)p.X, (int)p.Y, fill: new v4f(1, 1, 1, .5));
			}
			else if (_bfs != null)
			{
				for (var y = 0; y < _bfs.Size.Y; y++)
					for (var x = 0; x < _bfs.Size.X; x++)
						if (_bfs.Visited[y, x])
							Grid.DrawCell((int)x, (int)y, fill: new v4f(.5, .5, .5, .5));
			}

			Grid.DrawMouseCursor();

			Sdl2Wrapper.DrawText(new v2i(32, 32), ui.ToArray());

			base.Render();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			var m = Grid.MousePositionDiscrete;

			if (_heightMap != null)
			{
				_heightMap[m.Y, m.X] = System.Math.Min('z' - 'a', _heightMap[m.Y, m.X] + 1);

				_bfsPath = null;
				_bfs = new Alg.BreadthFirstSearch(_heightMap, new v2i(_heightMap.GetLength(1), _heightMap.GetLength(0)), _start, 
					AoC.App._2022_12.HillClimbingAlgorithm.CheckHeight);
			}
		}

		protected override void LeftMouseUp()
		{

		}
	}
}
