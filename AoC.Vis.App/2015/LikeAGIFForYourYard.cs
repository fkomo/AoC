﻿using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class LikeAGIFForYourYard : Sdl2Loop
	{
		private long _step;
		private bool[][] _lights;
		private int _size;

		public override string Name => $"#18 {nameof(LikeAGIFForYourYard)}";

		public LikeAGIFForYourYard(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(true);

			Grid.MinorSize = 8;

			var input = InputProvider.Read(AppSettings.InputDirectory, 2015, 18);
			_size = input.Length;
			_lights = AoC.App._2015_18.LikeAGIFForYourYard.CreateLights(input);

			var min = new v2i(_size / -2);
			var max = new v2i(_size / 2);
			Grid.MoveCenter((max + min) / 2 * Grid.MinorSize);

			_step = 0;
		}

		protected override void Update()
		{

		}

		protected override void Render()
		{
			DrawGrid();

			var p = new v2i();
			for (p.Y = 0; p.Y < _size; p.Y++)
				for (p.X = 0; p.X < _size; p.X++)
					if (_lights[p.Y][p.X])
						DrawGridCell((int)(_size / -2 + p.X), (int)(_size / -2 + p.Y), 
							fill: new v4f(0.7f));

			var min = new v2i(_size / -2);
			var max = new v2i(_size / 2);
			DrawGridRect(min, new v2i(_size), new v4f(0, 0, 1, 1));

			DrawGridMouseCursor();

			DrawText(new v2i(32, 32), 
				new Text($"step: {_step}"),
				new Text($"lights: {_lights.Sum(i => i.Count(c => c))}"));
		}

		protected override void Destroy()
		{
			//ShowCursor();
		}

		protected override void LeftMouseDown()
		{
			_lights = AoC.App._2015_18.LikeAGIFForYourYard.GameOfLifeStepWithFixedCorners(_lights);
			_step++;
		}
	}
}
