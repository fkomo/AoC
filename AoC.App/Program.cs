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

			// set aoc session cookie
			if (!string.IsNullOrEmpty(config["aoc:session"]) && File.Exists(config["aoc:session"]))
			{
				var session = File.ReadAllText(config["aoc:session"]);
				AoCHttpClient.SetSession(session);
			}

			// download input files
			if (!string.IsNullOrEmpty(config["aoc:input"]))
			{
				for (var year = 2015; year <= DateTime.Now.Year; year++)
					InputProvider.DownloadMissingInput(config["aoc:input"], year);
			}

#if _DEBUG
			// generate (empty) puzzle .cs classes
			if (!string.IsNullOrEmpty(config["aoc:code"]))
			{
				for (var year = 2015; year <= DateTime.Now.Year; year++)
					AdventOfCode.GeneratePuzzleCode(config["aoc:code"], year);
			}
#endif
			bool.TryParse(config["aoc:ignoreSkip"], out bool ignoreSkip);

			// run all puzzles in solution
			AdventOfCode.RunAll(
				inputStorage: config["aoc:input"], 
				ignoreSkip: ignoreSkip);
		}
	}
}
