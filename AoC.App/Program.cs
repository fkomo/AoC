using Microsoft.Extensions.Configuration;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile($"appsettings.json")
				.Build();

			_ = bool.TryParse(config["aoc:ignoreSkip"], out bool ignoreSkip);

			string year = null;
			string inputSuffix = null;
			bool? skipSolved = null;

			//year = "2020";
			//inputSuffix = "s";
			//skipSolved = true;

			if (year == null)
			{
				Console.Write($"{Environment.NewLine}  year [2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023] ? ");
				year = Console.ReadLine();
			}

			if (inputSuffix == null)
			{
				Console.Write($"  puzzle input suffix [s(sample)] ? ");
				inputSuffix = Console.ReadLine();
			}

			if (inputSuffix == "s")
				inputSuffix = "sample";
			if (!string.IsNullOrEmpty(inputSuffix))
				inputSuffix = "." + inputSuffix;

			if (!skipSolved.HasValue)
			{
				Console.Write($"  skip solved [y/n(default)] puzzles ? ");
				skipSolved = Console.ReadLine() == "y";
			}
			
			Log.Line();

			var years = Array.Empty<int>();
			if (!string.IsNullOrEmpty(year))
				years = new int[] { int.Parse(year) };

			// run all puzzles in solution
			AdventOfCode.RunAll(years,
				inputStorage: config["aoc:input"],
				ignoreSkip: ignoreSkip,
				skipSolved: skipSolved.Value,
				inputSuffix: inputSuffix);
		}
	}
}