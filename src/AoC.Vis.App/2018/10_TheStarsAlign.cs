using Ujeby.AoC.App._2018_10;
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
	internal class TheStarsAlign : AoCRunnable
	{
		long _second;

		Star[] _stars;

		public override string Name => $"#10 {nameof(TheStarsAlign)}";

		public TheStarsAlign(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			//var input = InputProvider.Read(AppSettings.InputDirectory, 2018, 10, suffix: ".sample");
			var input = InputProvider.Read(AppSettings.InputDirectory, 2018, 10);

			_second = 0;
			_stars = [.. input.Select(x => x.ToNumArray()).Select(x => new Star(new v2i(x[0], x[1]), new v2i(x[2], x[3])))];
		}

		protected override void Update()
		{
			var starsInTime = _stars.Select(x => x.Pos + x.Vel * _second).ToArray();

			if (starsInTime.ContainsLine(8))
				return;

			_second++;
		}

		protected override void Render()
		{
			Grid.Draw(showMinor: false);

			var starsInTime = _stars.Select(x => x.Pos + x.Vel * _second).ToHashSet();

			var starsWithNeighbours = starsInTime.Where(x => v2i.UpDownLeftRight.Any(d => starsInTime.Contains(x + d)));

			var textArea = aab2i.FromPoints([.. starsWithNeighbours]);
			var textPoints = starsInTime.Where(textArea.Contains).ToArray();
			var textPointsCentered = textPoints.Select(x => x - (textArea.Min + textArea.Max) / 2);

			Grid.DrawCells(textPointsCentered, fill: new v4f(1, 1, 1, .5));

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_second)}: {_second}")
				);

			base.Render();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
