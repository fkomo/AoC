using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class TrenchMap : Sdl2Loop
	{
		private long _step;
		private bool _paused;

		private string _imageEnhAlg;
		private string[] _image;

		public override string Name => $"#23 {nameof(TrenchMap)}";

		public TrenchMap(v2i windowSize) : base(windowSize)
		{
		}

		protected override void Init()
		{
			ShowCursor(false);

			Grid.MinorSize = 5;

			var input = InputProvider.Read(AppSettings.InputDirectory, 2021, 20);
			_imageEnhAlg = input.First();
			_image = input.Skip(2).ToArray();

			_step = 0;
			_paused = false;
		}

		protected override void Update()
		{
			if (!_paused)
			{
				var enh = AoC.App._2021_20.TrenchMap.EnhanceImage((_image, '.'), _imageEnhAlg);

				_image = enh.Image;
				_step++;
			}
		}

		protected override void Render()
		{
			DrawGrid(showMinor: false);

			var pixelColor = new v4f(.5);
			var imageSize = new v2i(_image[0].Length, _image.Length);
			var p = new v2i();
			for (; p.Y < imageSize.Y; p.Y++)
				for (p.X = 0; p.X < imageSize.X; p.X++)
				{
					if (_image[p.Y][(int)p.X] == '.')
						continue;

					DrawGridCell(p - imageSize / 2, fill: pixelColor);
				}

			DrawGridRect(imageSize.Inv() / 2, imageSize, new v4f(0, 0, 1, .5));

			DrawGridMouseCursor();

			DrawText(new v2i(32, 32), 
				new Text($"step: {_step}"),
				new Text($"paused: {_paused}"),
				new Text($"size: {imageSize}"));
		}

		protected override void Destroy()
		{
			ShowCursor();
		}

		protected override void LeftMouseUp()
		{
			_paused = !_paused;
		}
	}
}
