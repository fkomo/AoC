using Ujeby.AoC.Vis.App.Common;
using Ujeby.Graphics.Sdl;
using Ujeby.Graphics.Sdl.Interfaces;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class Program
	{
		static void Main()
		{
			try
			{
				v2i windowSize = new(1920, 1080);
				Sdl2Wrapper.Init("AoC.Vis", windowSize);

				while (true)
				{
					var menu = new Menu(windowSize,
						new IRunnable[]
						{
							new RopeBridge(windowSize),
							new Chitron(windowSize),
							new HillClimbingAlgorithm(windowSize),
							new RegolithReservoir(windowSize),
							new MonkeyMap(windowSize)
						}
					);
					menu.Run(HandleInput);

					if (menu.Selected == null)
						break;

					menu.Selected.Run(HandleInput);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				Sdl2Wrapper.Destroy();
			}
		}

		private static bool HandleInput()
		{
			return true;
		}
	}
}
