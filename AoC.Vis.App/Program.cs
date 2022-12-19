using Ujeby.Common.Drawing;
using Ujeby.Common.Drawing.Interfaces;

namespace Ujeby.AoC.Vis.App
{
	internal class Program
	{
		static void Main()
		{
			try
			{
				Core.Init();

				var menu = new Menu(
					new IRunnable[]
					{
						new RopeBridge(),
						new Chitron(),
						new HillClimbingAlgorithm(),
						new RegolithReservoir(),
						new PyroclasticFlow(),
					}
				);
				
				menu.Run(HandleInput);

				menu.Selected?.Run(HandleInput);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				Core.Destroy();
			}
		}

		private static bool HandleInput()
		{
			return true;
		}
	}
}
