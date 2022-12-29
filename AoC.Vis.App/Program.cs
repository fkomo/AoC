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
					var menu = new AoCMenu(windowSize,
						new Dictionary<string, IRunnable[]>
						{
							{ 
								"2020", 
								new IRunnable[]
								{
									new Dummy(windowSize),
								}
							},
							{ 
								"2021", 
								new IRunnable[]
								{
									new Chitron(windowSize),
								}
							},
							{ 
								"2022", 
								new IRunnable[]
								{
									new BlizzardBasin(windowSize),
									new RopeBridge(windowSize),
									new HillClimbingAlgorithm(windowSize),
									new RegolithReservoir(windowSize),
									new MonkeyMap(windowSize),
									new UnstableDiffusion(windowSize),
								}
							}
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
