using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App.Ui
{
	public class AoCRunnable : Sdl2Loop
	{
		public AoCRunnable(v2i windowSize) : base(windowSize)
		{
		}

		public override string Name => null;

		public virtual void Run()
		{
			Run(HandleUserInput);
		}
	}
}
