using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App.Ui
{
	public class AoCRunnable : Sdl2Loop
	{
		public AoCRunnable(v2i windowSize) : base(windowSize)
		{
			_fpsUpdate = 1;
		}

		public override string Name => null;

		public virtual void Run()
		{
			Run(HandleUserInput);
		}

		protected override void Render()
		{
			Sdl2Wrapper.DrawText(new v2i(WindowSize.X - 64, 32), null,
				new Text($"{(int)Fps}", Colors.Yellow, Colors.Black)
				);
		}
	}
}
