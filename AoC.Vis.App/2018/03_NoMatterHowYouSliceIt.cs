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
	internal class NoMatterHowYouSliceIt : AoCRunnable
	{
		Dictionary<long, v4f> _colors;
		Dictionary<long, aab2i> _claims;

		public override string Name => $"#03 {nameof(NoMatterHowYouSliceIt)}";

		public NoMatterHowYouSliceIt(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, 2018, 03);

			_claims = input
				.Select(x => x.ToNumArray())
				.ToDictionary(x => x[0], x => new aab2i(new(x[1], x[2]), new(x[1] + x[3] - 1, x[2] + x[4] - 1)));

			_colors = _claims.Keys.ToDictionary(x => x, x => new v4f(v3f.FromRGB(Random.Shared.Next()), .3));
		}

		protected override void Update()
		{
		}

		protected override void Render()
		{
			Grid.Draw(showMinor: false);

			foreach (var claim in _claims)
				Grid.DrawRect(claim.Value.Min, claim.Value.Size, fill: _colors[claim.Key]);

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_claims)}: {_claims.Count}")
				);

			base.Render();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
