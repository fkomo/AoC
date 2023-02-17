using Microsoft.Extensions.Configuration;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			var config = new ConfigurationBuilder().AddJsonFile($"appsettings.json").Build();

			// download inputs first
			if (!string.IsNullOrEmpty(config["aoc:input"]) && File.Exists(config["aoc:session"]))
			{
				var session = File.ReadAllText(config["aoc:session"]);
				AdventOfCode.SetAoCSession(session);
				
				for (var year = 2015; year <= DateTime.Now.Year; year++)
					AdventOfCode.DownloadMissingInput(config["aoc:input"], year);
			}

			AdventOfCode.RunAll(
				inputStorage: config["aoc:input"],
				code: config["aoc:code"]);
		}
	}
}
