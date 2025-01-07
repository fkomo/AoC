using Microsoft.Extensions.Configuration;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	internal class Settings
	{
		public const string ConfigSection = "AoC";

		public string[] Puzzles { get; set; }

		public bool IgnoreSkip { get; set; } = false;

		public string Input { get; set; }
		public string InputSuffix { get; set; }
	}

	public class Program
	{
		static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddJsonFile(AdventOfCode.GetSettingsFilename())
				.Build();

			var settings = new Settings();
			config.Bind(Settings.ConfigSection, settings);

			Console.WriteLine($"{Environment.NewLine}  Puzzles: {string.Join(", ", settings.Puzzles.Select(x=> $"{x}"))}");

			if (settings.InputSuffix == null)
			{
				Console.Write($"  Puzzle input suffix [s(sample)] ? ");
				settings.InputSuffix = Console.ReadLine();
			}
			else
				Console.WriteLine($"  Puzzle input suffix: {settings.InputSuffix}");

			if (settings.InputSuffix == "s")
				settings.InputSuffix = "sample";

			if (!string.IsNullOrEmpty(settings.InputSuffix))
				settings.InputSuffix = "." + settings.InputSuffix;

			Log.Line();

			// run all puzzles in solution
			AdventOfCode.RunAll(settings.Puzzles, 
				inputStorage: settings.Input, ignoreSkip: settings.IgnoreSkip, inputSuffix: settings.InputSuffix);
		}

		public static void Init()
		{
		}
	}
}