using Microsoft.Extensions.Configuration;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			var config = new ConfigurationBuilder().AddJsonFile($"appsettings.json").Build();

			var session = string.Empty;
			if (File.Exists(config["aoc:session"]))
				session = File.ReadAllText(config["aoc:session"]);

			AdventOfCode.RunAll(
				session: session, 
				inputStorage: config["aoc:input"],
				code: config["aoc:code"]);
		}
	}
}
