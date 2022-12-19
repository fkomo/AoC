using System.Numerics;
using Ujeby.Common.Drawing;
using Ujeby.Common.Drawing.Entities;

namespace Ujeby.AoC.Vis.App
{
	internal class PyroclasticFlow : BaseLoop
	{
		private string[] _chamber;

		protected override void Init()
		{
			SDL2.SDL.SDL_ShowCursor(0);

			var input = new AoC.App.Year2022.Day17.PyroclasticFlow().ReadInput();
			_chamber = AoC.App.Year2022.Day17.PyroclasticFlow.FallingRocks(2022, input[0])
				.ToArray();
		}

		protected override void Update()
		{

		}

		protected override void Render()
		{
			DrawGrid();

			var chamberColor = Colors.White;
			chamberColor.W = 0.8f;

			var colors = Colors.All;

			DrawGridRect(0, 0, 7, _chamber.Length, chamberColor, fill: false);
			for (var y = 0; y < _chamber.Length; y++)
			{
				for (var x = 0; x < _chamber[y].Length; x++)
				{
					if (_chamber[y][x] == AoC.App.Year2022.Day17.PyroclasticFlow.Empty)
						continue;
					
					DrawGridCell(x, y, colors[_chamber[y][x] - '1']);
				}
			}

			DrawGridCursor();

			DrawTextLines(new Vector2(32, 32), new Text($"tower height: {_chamber.Length}"));
		}

		protected override void Destroy()
		{
			SDL2.SDL.SDL_ShowCursor(1);
		}
	}
}
