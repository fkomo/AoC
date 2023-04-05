using Microsoft.Extensions.Configuration;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics.Interfaces;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class Program
	{
		static void Main()
		{
			var config = new ConfigurationBuilder().AddJsonFile($"appsettings.json").Build();
			AppSettings.InputDirectory = config["aoc:input"];

			try
			{
				v2i windowSize = new(1920, 1080);
				Sdl2Wrapper.CreateWindow("AoC.Vis", windowSize);
				Sdl2Wrapper.CreateFont();

				while (true)
				{
					var menu = new AoCMenu(windowSize,
						new Dictionary<string, IRunnable[]>
						{
							{
								"2015",
								new IRunnable[]
								{
									new LikeAGIFForYourYard(windowSize),
								}
							},
							{
								"2021",
								new IRunnable[]
								{
									new Chitron(windowSize),
									new TrickShot(windowSize),
									new TrenchMap(windowSize),
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
