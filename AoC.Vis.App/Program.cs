using Microsoft.Extensions.Configuration;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
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
				Sdl2Wrapper.SetFont(Ujeby.Graphics.Entities.FontNames.Basic7x11);

				while (true)
				{
					var menu = new AoCMenu(windowSize,
						new Dictionary<string, AoCRunnable[]>
						{
							{
								"2015",
								new AoCRunnable[]
								{
									new LikeAGIFForYourYard(windowSize),
								}
							},
							{
								"2016",
								new AoCRunnable[]
								{
									new AMazeOfTwistyLittleCubicles(windowSize),
									new GridComputing(windowSize),
									new AirDuctSpelunking(windowSize),
								}
							},
							{
								"2017",
								new AoCRunnable[]
								{
									new SpiralMemory(windowSize),
								}
							},
							//{
							//	"2018",
							//	Array.Empty<AoCRunnable>()
							//},
							//{
							//	"2019",
							//	Array.Empty<AoCRunnable>()
							//},
							//{
							//	"2020",
							//	Array.Empty<AoCRunnable>()
							//},
							{
								"2021",
								new AoCRunnable[]
								{
									new Chitron(windowSize),
									new TrickShot(windowSize),
									new TrenchMap(windowSize),
								}
							},
							{
								"2022",
								new AoCRunnable[]
								{
									new BlizzardBasin(windowSize),
									new RopeBridge(windowSize),
									new HillClimbingAlgorithm(windowSize),
									new RegolithReservoir(windowSize),
									new MonkeyMap(windowSize),
									new UnstableDiffusion(windowSize),
								}
							},
							{
								"2023",
								new AoCRunnable[]
								{
									new HauntedWasteland(windowSize),
									new LavaductLagoon(windowSize),
								}
							},
						}
					);
					menu.Run();

					if (menu.Selected == null)
						break;

					menu.Selected.Run();
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
	}
}
