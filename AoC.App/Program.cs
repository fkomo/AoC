using Microsoft.Extensions.Configuration;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	internal class Settings
	{
		public const string ConfigSection = "AoC";

		public int[] Years { get; set; }

		public bool? SkipSolved { get; set; } = true;
		public bool IgnoreSkip { get; set; } = false;

		public string Input { get; set; }
		public string InputSuffix { get; set; }
	}

	public class Program
	{
		static void Main(string[] args)
		{
			string env = null;
			var envFilePath = Path.Combine(AppContext.BaseDirectory, "env.txt");
			if (File.Exists(envFilePath))
				env = File.ReadAllText(envFilePath);

			var settingsFile = (string.IsNullOrEmpty(env)) ? "appsettings.json" : $"appsettings.{env}.json";

			var config = new ConfigurationBuilder()
				.AddJsonFile(settingsFile)
				.Build();

			var settings = new Settings();
			config.Bind(Settings.ConfigSection, settings);

			if (settings.Years == null || settings.Years.Length < 1)
			{
				Console.Write($"{Environment.NewLine}  Year [2015, .., {DateTime.Now.Year}] ? ");
				settings.Years = [int.Parse(Console.ReadLine())];
			}
			else
				Console.WriteLine($"{Environment.NewLine}  Years: {string.Join(", ", settings.Years)}");

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

			if (!settings.SkipSolved.HasValue)
			{
				Console.Write($"  Skip solved [y/n(default)] puzzles ? ");
				settings.SkipSolved = Console.ReadLine() == "y";
			}
			else
				Console.WriteLine($"  Skip solved puzzles: {settings.SkipSolved}"); 
			
			Log.Line();

			// run all puzzles in solution
			AdventOfCode.RunAll(settings.Years, 
				inputStorage: settings.Input, ignoreSkip: settings.IgnoreSkip, skipSolved: settings.SkipSolved.Value, inputSuffix: settings.InputSuffix);
		}

		public static void Init()
		{
		}
	}
}