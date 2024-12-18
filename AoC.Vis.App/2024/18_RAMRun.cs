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
	internal class RAMRun : AoCRunnable
	{
		int step = 10;

		string[] _input;

		int _bytesToUse;
		v2i[] _bytes;

		v2i _size;
		v2i[] _path;

		public override string Name => $"#18 {nameof(RAMRun)}";

		public RAMRun(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			//_input = InputProvider.Read(AppSettings.InputDirectory, 2024, 18, suffix: ".sample");
			//_size = new v2i(7);

			_input = InputProvider.Read(AppSettings.InputDirectory, 2024, 18);
			_size = new v2i(71);

			_bytesToUse = 1;

			UpdatePath();
		}

		protected override void Update()
		{

		}

		protected override void Render()
		{
			Grid.Draw();

			var byteColor = new v4f(.5);
			foreach (var p in _bytes)
				Grid.DrawCell(p, fill: byteColor);

			Grid.DrawRect(v2i.Zero, _size, border: Colors.Blue);

			for (var i = 0; i < _path.Length; i++)
				Grid.DrawCell(_path[i], fill: HeatMap.GetColorForValue(i, _path.Length, alpha: .5));

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"{nameof(_size)}: {_size}"),
				new Text($"{nameof(_bytesToUse)}: {_bytesToUse}"),
				new Text($"last: {_bytes[^1]}"),
				new Text($"{nameof(_path.Length)}: {_path.Length}")
				);

			base.Render();
		}

		protected override void LeftMouseUp()
		{
			_bytesToUse = (_bytesToUse + step) % _input.Length;
			UpdatePath();
		}

		void UpdatePath()
		{
			_bytes = _input.Take(_bytesToUse).Select(x => new v2i(x.ToNumArray())).ToArray();

			var memSpace = new aab2i(v2i.Zero, _size - 1);
			var empty = memSpace.EnumPoints().Except(_bytes.Take(_bytesToUse)).ToArray();

			var neighbours = Enumerable.Range(0, empty.Length)
				.ToDictionary(
					x => x,
					x => v2i.UpDownLeftRight
						.Select(xx => Array.IndexOf(empty, empty[x] + xx))
						.Where(xx => xx != -1)
						.ToArray());

			int[] Neighbours(int node) => neighbours[node];

			_path = Ujeby.Alg.Graph.BreadthFirstSearch(Array.IndexOf(empty, v2i.Zero), Array.IndexOf(empty, _size - 1), Neighbours)
				.Select(x => empty[x])
				.ToArray();
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
